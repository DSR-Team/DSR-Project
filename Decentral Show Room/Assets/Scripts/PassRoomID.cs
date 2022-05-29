using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PassRoomID : MonoBehaviour
{
    public Text textObj;
    [SerializeField] private string roomID;
    [SerializeField] private string scenename = "RoomScene";

   
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetRoomID()
    {
        roomID = textObj.text;
        SceneManager.LoadScene(scenename);
    }
    public string GetRoomID()
    {
        return roomID;
    }
}
