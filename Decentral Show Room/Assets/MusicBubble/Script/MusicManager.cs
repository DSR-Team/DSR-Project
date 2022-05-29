using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public string PlayerName;
    public AudioClip BGM;
    [Range(0f, 1f)]
    public float volume = 1f;
    public MusicBubble [] bubbles;

    private AudioSource BGMsource;
    private MusicBubble bubbleNow=null;

    void Start()
    {
        BGMsource = GetComponent<AudioSource>();
        BGMsource.volume = volume;
        BGMsource.loop = true;
        BGMsource.clip = BGM;
        if(BGMsource.clip != null)
        {
            BGMsource.Play();
        }
        
        for(int i=0; i<bubbles.Length; i++)
        {
            bubbles[i].manager = GetComponent<MusicManager>();
            bubbles[i].index = i;
            bubbles[i].SetPlayerName(PlayerName);
        }
    }

    // API for calling
    public void PlayBubbleNow()
    {
        if(bubbleNow==null) return;
        bubbleNow.Play();
    }
    public void StopBubbleNow()
    {
        if(bubbleNow==null) return;
        bubbleNow.Stop();
    }
    // end API for calling

    public void TriggerIn(int _index)
    {
        if(BGMsource.clip != null) BGMsource.Pause();
        bubbleNow = bubbles[_index];
    }

    public void TriggerOut(int _index)
    {
        if(BGMsource.clip != null) BGMsource.UnPause();
        bubbleNow.Stop();
        bubbleNow = null;
    }

    
}
