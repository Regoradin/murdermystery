using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioVisualizer : MonoBehaviour
{
    private AudioSource[] potentialAudios;
    private AudioSource audioPlaying;
    private bool mePlaying,collectedSpectrum;
    private float amplitude;
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
        string spectrumString = "";

        Debug.Log(spectrumString);
        mePlaying = false;
        foreach(AudioSource audio in potentialAudios)
        {
            if (audio.isPlaying)
            {
                mePlaying = true;
                audioPlaying = audio;
                Debug.Log("Is Playing!");
                
            }
        }

        if (mePlaying)
        {
            if (!collectedSpectrum)
            {
                audioPlaying.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
                collectedSpectrum = true;
            }
            else
            {
                for (int i = 0; i < spectrum.Length; i++)
                {
                    amplitude = Mathf.Max(amplitude, spectrum[i]*5+2);
                }
            }

            
            
        }
        else
        {
            collectedSpectrum = false;
            amplitude = 1;
        }


    }


}
