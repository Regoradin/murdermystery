using System;
using UnityEngine;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint
{
    public Rect rect;
    private float yOffset;

    public ConnectionPointType type;

    public StoryNode node;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(StoryNode node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint, float yOffset = 0.5f)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.yOffset = yOffset * node.rect.height;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
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
        //Debug.Log(rect.y + " " + rect.x);


        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
