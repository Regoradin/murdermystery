using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour
{
    private class Snippet
    {
        private Animator anim;
        private string trigger;

        public float startTime;
        private bool isPlaying;

        public Snippet (Animator anim, string trigger, float startTime)
        {
            this.anim = anim;
            this.trigger = trigger;
            this.startTime = startTime;
            isPlaying = false;
            isFinished = false;
        }

        public void Play()
        {
            if (!isPlaying)
            {
                anim.enabled = true;
                anim.Play("Base", 0, 0);
                anim.SetTrigger(trigger);
                isPlaying = true;
            }
        }

        public void Stop()
        {
            anim.enabled = false;
            isPlaying = false;
        }
    }

    private Dictionary<string, Snippet> snippets;

    private float startTime;
    private float interruptTime;
    private bool isPlaying = false;
    public bool isFinished = false;

    public enum InteractionType
    {
        Interrupt,
        Continue
    }
    
    public Dictionary<string, Story> interactions;
    public Dictionary<string, InteractionType> interactionTypes;

    public void AddSnippet(string name, Animator anim, string trigger, float relativeStartTime)
    {
        Snippet newSnippet = new Snippet (anim, trigger, relativeStartTime);
        snippets.Add(name, newSnippet);
    }

    private void Awake()
    {
        snippets = new Dictionary<string, Snippet>();
        interruptTime = 0;

        interactions = new Dictionary<string, Story>();
        interactionTypes = new Dictionary<string, InteractionType>();
    }

    private void Update()
    {
        if (isPlaying)
        {
            StartNewSnippets();
        }
    }

    public void Play()
    {
        startTime = Time.time;
        isPlaying = true;
        isFinished = false;
    }

    public void Stop()
    {
        isPlaying = false;
        interruptTime = Time.time - startTime;
        StopAllSnippets();
    }

    private void StartNewSnippets()
    {
        foreach (Snippet snippet in snippets.Values)
        {
            if (Time.time - startTime  >= snippet.startTime)
            {
                snippet.Play();
            }
        }
    }

    private void StopAllSnippets()
    {
        foreach (Snippet snippet in snippets.Values)
        {
            snippet.Stop();
        }
    }
}
