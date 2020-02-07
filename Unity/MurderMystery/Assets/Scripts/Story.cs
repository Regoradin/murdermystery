using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour
{
    public class Snippet
    {
        private Animator anim;
        private string trigger;

        public float startTime;
        private bool isPlaying;
        public bool isFinished;

        private bool interrupted;

        public Snippet (Animator anim, string trigger, float startTime)
        {
            this.anim = anim;
            this.trigger = trigger;
            this.startTime = startTime;
            isPlaying = false;
            isFinished = false;
            interrupted = false;
        }

        public void Play()
        {
            if (!isPlaying && !isFinished)
            {
                Debug.Log("Playing Snippet");
                anim.enabled = true;
                anim.SetTrigger(trigger);
                isPlaying = true;
            }
        }
        public void Interrupt()
        {
            anim.enabled = false;
            interrupted = true;
            isPlaying = false;
        }
    }

    private Dictionary<string, Snippet> snippets;
    private int currentSnippetIndex;

    private float startTime;
    private float interruptTime;
    private bool playing = false;

    public void AddSnippet(string name, Animator anim, string trigger, float relativeStartTime)
    {
        Snippet newSnippet = new Snippet (anim, trigger, relativeStartTime);
        snippets.Add(name, newSnippet);
    }

    public void AddDebugSnippet(Animator anim)
    {
        AddSnippet("Test1", anim, "test", 0);
        AddSnippet("Test2", anim, "test", 2);
        AddSnippet("Test3", anim, "test", 5);
    }

    private void Start()
    {
        snippets = new Dictionary<string, Snippet>();
        interruptTime = 0;
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
        Debug.Log("Start time: " + startTime);
        playing = true;
    }

    public void Stop()
    {
        playing = false;
        interruptTime = Time.time - startTime;
        Debug.Log("Interrupt Time: " + interruptTime);
        InterruptAllSnippets();
    }

    private void StartNewSnippets()
    {
        foreach (Snippet snippet in snippets.Values)
        {
            if (Time.time - startTime + interruptTime >= snippet.startTime)
            {
                snippet.Play();
            }
        }
    }

    private void InterruptAllSnippets()
    {
        foreach (Snippet snippet in snippets.Values)
        {
            snippet.Interrupt();
        }
    }

    public void FinishSnippet(string name)
    {
        snippets[name].isFinished = true;
    }
}
