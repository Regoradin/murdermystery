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

    public Story story
	{
		get { return (Story)EditorUtility.InstanceIDToObject(_storyID); }
		set { _storyID = value.GetInstanceID(); }
	}
	private int _storyID;
    private StoryEditor editor;

    public StoryNode(Vector2 position, float width, float height, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<StoryNode> OnClickRemoveNode, StoryEditor editor, Story story)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.defaultNodeStyle = style;
        this.selectedNodeStyle = selectedStyle;
        this.style = defaultNodeStyle;
        this.editor = editor;

        this.outPointStyle = outPointStyle;
        this.OnClickOutPoint = OnClickOutPoint;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);

        this.OnRemoveNode = OnClickRemoveNode;

        if (story != null)
        {
            this.story = story;
        }
        else
        {
            Debug.Log(StoryStructure.Instance);
            this._storyID = StoryStructure.Instance.gameObject.AddComponent<Story>().GetInstanceID();
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
        rect.height = 170 +
            (50 * (story.animSnippets.Count + story.audioSnippets.Count)) +
            (30 * story.interactions.Count);
        GUI.Box(rect, title, style);
        DrawContents();

        inPoint.Draw();
        if (outPoints != null)
        {
            foreach (ConnectionPoint point in outPoints)
            {
                point.Draw();
            }
        }
        
        if (GUI.changed)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());            
        }


    }

    public void LoadInteractionConnections()
    {
        outPoints = new List<ConnectionPoint>();

        for(int i =0; i < story.interactions.Count; i++)
        {
            ConnectionPoint point = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0.15f * (i + 1), story.interactions[i].name);
            point.Draw();

            outPoints.Add(point);
        }


        
        for (int i = 0; i < story.interactions.Count; i++)
        {
            if (story.interactions[i].nextStory != null)
            {
                ConnectionPoint outPoint = outPoints[i];
                ConnectionPoint inPoint = null;
                foreach (StoryNode node in editor.nodes)
                {
                    if (node.story == story.interactions[i].nextStory)
                    {
                        inPoint = node.inPoint;
                    
                    }
                }
                outPoints.Add(outPoint);
                editor.CreateConnection(outPoint, inPoint);
            }
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

        DrawAllInteractionFields();
        DrawAllSnippetFields();

        GUILayout.EndArea();
    }

    private void DrawAllInteractionFields()
    {
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

        story.UpdateInteractions(newInteractions);
    }

    private void DrawAllSnippetFields()
    {
        List<AnimSnippet> newAnimSnippets = new List<AnimSnippet>();
        List<AudioSnippet> newAudioSnippets = new List<AudioSnippet>();
        if (story.animSnippets != null)
        {
            foreach(AnimSnippet animSnippet in story.animSnippets)
            {
                AnimSnippet newSnippet = null;
                Animator anim = animSnippet.anim;
                string trigger = animSnippet.trigger;
                float time = animSnippet.startTime;
                newSnippet = DrawAnimSnippetField(anim, trigger, time);
                
                if (newSnippet != null)
                {
                    newAnimSnippets.Add(newSnippet);
                }
            }

        }

        AnimSnippet newBlankAnimSnippet = DrawAnimSnippetField(null, null, 0);
        if (newBlankAnimSnippet != null)
        {
            newAnimSnippets.Add(newBlankAnimSnippet);
        }

        if (story.audioSnippets != null)
        {
            foreach(AudioSnippet audioSnippet in story.audioSnippets)
            {
                AudioSnippet newSnippet = null;
                AudioSource audio = audioSnippet.audio;
                float time = audioSnippet.startTime;
                newSnippet = DrawAudioSnippetField(audio, time);

                if (newSnippet != null)
                {
                    newAudioSnippets.Add(newSnippet);
                }
            }

        }

        AudioSnippet newBlankAudioSnippet = DrawAudioSnippetField(null, 0);
        if (newBlankAudioSnippet != null)
        {
            newAudioSnippets.Add(newBlankAudioSnippet);
        }
        
        story.UpdateAnimSnippets(newAnimSnippets);
        story.UpdateAudioSnippets(newAudioSnippets);
    }

    private AnimSnippet DrawAnimSnippetField(Animator anim, string trigger, float time)
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
            return new AnimSnippet(anim, trigger, time);
        }
        else
        {
            return null;
        }

    }

    private AudioSnippet DrawAudioSnippetField(AudioSource audio, float time)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("AdioSource:");
        audio = (AudioSource)EditorGUILayout.ObjectField(audio, typeof(AudioSource), true);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Time:");
        time = EditorGUILayout.FloatField(time);

        GUILayout.EndHorizontal();

        if (audio != null)
        {
            return new AudioSnippet(audio, time);
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
            return new Interaction(name, nextStory, isInterrupt ? Interaction.InteractionType.Interrupt : Interaction.InteractionType.Continue);
        }
        else
        {
            return null;
        }
    }

    public void RemoveInteraction()
    {
        
    }
}
