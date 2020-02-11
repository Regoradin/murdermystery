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

    public void Interact(string interaction)
    {
        Debug.Log("INTERACTION: " + interaction);
        if (interaction == "Finish")
        {
            FinishCurrentStory();
        }
        else if (currentStory.interactions.ContainsKey(interaction))
        {
            Story nextStory = currentStory.interactions[interaction];
            if (currentStory.interactionTypes[interaction] == Story.InteractionType.Interrupt)
            {
                storyStack.Push(currentStory);
            }
            else 
            {
                //This implementation only applies if snippet-finishing is non functional
                //if this ever gets changed, CheckIfFinished() on the Update of Story needs to be  re-enabled
                currentStory.isFinished = true;                    
            }
            currentStory.Stop();
            currentStory = nextStory;
            currentStory.Play();

        }
    }

    public void DebugSetup(Animator anim)
    {
        Story sone = gameObject.AddComponent<Story>();
        Story stwo = gameObject.AddComponent<Story>();
        Story sthree = gameObject.AddComponent<Story>();
        
        sone.AddSnippet("123", anim, "123", 0);
        stwo.AddSnippet("1Down", anim, "1Down", 0);
        sthree.AddSnippet("1Back", anim, "1Back", 0);

        sone.interactions.Add("down", stwo);
        sone.interactions.Add("back", sthree);

        sone.interactionTypes.Add("down", Story.InteractionType.Continue);
        sone.interactionTypes.Add("back", Story.InteractionType.Interrupt);

        currentStory = sone;
        currentStory.Play();
    }

    public void FinishCurrentStory()
    {
        currentStory.isFinished = true;
        currentStory.Stop();
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
                Debug.Log("Popping from stack");
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
        Debug.Log("Picking random story");
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
        return null;
    }
}
