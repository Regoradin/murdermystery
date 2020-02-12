using UnityEngine;
using UnityEditor;
using System;

public class StoryNode
{
    public Rect rect;
    public string title;
    public bool isDragged;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    
    public GUIStyle style;

    public StoryNode(Vector2 position, float width, float height, GUIStyle style, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint)
    {
        rect = new Rect(position.x, position.y, width, height);
        this.style = style;

        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectionPoint(this, ConnectionPointType.Out, inPointStyle, OnClickInPoint);
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);
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
                    }
                    GUI.changed = true;
                }
                break;
                
            case EventType.MouseUp:
                isDragged = false;
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
}
