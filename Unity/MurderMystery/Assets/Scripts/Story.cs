using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Story : MonoBehaviour
{
    [Serializable]
    public class Snippet
    {
        public float startTime;
        protected bool isPlaying;

        public Snippet(float startTime)
        {
            this.startTime = startTime;
            isPlaying = false;
        }

        public virtual void Play()
        {
            Debug.Log("NOs");
        }
        public virtual void Stop()
        {
            Debug.Log("NO");
        }

    }
    
    [Serializable]
    public class AnimSnippet
    {
        public Animator anim;
        public string trigger;
        public float startTime;
        private bool isPlaying;


        public AnimSnippet (Animator anim, string trigger, float startTime)
        {
            this.anim = anim;
            this.trigger = trigger;
            this.startTime = startTime;
            isPlaying = false;
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
            anim.Play("Base", 0, 0);
            isPlaying = false;
        }

    }

    [Serializable]
    public class AudioSnippet
    {
        public AudioSource audio;
        public float startTime;
        private bool isPlaying;

        public AudioSnippet (AudioSource audio, float startTime)
        {
            this.audio = audio;
            this.startTime = startTime;
            isPlaying = false;
        }

        public void Play()
        {

            if (!isPlaying)
            {
                audio.enabled = true;
                audio.Play();
                isPlaying = true;
            }
        }
        public void Stop()	
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
    public List<AudioSnippet> audioSnippets;

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
    public void AddAudioSnippet(AudioSource audio, float relativeStartTime)
    {
        AudioSnippet newSnippet = new AudioSnippet (audio, relativeStartTime);
        audioSnippets.Add(newSnippet);
    }

    public void UpdateAnimSnippets(List<AnimSnippet> newSnippets)
    {
        animSnippets = newSnippets;
    }
    public void UpdateAudioSnippets(List<AudioSnippet> newSnippets)
    {
        audioSnippets = newSnippets;
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
        foreach (AudioSnippet snippet in audioSnippets)
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
        foreach (AudioSnippet snippet in audioSnippets)
        {
            snippet.Stop();
        }

    }
}
