using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using Siccity.GLTFUtility;


public class RequestMultiTypeNFT : MonoBehaviour
{
    string path = ".//Assets//NFT_cache//";
    public GameObject myImageDisplayer;
    public GameObject myAudioDisplayer;
    public GameObject myVideoPlayer;
    public GameObject myGifPlayer;
    public GameObject myModelDisplayer;
    public GameObject VR_camera;

    public string mimeType = "image";
    bool IsLoading = false;

    ///public GameObject ArtWork;
    public List<Transform> spawnPoints;
    private List<GameObject> Showing_ArtWorks = new List<GameObject>();

    API_room.Root rp_room;
    API_NFT.Root response;
    NFT_InfoRecorder NFT_Info;

    string api_url = "https://stormy-temple-44410.herokuapp.com/";
    public string roomId = "0xK5gH";
    //"abcdef" 
    //"0xK5gH" 


    private void Start()
    {
        PassRoomID idObj = GameObject.Find("RoomID").GetComponent<PassRoomID>();
        roomId = idObj.GetRoomID();

        if (!IsLoading)
        {
            IsLoading = true;
            //Debug.Log("Start Loading NFT...");
            StartCoroutine(GetRoomRequest(api_url+ "rooms/" + roomId));
            //StartCoroutine(GetNFTRequest(api_url));
        }
        else if(IsLoading)
        {
            Debug.Log("Still loading other NFT, please command later");
        }
    }

    IEnumerator GetRoomRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            Debug.Log("Room Url : " + url);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            rp_room = JsonConvert.DeserializeObject<API_room.Root>(webRequest.downloadHandler.text);
            
