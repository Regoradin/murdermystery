using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;
using static Story;

public class StoryNode
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public List<ConnectionPoint> outPoints;
    
    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<StoryNode> OnRemoveNode;

    private GUIStyle outPointStyle;
    private Action<ConnectionPoint> OnClickOutPoint;

    public Story story;

    public StoryNode(Vector2 position, float width, float height, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<StoryNode> OnClickRemoveNode, Story story)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.defaultNodeStyle = style;
        this.selectedNodeStyle = selectedStyle;
        this.style = defaultNodeStyle;

        this.outPointStyle = outPointStyle;
        this.OnClickOutPoint = OnClickOutPoint;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoints = new List<ConnectionPoint>();

        this.OnRemoveNode = OnClickRemoveNode;

        if (story != null)
        {
            this.story = story;
        }
        else
        {
            this.story = StoryStructure.Instance.gameObject.AddComponent<Story>();
        }
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
        story.nodePosition = rect.position;

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    public void Draw()
    {
        GUI.Box(rect, title, style);
        inPoint.Draw();

        DrawContents();
        
        if (GUI.changed)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());            
        }


    }

    public bool ProcessEvents(Event e)
    {
        switch(e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    GUI.changed = true;
                }
                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;                
                
            case EventType.MouseUp:
                isDragged = false;
                style = defaultNodeStyle;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        UnityEngine.Object.DestroyImmediate(story);

        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }

    private void DrawContents()
    {
        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 10, rect.width - 20, rect.height - 20));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name: ");
        story.name = GUILayout.TextField(story.name);
        GUILayout.EndHorizontal();

        List<Interaction> newInteractions = new List<Interaction>();

        if (story.interactions != null)
        {
            foreach(Interaction interaction in story.interactions)
            {
                string name = interaction.name;
                Story nextStory = interaction.nextStory;
                Interaction.InteractionType type = interaction.type;
                
                Interaction newInteraction = DrawInteractionField(name, nextStory, type);

                if (newInteraction != null)
                {
                    newInteractions.Add(newInteraction);
                }
            }
        }
        
        Interaction newBlankInteraction = DrawInteractionField(null, null, Interaction.InteractionType.Continue);
        if (newBlankInteraction != null)
        {
            newInteractions.Add(newBlankInteraction);
        }

        DrawAllSnippetFields();

        GUILayout.EndArea();

        for(int i =0; i < newInteractions.Count; i++)
        {
            ConnectionPoint point = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0.15f * (i + 1), newInteractions[i].name);
            point.Draw();

            outPoints.Add(point);
        }

        story.UpdateInteractions(newInteractions);

    }

    private void DrawAllSnippetFields()
    {
        List<Snippet> newSnippets = new List<Snippet>();

        if (story.snippets != null)
        {
            foreach(Snippet snippet in story.snippets)
            {
                Animator anim = snippet.anim;
                string trigger = snippet.trigger;
                float time = snippet.startTime;

                Snippet newSnippet = DrawSnippetField(anim, trigger, time);
                if (newSnippet != null)
                {
                    newSnippets.Add(newSnippet);
                }
            }

        }

        Snippet newBlankSnippet = DrawSnippetField(null, null, 0);
        if (newBlankSnippet != null)
        {
            newSnippets.Add(newBlankSnippet);
        }
        
        story.UpdateSnippets(newSnippets);
    }

    private Snippet DrawSnippetField(Animator anim, string trigger, float time)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Animation:");
        anim = (Animator)EditorGUILayout.ObjectField(anim, typeof(Animator), true);

        GUILayout.EndHorizontal();
            
        GUILayout.BeginHorizontal();

        GUILayout.Label("Trigger:");
        trigger = EditorGUILayout.TextField(trigger);
        GUILayout.Label("Time:");
        time = EditorGUILayout.FloatField(time);

        GUILayout.EndHorizontal();

        if(anim != null)
        {
            return new Snippet(anim, trigger, time);
        }
        else
        {
            return null;
        }

    }

    private Interaction DrawInteractionField(string name, Story nextStory, Interaction.InteractionType type)
    {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Interaction:");
        name = EditorGUILayout.TextField(name);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();

        bool isInterrupt = (type == Interaction.InteractionType.Interrupt);
        GUILayout.Label("Interrupt?");
        isInterrupt = EditorGUILayout.Toggle(isInterrupt);
        
        GUILayout.EndHorizontal();

        if (name != null && name != "")
        {
            return new Interaction(name, null, isInterrupt ? Interaction.InteractionType.Interrupt : Interaction.InteractionType.Continue);
        }
        else
        {
            return null;
        }
    }
}
