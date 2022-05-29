using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBaseGIF : BubbleBase
{
    public GifPlayerManager player;
    private int playState = 0;
    public override void PlayPause()
    {
        if(playState==0)
        {
            player.SetPlay(true);
            playState=1;
        }
        else
        {
            player.SetPlay(false);
            playState=0;
        }
    }

    public override void Stop()
    {
        player.SetPlay(false);
        playState=0;
    }
}
