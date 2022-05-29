using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleBase : MonoBehaviour
{
    public BubbleBound bound;
    public GameObject info;
    public float infoDis=1f;
    public GameObject NFTColliderObj;
    public NFT_InfoRecorder infoRecorder;
    public GameObject [] interactiveObjs;
    [HideInInspector] public BubbleManager manager;
    [HideInInspector] public int index;
    

    private int infoStatus=0;

    void Awake()
    {
        BubbleManager _manager = GameObject.Find("BubbleManager").GetComponent<BubbleManager>();
        _manager.RegistBubble(this.gameObject.GetComponent<BubbleBase>());
    }

    public void SetPlayerName(string _name)
    {
        bound.playerName = _name;
    }

    // API for calling
    public void AddInteractiveObj(GameObject g)
    {
        int length = interactiveObjs.Length;
        GameObject [] newObjs = new GameObject [length+1];
        for(int i=0; i<length; i++)
        {
            newObjs[i] = interactiveObjs[i];
        }
        newObjs[length] = g;
        interactiveObjs = newObjs;
    }
    // end API for calling 

    public void TriggerIn()
    {
        for(int i=0; i<interactiveObjs.Length; i++)
        {
            interactiveObjs[i].SetActive(true);
        }
        if(NFTColliderObj!=null) NFTColliderObj.GetComponent<Collider>().enabled = true;
        manager.TriggerIn(index);
    }

    public void TriggerOut()
    {
        for(int i=0; i<interactiveObjs.Length; i++)
        {
            interactiveObjs[i].SetActive(false);
        }
        if(NFTColliderObj!=null) NFTColliderObj.GetComponent<Collider>().enabled = false;
        manager.TriggerOut(index);
        info.SetActive(false);
        infoStatus=0;
    }
    
    public virtual void PlayPause(){}
    public virtual void Stop(){}

    private void SetInfo()
    {
        TextMeshProUGUI TitleComp = info.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        TitleComp.text = infoRecorder.Title;

        TextMeshProUGUI AuthorComp = info.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        AuthorComp.text = infoRecorder.Creators[0].Substring(0, 5) + "..." + infoRecorder.Creators[0].Substring(31, 5);

        TextMeshProUGUI DisComp = info.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        DisComp.text = infoRecorder.Description;
    }
    public virtual void OpenCloseInfo()
    {
        SetInfo();
        if(infoStatus==0)
        {
            info.SetActive(true);
            info.transform.position = Camera.main.transform.position + Camera.main.transform.forward * infoDis;
            info.transform.rotation = new Quaternion( 0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w );
            infoStatus=1;
        }
        else
        {
            info.SetActive(false);
            infoStatus=0;
        }
        
    }
}
