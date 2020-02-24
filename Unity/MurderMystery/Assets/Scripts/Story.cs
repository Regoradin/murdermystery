using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour
{
    [Serializable]
    public abstract class Snippet
    {
        public float startTime;
        protected bool isPlaying;

        public Snippet(float startTime)
        {
            this.startTime = startTime;
            isPlaying = false;
        }

        public abstract void Play();
        public abstract void Stop();
    }
    
    [Serializable]
    public class AnimSnippet : Snippet
    {
        public Animator anim;
        public string trigger;

        public AnimSnippet (Animator anim, string trigger, float startTime) : base(startTime)
        {
            this.anim = anim;
            this.trigger = trigger;
        }

        public override void Play()
        {
            if (!isPlaying)
            {
                anim.enabled = true;
                anim.Play("Base", 0, 0);
                anim.SetTrigger(trigger);
                isPlaying = true;
            }
        }

        public override void Stop()
        {
            anim.Play("Base", 0, 0);
            isPlaying = false;
        }

    }

    [Serializable]
    public class AudioSnippet : Snippet
    {
        public AudioSource audio;

        public AudioSnippet (AudioSource audio, float startTime) : base(startTime)
        {
            this.audio = audio;
        }

        public override void Play()
        {
            if (!isPlaying)
            {
                audio.enabled = true;
                audio.Play();
                isPlaying = true;
            }
        }
        public override void Stop()
        {
            audio.Stop();
            isPlaying = false;
        }
    }

    [Serializable]
    public class Interaction
    {
        public enum InteractionType
        {
            Interrupt,
            Continue
        }

        public string name;
        public Story nextStory;
        public InteractionType type;

        public Interaction(string name, Story nextStory, InteractionType type)
        {
            this.name = name;
            this.nextStory = nextStory;
            this.type = type;
        }
    }

    public List<AnimSnippet> animSnippets;

    //Data to work properly with story editor window
    public string name;
    public Vector2 nodePosition;
    
    private float startTime;
    private float interruptTime;
    public bool isPlaying = false;
    public bool isFinished = false;

    public List<Interaction> interactions;

    public void AddAnimSnippet(Animator anim, string trigger, float relativeStartTime)
    {
        AnimSnippet newSnippet = new AnimSnippet (anim, trigger, relativeStartTime);
        animSnippets.Add(newSnippet);
    }

    public void UpdateAnimSnippets(List<AnimSnippet> newSnippets)
    {
        animSnippets = newSnippets;
    }

    public void UpdateInteractions(List<Interaction> newInteractions)
    {
        interactions = newInteractions;
    }

    public void RemoveInteraction(string name)
    {
        List<Interaction> toRemove = new List<Interaction>();
        foreach (Interaction interaction in interactions)
        {
            if (interaction.name == name)
            {
                toRemove.Add(interaction);
            }
        }
        foreach(Interaction interaction in toRemove)
        {
            interactions.Remove(interaction);
        }
    }

    public void ConnectInteraction(string name, Story nextStory)
    {
        foreach (Interaction interaction in interactions)
        {
            if (interaction.name == name)
            {
                interaction.nextStory = nextStory;
                return;
            }
        }
    }

    private void Awake()
    {
        interruptTime = 0;
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
        foreach (AnimSnippet snippet in animSnippets)
        {
            if (Time.time - startTime  >= snippet.startTime)
            {
                snippet.Play();
            }
        }
    }

    private void StopAllSnippets()
    {
        foreach (AnimSnippet snippet in animSnippets)
        {
            snippet.Stop();
        }
    }
}
