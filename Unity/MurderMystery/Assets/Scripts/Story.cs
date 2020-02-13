using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour
{
    public class Snippet
    {
        public Animator anim;
        public string trigger;

        public float startTime;
        public bool isFinished;

        public Snippet (Animator anim, string trigger, float startTime)
        {
            this.anim = anim;
            this.trigger = trigger;
            this.startTime = startTime;
            isFinished = false;
        }
    }

    private SortedList<float, Snippet> snippets;
    private int currentSnippetIndex;

    private float startTime;
    private bool playing = false;

    public void AddSnippet(Animator anim, string trigger, float relativeStartTime)
    {
        Snippet newSnippet = new Snippet (anim, trigger, relativeStartTime);
        snippets.Add(relativeStartTime, newSnippet);
    }

    public void AddDebugSnippet(Animator anim)
    {
        AddSnippet(anim, "test", 0);
    }

    private void Start()
    {
        snippets = new SortedList<float, Snippet>();
    }

    private void Update()
    {
        if (playing)
        {
            StartNewSnippets();            
        }
    }

    public void Play()
    {
        startTime = Time.time;
        playing = true;
    }

    public void Stop()
    {
        playing = false;
        StopAllSnippets();
    }

    private void StartNewSnippets()
    {
        while (snippets[currentSnippetIndex].startTime <= Time.time - startTime)
        {
            snippets[currentSnippetIndex].anim.enabled = true;
            snippets[currentSnippetIndex].anim.SetTrigger(snippets[currentSnippetIndex].trigger);
            currentSnippetIndex++;
            if (currentSnippetIndex == snippets.Count)
            {
                currentSnippetIndex = 0;
            }
        }        
    }

    private void StopAllSnippets()
    {
        foreach (Snippet snippet in snippets.Values)
        {
            //TODO: make this a less shoddy solution
            snippet.anim.enabled = false;
        }
    }
}
