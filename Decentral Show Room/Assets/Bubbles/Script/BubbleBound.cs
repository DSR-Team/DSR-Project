using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBound : MonoBehaviour
{
    public BubbleBase bubble;
    [HideInInspector]
    public string playerName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == playerName)
        {
            bubble.TriggerIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == playerName)
        {
            bubble.TriggerOut();
        }
    }
}
