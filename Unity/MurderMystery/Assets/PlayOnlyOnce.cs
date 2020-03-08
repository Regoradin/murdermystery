using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayOnlyOnce : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Animator anim;
    public GazeDetectorMontage gaze;
    bool playing;
    bool stopMontage;


    // Start is called before the first frame update
    void Start()
    {
        playing = false;
        videoPlayer = GetComponent<VideoPlayer>();
        anim = GetComponent<Animator>();
        gaze = GetComponent<GazeDetectorMontage>();
        stopMontage = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gaze.gazed && !playing && !stopMontage)
        {
            playVideo();
        }
        if (playing && !stopMontage)
        {
            videoPlayer.loopPointReached += CheckOver; 
        }


    }
    //Play the video once gazed upon
    void playVideo()
    {
        anim.SetTrigger("ScreenOpen");
        videoPlayer.Play();
        playing = true;
    }

    //Check to see if the video is done playing
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //print("Video Is Over");
        playing = false;
        anim.SetTrigger("ScreenClose");
        stopMontage = true;
    }

    
}
