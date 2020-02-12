using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//Story editor heavily based on: https://gram.gs/gramlog/creating-node-based-editor-unity/

public class StoryEditor : EditorWindow
{
    private List<StoryNode> nodes;
    private List<Connection> connections;

    private GUIStyle nodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;
    
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

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
        
    }

    private void OnGUI()
    {
        DrawNodes();
        DrawConnections();
        
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);
        if(GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            foreach (StoryNode node in nodes)
            {
                node.Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            Debug.Log("Drawing connections: " + connections.Count);

            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count -1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);
                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        switch(e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
                
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Story"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<StoryNode>();
        }
        nodes.Add(new StoryNode(mousePosition, 200, 50, nodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint));
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
            }
            ClearConnectionSelection();
        }
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

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (connections == null)
        {
            connections = new List<Connection>();
        }

        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}
