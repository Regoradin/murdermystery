using System;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint
{
    public Rect rect;
    
    private float yOffset;

    public ConnectionPointType type;

    public StoryNode node;
    public string interactionName;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(StoryNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint, float yOffset = 0.5f, string interactionName = null)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.yOffset = yOffset * node.rect.height;
        this.interactionName = interactionName;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        CalculateRect();
        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }

    private void CalculateRect()
    {
        //rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
        rect.y = node.rect.y + yOffset - rect.height * 0.5f;

        switch(type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;
            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }
    }

    public Vector2 GetCenter()
    {
        CalculateRect();
        return rect.center;
    }
}
