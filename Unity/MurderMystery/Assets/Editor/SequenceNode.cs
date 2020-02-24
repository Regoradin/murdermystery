using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;
using static Story;

public class SequenceNode : Node
{
    public List<ConnectionPoint> outPoints;

    private int sequenceIndex;

    public SequenceNode(Vector2 position, float width, float height, GUIStyle style, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode, StoryEditor editor, int sequenceIndex) : base(position, width, height, style, selectedStyle, inPointStyle, OnClickInPoint, outPointStyle, OnClickOutPoint, OnClickRemoveNode, editor)
    {
        outPoints = new List<ConnectionPoint>();
        this.sequenceIndex = sequenceIndex;
        if (sequenceIndex >= 0)
        {
            if (sequenceIndex >= StoryStructure.Instance.sequences.Count)
            {
                StoryStructure.Instance.sequences.Add(new ListWrapper<Story>());
            }
        }
        else
        {
            Debug.LogError("Sequence made with a bad index");
        }

    }

    public void AddStory(ConnectionPoint outPoint, Story story)
    {
        if (outPoints.Contains(outPoint))
        {
            int index = outPoints.IndexOf(outPoint);
            while (StoryStructure.Instance.sequences[sequenceIndex].Count <= index)
            {
                StoryStructure.Instance.sequences[sequenceIndex].InnerList.Add(null);
            }
            StoryStructure.Instance.sequences[sequenceIndex][index] = story;
        }
    }

    public void LoadConnections()
    {
        for (int i = 0; i < StoryStructure.Instance.sequences[sequenceIndex].Count; i++)
        {
            AddStory(outPoints[i], StoryStructure.Instance.sequences[sequenceIndex][i]);
        }
    }

    public override void Draw()
    {
        base.Draw();
        foreach (ConnectionPoint point in outPoints)
        {
            point.Draw();
        }
        
    }
    
    protected override void DrawContents()
    {
        DrawSequenceFields();
    }

    private void DrawSequenceFields()
    {
        outPoints = new List<ConnectionPoint>();
        GUILayout.Label("Index " + sequenceIndex);
        for (int i = 0; i < StoryStructure.Instance.sequences[sequenceIndex].Count; i++)
        {
            ConnectionPoint point = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0.1f * (i + 1));
            outPoints.Add(point);
        }
        ConnectionPoint blankPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0.1f * (outPoints.Count + 1));
        outPoints.Add(blankPoint);
        
    }
    
}
