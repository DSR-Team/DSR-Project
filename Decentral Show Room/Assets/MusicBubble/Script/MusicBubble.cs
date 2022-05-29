using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;

public class MusicBubble : MonoBehaviour
{
    public MusicBubbleBoundary boundary;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [HideInInspector]
    public MusicManager manager;
    [HideInInspector]
    public int index;

    private AudioSource source;

    // API for calling
    public void SetClip(AudioClip _clip)
    {
        clip=_clip;
        source.clip = clip;
    }
    // end API for calling

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        if(source.clip==null)
            Debug.LogWarning("No Clip is found in: " + gameObject.name);
        source.volume = volume;
    }

    public void SetPlayerName(string _name)
    {
        boundary.playerName = _name;
    }

    public void TriggerIn()
    {
        manager.TriggerIn(index);
    }

    public void TriggerOut()
    {
        manager.TriggerOut(index);
    }
    public void Play()
    {
        if(source.clip!=null)
            source.Play();
    }
    public void Stop()
    {
        if(source.clip!=null)
            source.Stop();
    }
    
}
