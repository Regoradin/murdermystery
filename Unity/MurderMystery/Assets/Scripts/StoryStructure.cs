using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryStructure : MonoBehaviour
{
    private Story currentStory;
    private Stack<Story> storyStack;

    private void Start()
    {
        storyStack = new Stack<Story>();
    }

    public void Interact(string interaction)
    {
        if (currentStory.interactions.ContainsKey(interaction))
        {
            Story nextStory = currentStory.interactions[interaction];
            if (currentStory.interactionTypes[interaction] == Story.InteractionType.Interrupt)
            {
                storyStack.Push(currentStory);
                currentStory.Stop();
                nextStory.Play();
            }
            if (currentStory.interactionTypes[interaction] == Story.InteractionType.Continue)
            {
                currentStory.Stop();
                nextStory.Play();
            }
        }
    }
}
