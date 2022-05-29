//using System;
//using System.IO;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.Networking;
//using Siccity.GLTFUtility;

//public class ModelLoader : MonoBehaviour
//{
//    GameObject wrapper;
//    string filePath;
//    string url = "https://ipfs.io/ipfs/QmYweVDUwe1bcAxz5CcrQcd7A2Tp8ubts9XrTznMnd6htq";

//    private void Start()
//    {
//        filePath = ".//Assets//NFT_cache//";
//        wrapper = new GameObject
//        {
//            name = "Model"
//        };
//        DownloadFile(url);
//    }
//    public void DownloadFile(string url)
//    {
//        string path = GetFilePath(url);
//        if (File.Exists(path))
//        {
//            Debug.Log("Found file locally, loading...");
//            LoadModel(path);
//            return;
//        }

//        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
//        {
//            if (req.isNetworkError || req.isHttpError)
//            {
//                // Log any errors that may happen
//                Debug.Log($"{req.error} : {req.downloadHandler.text}");
//            }
//            else
//            {
//                // Save the model into a new wrapper
//                LoadModel(path);
//            }
//        }));
//    }

//    string GetFilePath(string url)
//    {
//        string[] pieces = url.Split('/');
//        string filename = pieces[pieces.Length - 1];
//        string nickname = "model.glb";
//        return $"{filePath}{nickname}";
//    }

//    void LoadModel(string path)
//    {
//        ResetWrapper();
//        GameObject model = Importer.LoadFromFile(path);
//        model.transform.SetParent(wrapper.transform);
//    }

//    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
//    {
//        using (UnityWebRequest req = UnityWebRequest.Get(url))
//        {
//            req.certificateHandler = new BypassCertificate();
//            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));
//            yield return req.SendWebRequest();
//            callback(req);
//        }
//    }

//    void ResetWrapper()
//    {
//        if (wrapper != null)
//        {
//            foreach (Transform trans in wrapper.transform)
//            {
//                Destroy(trans.gameObject);
//            }
//        }
//    }
//}