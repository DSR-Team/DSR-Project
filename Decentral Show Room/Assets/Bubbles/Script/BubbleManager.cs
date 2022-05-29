using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public string PlayerName;
    public AudioClip BGM;
    [Range(0f, 1f)]
    public float volume = 1f;
    private BubbleBase [] bubbles = new BubbleBase[6];
    private int bubbleIndex = 0;

    private AudioSource BGMsource;
    private BubbleBase bubbleNow=null;
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
    }
    
    public void InitBubbleManager()
    {
        for(int i=0; i<bubbles.Length; i++)
        {
            bubbles[i].manager = GetComponent<BubbleManager>();
            bubbles[i].index = i;
            bubbles[i].SetPlayerName(PlayerName);
        }
    }

    public void RegistBubble(BubbleBase b)
    {
        bubbles[bubbleIndex] = b;
        bubbleIndex += 1;
        if(bubbleIndex==6)
        {
            InitBubbleManager();
        }
    }

    // API for calling
    public void PlayPauseBubbleNow()
    {
        if(bubbleNow==null) return;
        bubbleNow.PlayPause();
    }
    public void OpenCloseInfoBubbleNow()
    {
        if(bubbleNow==null) return;
        bubbleNow.OpenCloseInfo();
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
