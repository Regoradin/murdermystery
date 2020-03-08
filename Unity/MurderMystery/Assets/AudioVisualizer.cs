using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioVisualizer : MonoBehaviour
{
    private AudioSource[] potentialAudios;
    private AudioSource audioPlaying;
    private bool mePlaying,collectedSpectrum;
    private float amplitude,TimeStamp;
    private float[] spectrum;
    [SerializeField]
    private Material mat;


    private void Start()
    {
        potentialAudios = gameObject.GetComponentsInChildren<AudioSource>();
        spectrum = new float[256];
    }

    private void Update()
    {
        mat.SetFloat("Vector1_569C1A10", amplitude);
        mePlaying = false;
        foreach(AudioSource audio in potentialAudios)
        {
            if (audio.isPlaying)
            {
                mePlaying = true;
                audioPlaying = audio;
                //Debug.Log("Is Playing!");
                
            }
        }

        if (mePlaying)
        {
            TimeStamp += Time.deltaTime;
            if (!collectedSpectrum)
            {
                audioPlaying.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
                collectedSpectrum = true;
                amplitude = spectrum[0];
            }
            else
            {
                int index = Mathf.RoundToInt(TimeStamp);
                amplitude = spectrum[index]*1000000000;
            }
            
            
        }
        else
        {
            collectedSpectrum = false;
            amplitude = 1;
            TimeStamp = 0;
        }


    }


}
