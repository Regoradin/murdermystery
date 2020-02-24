using System;
using UnityEditor;
using UnityEngine;

public class Connection
{
    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;

    }

    public void Draw()
    {
        Handles.DrawBezier(
                           inPoint.GetCenter(),
                           outPoint.GetCenter(),
                           inPoint.GetCenter() + Vector2.left * 50f,
                           outPoint.GetCenter() - Vector2.left * 50f,
                           Color.white,
                           null,
                           2f
                           );

        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap))
        {
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        }
        StoryNode outStoryNode = outPoint.node as StoryNode;
        StoryNode inStoryNode = inPoint.node as StoryNode;
        SequenceNode outSequenceNode = outPoint.node as SequenceNode;
        if (outStoryNode != null && inStoryNode != null)
        {
            outStoryNode.story.ConnectInteraction(outPoint.interactionName, inStoryNode.story);
        }
        else if (outSequenceNode != null && inStoryNode != null)
        {
            outSequenceNode.AddStory(outPoint, inStoryNode.story);
        }
        else
        {
            Debug.Log("Not valid connection point");
        }
        
    }

}
