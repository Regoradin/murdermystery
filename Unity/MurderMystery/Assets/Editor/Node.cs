using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using static Story;

public class Node
{
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    protected GUIStyle inPointStyle;
    protected Action<ConnectionPoint> OnClickInPoint;
    protected GUIStyle outPointStyle;
    protected Action<ConnectionPoint> OnClickOutPoint;

    protected StoryEditor editor;

    
    public Node(Vector2 position, float width, float height, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, Action<ConnectionPoint> OnClickInPoint, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, StoryEditor editor)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.defaultNodeStyle = style;
        this.selectedNodeStyle = selectedStyle;
        this.style = defaultNodeStyle;
        this.editor = editor;

        this.inPointStyle = inPointStyle;
        this.OnClickInPoint = OnClickInPoint;
        this.outPointStyle = outPointStyle;
        this.OnClickOutPoint = OnClickOutPoint;

        this.OnRemoveNode = OnClickRemoveNode;
    }

    public virtual void Drag(Vector2 delta)
    {
        rect.position += delta;

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    public virtual void Draw()
    {
        GUI.Box(rect, title, style);

        GUILayout.BeginArea(new Rect(rect.x + 10, rect.y + 10, rect.width - 20, rect.height - 20));
        DrawContents();
        GUILayout.EndArea();

        if (GUI.changed)
        {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());            
        }
    }

    public virtual bool ProcessEvents(Event e)
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

    protected virtual void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }


    protected virtual void DrawContents()
    {
    }
    
}
