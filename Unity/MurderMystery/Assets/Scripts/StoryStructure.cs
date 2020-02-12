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
    
    private void FinishCurrentStory()
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

    public void DebugSetup(Animator anim)
    {
        Story sone = gameObject.AddComponent<Story>();
        Story stwo = gameObject.AddComponent<Story>();
        Story sthree = gameObject.AddComponent<Story>();
        
        sone.AddSnippet("123", anim, "123", 1);
        stwo.AddSnippet("1Down", anim, "1Down", 0);
        sthree.AddSnippet("1Back", anim, "1Back", 0);

        sone.interactions.Add("down", stwo);
        sone.interactions.Add("back", sthree);

        sone.interactionTypes.Add("down", Story.InteractionType.Continue);
        sone.interactionTypes.Add("back", Story.InteractionType.Interrupt);

        List<Story> l1 = new List<Story>();
        List<Story> l2 = new List<Story>();

        l1.Add(sone);
        l2.Add(stwo);
        l2.Add(sthree);

        sequences.Add(l1);
        sequences.Add(l2);
        
        currentStory = sone;
        currentStory.Play();
    }

}
