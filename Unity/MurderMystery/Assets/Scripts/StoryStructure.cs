using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryStructure : MonoBehaviour
{
    private Story currentStory;
    private Stack<Story> storyStack;

    private List<List<Story>> sequences;

    private void Start()
    {
        storyStack = new Stack<Story>();
        sequences = new List<List<Story>>();
    }

    public void Interact(string interaction)
    {
        if (currentStory.interactions.ContainsKey(interaction))
        {
            Story nextStory = currentStory.interactions[interaction];
            if (currentStory.interactionTypes[interaction] == Story.InteractionType.Interrupt)
            {
                storyStack.Push(currentStory);
            }
            currentStory.Stop();
            currentStory = nextStory;
            currentStory.Play();

        }
    }

    public void FinishSnippet(string name)
    {
        currentStory.FinishSnippet(name);
    }

    private void Update()
    {
        while (currentStory.isFinished)
        {
            if (storyStack.Count > 0)
            {
                currentStory = storyStack.Pop();
            }
            else
            {
                currentStory = pickRandomStory();
            }
            currentStory.Play();
        }
    }

    private Story pickRandomStory()
    {
        while(sequences.Count > 0)
        {
            List<Story> sequence = sequences[Random.Range(0, sequences.Count)];
            foreach (Story story in sequence)
            {
                if (!story.isFinished)
                {
                    return story;
                }
            }
            //only reach here if a sequence is finished, in which case we can get rid of it.
            sequences.Remove(sequence);
        }
        Debug.LogError("Ran out of stories to choose from!");
    }
}
