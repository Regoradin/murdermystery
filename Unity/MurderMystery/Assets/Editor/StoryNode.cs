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
    public ConnectionPoint outPoint;
    
    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<StoryNode> OnRemoveNode;

    private Story story;

    public StoryNode(Vector2 position, float width, float height, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<StoryNode> OnClickRemoveNode, Story story)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.defaultNodeStyle = style;
        this.selectedNodeStyle = selectedStyle;
        this.style = defaultNodeStyle;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, inPointStyle, OnClickOutPoint);

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
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);
        
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

        HashSet<Snippet> newSnippets = new HashSet<Snippet>();

        if (story.snippets != null)
        {
            foreach(Snippet snippet in story.snippets)
            {
                Animator anim = snippet.anim;
                string trigger = snippet.trigger;
                float time = snippet.startTime;

                Snippet newSnippet = DrawSnippetFields(anim, trigger, time);
                if (newSnippet != null)
                {
                    newSnippets.Add(newSnippet);
                }
            }

        }

        Snippet newBlankSnippet = DrawSnippetFields(null, null, 0);
        if (newBlankSnippet != null)
        {
            newSnippets.Add(newBlankSnippet);
        }

        
        story.UpdateSnippets(newSnippets);

        GUILayout.EndArea();

    }

    private Snippet DrawSnippetFields(Animator anim, string trigger, float time)
    {
        GUILayout.Label("Animation: ");
        anim = (Animator)EditorGUILayout.ObjectField(anim, typeof(Animator), true);
            
        GUILayout.BeginHorizontal();

        GUILayout.Label("Trigger:");
        trigger = GUILayout.TextField(trigger);
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
}
