using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//Story editor based on: https://gram.gs/gramlog/creating-node-based-editor-unity/

public class StoryEditor : EditorWindow
{
    public List<Node> allNodes;
    public List<StoryNode> storyNodes;
    public List<SequenceNode> sequenceNodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 drag;
    private Vector2 offset;
    
    [MenuItem("Window/Story Editor")]
    private static void OpenWindow()
    {
        StoryEditor window = GetWindow<StoryEditor>();
        window.titleContent = new GUIContent("Story Editor");
    }

    private void OnEnable()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

        LoadSavedStories();
    }

    private void LoadSavedStories()
    {
        foreach (Story story in StoryStructure.Instance.GetComponents<Story>())
        {
            OnClickAddStoryNode(story.nodePosition, story);
        }
    }

    private void LoadSavedInteractions()
    {
        if (storyNodes != null)
        {
            foreach (StoryNode node in storyNodes)
            {
                node.LoadInteractionConnections();
            }
        }
        if (sequenceNodes != null)
        {
            foreach (SequenceNode node in sequenceNodes)
            {
                node.LoadConnections();
            }
        }
    }

    private void OnGUI()
    {

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        connections = new List<Connection>();
        
        DrawNodes();

        LoadSavedInteractions();
        DrawConnections();

        DrawConnectionLine(Event.current);
        
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);
        if(GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawNodes()
    {
        if (storyNodes != null)
        {
            foreach (Node node in storyNodes)
            {
                node.Draw();
            }
        }
        if (sequenceNodes != null)
        {
            foreach (Node node in sequenceNodes)
            {
                node.Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                               selectedInPoint.GetCenter(),
                               e.mousePosition,
                               selectedInPoint.GetCenter() + Vector2.left * 50f,
                               e.mousePosition - Vector2.left * 50f,
                               Color.white,
                               null,
                               2f
                               );
 
            GUI.changed = true;
        }
 
        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                               selectedOutPoint.GetCenter(),
                               e.mousePosition,
                               selectedOutPoint.GetCenter() - Vector2.left * 50f,
                               e.mousePosition + Vector2.left * 50f,
                               Color.white,
                               null,
                               2f
                               );
 
            GUI.changed = true;
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }
        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
    
    private void ProcessNodeEvents(Event e)
    {
        if (allNodes != null)
        {
            for (int i = allNodes.Count -1; i >= 0; i--)
            {
                bool guiChanged = allNodes[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;
        
        switch(e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Story"), false, () => OnClickAddStoryNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Sequence"), false, () => OnClickAddSequenceNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddStoryNode(Vector2 mousePosition, Story story = null)
    {
        if (storyNodes == null)
        {
            storyNodes = new List<StoryNode>();
        }
        StoryNode newNode = new StoryNode(mousePosition, 200, 250, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, this, story);
        storyNodes.Add(newNode);

        if (allNodes == null)
        {
            allNodes = new List<Node>();
        }
        allNodes.Add(newNode);
    }

    private void OnClickAddSequenceNode(Vector2 mousePosition)
    {
        if (sequenceNodes == null)
        {
            sequenceNodes = new List<SequenceNode>();
        }
        if (StoryStructure.Instance.sequences == null)
        {
            StoryStructure.Instance.sequences = new List<ListWrapper<Story>>();
        }
        int index = StoryStructure.Instance.sequences.Count;
        
        SequenceNode newNode = new SequenceNode(mousePosition, 200, 250, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, this, index);
        sequenceNodes.Add(newNode);
        if (allNodes == null)
        {
            allNodes = new List<Node>();
        }
        allNodes.Add(newNode);
    }

    
    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        Debug.Log("onLcikc in point");
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            Debug.Log("outpoint not null");
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                Debug.Log("Creating connection");
                CreateConnection();
            }
            ClearConnectionSelection();
        }
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;
        if (storyNodes != null)
        {
            foreach(Node node in storyNodes)
            {
                node.Drag(delta);
            }
        }
        if (sequenceNodes != null)
        {
            foreach(Node node in sequenceNodes)
            {
                node.Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;
        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
            }
            ClearConnectionSelection();
        }
    }

    private void OnClickRemoveNode(Node node)
    {
        StoryNode storyNode = node as StoryNode;
        if (storyNode != null)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                {
                    if (connections[i].inPoint == storyNode.inPoint || storyNode.outPoints.Contains(connections[i].outPoint))
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                    foreach (Connection connection in connectionsToRemove)
                    {
                        connections.Remove(connection);
                    }
                }
            }
            storyNodes.Remove(storyNode);
            return;
        }
        SequenceNode seqNode = node as SequenceNode;
        if (seqNode != null)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();
                for (int i = 0; i < connections.Count; i++)
                {
                    if (seqNode.outPoints.Contains(connections[i].outPoint))
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                    foreach (Connection connection in connectionsToRemove)
                    {
                        connections.Remove(connection);
                    }
                }
            }
            sequenceNodes.Remove(seqNode);
            return;
        }
        
    }
    
    private void OnClickRemoveConnection(Connection connection)
    {
        StoryNode storyNode = connection.outPoint.node as StoryNode;
        storyNode.story.RemoveInteraction(connection.outPoint.interactionName);
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        Connection newConnection = new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);
        connections.Add(newConnection);
    }

    public void CreateConnection(ConnectionPoint outPoint, ConnectionPoint inPoint)
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        Connection newConnection = new Connection(inPoint, outPoint, OnClickRemoveConnection);
        connections.Add(newConnection);

    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}
