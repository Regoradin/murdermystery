using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryStructure : Singleton<StoryStructure>
{
    private Story currentStory;
    private Stack<Story> storyStack;

    private List<List<Story>> sequences;

    private void Awake()
    {
        storyStack = new Stack<Story>();
        sequences = new List<List<Story>>();
    }

    private void Update()
    {
        while (currentStory == null || currentStory.isFinished)
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
    
    public void Interact(string interaction)
    {
        if (interaction == "Finish")
        {
            FinishCurrentStory();
        }
        else if (currentStory.interactions.ContainsKey(interaction))
        {
            Story nextStory = currentStory.interactions[interaction];
            if (currentStory.interactionTypes[interaction] == Story.InteractionType.Interrupt)
            {
                //Interuptions shouldn't mark the current story as finished,
                //as you want to be able to return to it
                storyStack.Push(currentStory);
                currentStory.Stop();
            }
            else
            {
                FinishCurrentStory();
            }
            StartStory(nextStory);
        }
    }

    private void StartStory(Story story)
    {
        currentStory = story;
        //handle grabbing interaction dicts
        currentStory.Play();
    }
    
    public void FinishCurrentStory()
    {
        currentStory.isFinished = true;
        currentStory.Stop();
    }


    private Story pickRandomStory()
    {
        Debug.Log("Picking random story");
        while(sequences.Count > 0)
        {
            List<Story> sequence = sequences[Random.Range(0, sequences.Count)];
            Story nextStory = GetNextStoryInSequence(sequence);
            if (nextStory != null)
            {
                return nextStory;
            }
            sequences.Remove(sequence);
        }
        Debug.LogError("Ran out of stories to choose from!");
        return null;
    }

    private Story GetNextStoryInSequence(List<Story> sequence)
    {
        foreach (Story story in sequence)
        {
            if (!story.isFinished)
            {
                return story;
            }
        }
        return null;
    }
}
