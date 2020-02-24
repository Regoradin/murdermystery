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
    }

    protected override void DrawContents()
    {
        DrawSequenceFields();
    }

    private void DrawSequenceFields()
    {
        GUILayout.Label("Index " + sequenceIndex);
    }
    
}