            string tmp = "";
            for (int i = 0; i < rp_room.metadata.Count; i++)
            {
                if(rp_room.metadata[i].Count != 0)
                {
                    //Debug.Log("NFT " + i + " is loading...");
                    tmp = "token_metadata/" + rp_room.metadata[i]["contract"] + "/" + rp_room.metadata[i]["tokenId"];
                    StartCoroutine(GetNFTRequest(api_url + tmp, i));
                }
                else
                {
                    Debug.Log("No NFT " + i + " in this place!");
                }
            }     
        }
    }
    IEnumerator GetNFTRequest(string url, int number)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("NFT " + number + " Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("NFT " + number + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("NFT " + number + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            response = JsonConvert.DeserializeObject<API_NFT.Root>(webRequest.downloadHandler.text);
            
            GameObject spawnedNFT;

            if (response.mimeType.Contains("gif"))
            {
                spawnedNFT = Instantiate(myGifPlayer, spawnPoints[number].position, spawnPoints[number].rotation);
            }
            else if (response.mimeType.Contains("video"))
            {
                spawnedNFT = Instantiate(myVideoPlayer, spawnPoints[number].position, spawnPoints[number].rotation);
            }
            else if (response.mimeType.Contains("model"))
            {
                spawnedNFT = Instantiate(myModelDisplayer, spawnPoints[number].position, spawnPoints[number].rotation);
            }
            else if (response.mimeType.Contains("audio"))
            {
                spawnedNFT = Instantiate(myAudioDisplayer, spawnPoints[number].position, spawnPoints[number].rotation);
            }
            else if (response.mimeType.Contains("image"))
            {
                spawnedNFT = Instantiate(myImageDisplayer, spawnPoints[number].position, spawnPoints[number].rotation);
            }
            else
            {
                spawnedNFT = Instantiate(myModelDisplayer, spawnPoints[number].position, spawnPoints[number].rotation);
            }

            if (!Showing_ArtWorks.Contains(spawnedNFT))
                Showing_ArtWorks.Add(spawnedNFT);

            GameObject NFT =spawnedNFT.transform.GetChild(0).gameObject;
            NFT_Info = NFT.GetComponent<NFT_InfoRecorder>();
            NFT_Info.Title = response.name;
            NFT_Info.artwork_filename = roomId + "_" + number;


            for (int j = 0; j < response.creators.Count; j++)
            {
                if (j == 0)
                    NFT_Info.Creators.Add(response.creators[j]);
            }
            
            NFT_Info.RecentlySoldPrice = response.latestSoldPrice;
            NFT_Info.Description = response.description;
            
            if(response.tags != null)
            {
                for (int k = 0; k < response.tags.Count; k++)
                {
                    NFT_Info.tags.Add(response.tags[k]);
                }
            }

            string[] segments = response.artifactUri.Split('/');
            string artifact = "https://ipfs.io/ipfs/" + segments[segments.Length - 1];
            string[] segments2 = response.thumbnailUri.Split('/');
            string thumbnail = "https://ipfs.io/ipfs/" + segments2[segments2.Length - 1];

            LoadNFT(NFT_Info.artwork_filename, response.mimeType, thumbnail, artifact, NFT);
            //ex : https://ipfs.io/ipfs/QmNfUydPmBL3HKmPgU3h1GXN9SruDsFoe34m3qWMnEFhbF
        }
    }

    public void LoadNFT(string name, string mimeType, string thumbnaulUrl, string artifactUrl, GameObject NFT)
    {
        if (mimeType.Contains("gif"))
        {
            if (File.Exists(path + name + ".gif"))
            {
                Debug.Log("exist : " + name);
                StartCoroutine(GetImage(thumbnaulUrl, NFT, "gif"));
                ImportGIF(NFT);
            }
            else
            {
                StartCoroutine(GetGif(artifactUrl, name, NFT));
                StartCoroutine(GetImage(thumbnaulUrl, NFT, "gif"));
            }
        }
        else if (mimeType.Contains("video"))
        {
            if (File.Exists(path + name + ".mp4"))
            {
                Debug.Log("exist : " + name);
                StartCoroutine(GetImage(thumbnaulUrl, NFT, "video"));
                ImportVideo(NFT);
            }
            else
            {
                StartCoroutine(GetVideo(artifactUrl, name, NFT));
                StartCoroutine(GetImage(thumbnaulUrl, NFT, "video"));
            }
        }
        else if (mimeType.Contains("model"))
        {
            if (File.Exists(path + name + ".glb"))
            {
                Debug.Log("exist : " + name);
                ImportGLB(NFT, path + name + ".glb");
            }
            else
            {
                StartCoroutine(GetModel_3D(artifactUrl, name, NFT));
            }
        }
        else if (mimeType.Contains("audio"))
        {
            StartCoroutine(GetAudio(artifactUrl, NFT));
            //StartCoroutine(GetImage(thumbnaulUrl, NFT));
        }
        else if (mimeType.Contains("image"))
        {
            StartCoroutine(GetImage(thumbnaulUrl, NFT, "image"));
        }
        else
        {
            Debug.Log("Sorry, we don't support this NFT type");
        }
    }

    void ImportImage(GameObject NFT, Texture loadedTexture, String type)
    {
        Renderer ImageRenderer = NFT.GetComponent<Renderer>();
        if (type == "gif")
        {
            NFT.GetComponent<GifPlayerManager>().Prepare(loadedTexture);
        }
        else if (type == "video")
        {
            NFT.GetComponent<VR_VideoPlayerManager>().Prepare(loadedTexture);
        }
        ImageRenderer.material.mainTexture = loadedTexture;
    }
    void ImportAudio(GameObject NFT, AudioClip clip)
    {
        AudioSource source = NFT.GetComponent<AudioSource>();
        source.clip = clip;
        // source.Play();
    }
    
    void ImportGIF(GameObject NFT){
        NFT.GetComponent<GifPlayerManager>().SettleGIF();
    }
    void ImportVideo(GameObject NFT){
        NFT.GetComponent<VR_VideoPlayerManager>().SettleVideo();
    }
    void ImportGLB(GameObject NFT, string filepath)
    {
        ResetWrapper();
        GameObject GLB = Importer.LoadFromFile(filepath);
        Debug.LogWarning("filepath : " + filepath);
        GLB.transform.SetParent(NFT.transform);
        GLB.transform.position = NFT.transform.position;
        GLB.transform.rotation = NFT.transform.rotation;
        GLB.AddComponent<Rescale>();
        GameObject GrabbableObj = NFT.transform.parent.transform.GetChild(1).GetChild(2).gameObject;
        GameObject grabbable_GLB = Instantiate(GLB,  GrabbableObj.transform);
        grabbable_GLB.transform.position = GLB.transform.position;
        grabbable_GLB.transform.rotation = GLB.transform.rotation;
        grabbable_GLB.transform.localScale = GLB.transform.localScale;

        NFT.SetActive(true);
        Debug.Log("Success to import 3d model");
        CleanOtherCamera();
    }
 
 
    void ResetWrapper()
    {
        if (myModelDisplayer != null)
        {
            foreach (Transform trans in myModelDisplayer.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }
    IEnumerator GetImage(String url, GameObject NFT, String type)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        www.certificateHandler = new BypassCertificate();

        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error of Image: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error of Image: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("succeed to load Image ");
                ImportImage(NFT, DownloadHandlerTexture.GetContent(www), type);
                IsLoading = false;
                break;
        }
    }
    IEnumerator GetAudio(String url, GameObject NFT)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);

        www.certificateHandler = new BypassCertificate();
        

        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error of Audio: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error of Audio: " + www.error + " with : " + url);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Audio successfully downloaded");
                ImportAudio(NFT, DownloadHandlerAudioClip.GetContent(www));
                IsLoading = false;
                break;
        }
    }
    IEnumerator GetGif(String url, String name, GameObject NFT)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        www.certificateHandler = new BypassCertificate();


        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error of Gif: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error of Gif: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("gif successfully downloaded and saved to " + path);
                File.WriteAllBytes(path + name + ".gif", www.downloadHandler.data);
                ImportGIF(NFT);
                IsLoading = false;
                break;
        }
    }
    IEnumerator GetVideo(String url, String name, GameObject NFT)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        www.certificateHandler = new BypassCertificate();

        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error of Video: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error of Video: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Video successfully downloaded and saved to " + path);
                //File.WriteAllBytes(Application.persistentDataPath + "Video" + counter + ".mp4", wwwVideo.downloadHandler.data);
                //Writing videos... But its writing only last array counter name... And all videos are writing overwriting with same name(with last counter value)
                File.WriteAllBytes(path + name + ".mp4", www.downloadHandler.data);
                ImportVideo(NFT);
                IsLoading = false;
                break;
        }
    }
    IEnumerator GetModel_3D(String url, String name, GameObject NFT)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        www.certificateHandler = new BypassCertificate();
        //www.downloadHandler = new DownloadHandlerFile(path + "//myModel.glb");

        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError("Error of model: " + www.error);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError("HTTP Error of model: " + www.error + " with : " + url);
                IsLoading = false;
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("3D model successfully downloaded and saved to " + path + name);
                File.WriteAllBytes(path + name +".glb", www.downloadHandler.data);
                ImportGLB(NFT, path + name + ".glb");
                IsLoading = false;
                break;
        }
    }
    void CleanOtherCamera(){
        foreach(Camera tmp in Camera.allCameras){
            tmp.gameObject.SetActive(false);
        }
        VR_camera.gameObject.SetActive(true);
    }
}


public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        //Simply return true no matter what
        return true;
    }
}