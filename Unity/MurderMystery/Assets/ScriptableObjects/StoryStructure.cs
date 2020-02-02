using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StoryStructure", menuName = "Story/StoryStructure", order = 1)]
public class StoryStructure : ScriptableObject
{
    public string name;

    public List<Story> stories;
    private int currentStoryIndex = 0;

    public Story GetNextStory()
    {
        currentStoryIndex++;
        if (currentStoryIndex >= stories.Count)
        {
            currentStoryIndex = 0;
        }
        return stories[currentStoryIndex];
    }
}
