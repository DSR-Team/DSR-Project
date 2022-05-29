using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBaseMusic : BubbleBase
{
    public AudioSource source;
    private int audioStatus=0;

    public override void PlayPause()
    {
        if(source.clip==null) return;

        if(audioStatus==0) //Stop->Play
        {
            source.Play();
            audioStatus=1;
        }
        else if(audioStatus==1) //Play->Pause
        {
            source.Pause();
            audioStatus=2;
        }
        else //Pause->Play
        {
            source.UnPause();
            audioStatus=1;
        }
    }

    public override void Stop() //?->Stop
    {
        if(source.clip==null) return;

        source.Stop();
        audioStatus=0;
    }
}
