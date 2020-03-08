using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Story;

public class StoryStructure : Singleton<StoryStructure>
{
    private Story currentStory;
    public Story startingStory;

    private Stack<Story> storyStack;
    private List<List<Story>> sequences;

    private Dictionary<string, Interaction> interactions;

    private void Awake()
    {
        storyStack = new Stack<Story>();
        sequences = new List<List<Story>>();

        interactions = new Dictionary<string, Interaction>();

    }

    private void Start()
    {
        StartStory(startingStory);
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
    
    public void Interact(string interactionName)
    {
        if (interactionName == "Finish")
        {
            FinishCurrentStory();
        }
        else if (interactions.ContainsKey(interactionName))
        {
            Interaction interaction = interactions[interactionName];
            Story nextStory = interaction.nextStory;
            if (interaction.type == Interaction.InteractionType.Interrupt)
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

    private void AddInteraction(Interaction interaction)
    {
        if (!interactions.ContainsKey(interaction.name))
        {
            interactions.Add(interaction.name, interaction);
        }
        else
        {
            interactions[interaction.name] = interaction;
        }
    }

    private void StartStory(Story story)
    {
        currentStory = story;
        foreach(Interaction interaction in currentStory.interactions)
        {
            AddInteraction(interaction);
        }
        currentStory.Play();
    }
    
    public void FinishCurrentStory()
    {
        currentStory.isFinished = true;
        currentStory.Stop();
    }


    private Story pickRandomStory()
    {
        //Debug.Log("Picking random story");
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
