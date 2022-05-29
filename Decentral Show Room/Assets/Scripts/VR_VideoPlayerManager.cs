using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Play a single video or play from a list of videos 
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class VR_VideoPlayerManager : MonoBehaviour
{
    [Tooltip("Whether video should play on load")]
    public bool playAtStart = false;

    [Tooltip("Material used for playing the video (Uses URP/Unlit by default)")]
    public Material videoMaterial = null;

    [Tooltip("List of video clips to pull from")]
    //public VideoClip videoClip;

    private VideoPlayer videoPlayer = null;
    private MeshRenderer meshRenderer = null;
    
    private readonly string shaderUsed = "Universal Render Pipeline/Unlit";

    private Material offMaterial = null;
    private int index = 0;
    Texture thumbnail;
    Renderer videoRenderer;
    bool IsPrepared = false;
    bool IsSettled = false;

    string loadingPath = ".//Assets.//NFT_cache//";
    public string FileName = "myVideo.mp4";

    private void OnEnable()
    {
        // meshRenderer = GetComponent<MeshRenderer>();
        // videoPlayer = GetComponent<VideoPlayer>();

        // thumbnail = GetComponent<Renderer>().material;
        // FileName = GetComponent<NFT_InfoRecorder>().artwork_filename + ".mp4";

        // offMaterial = thumbnail;

        // videoMaterial = new Material(Shader.Find(shaderUsed));
        // videoMaterial.color = Color.white;

        // videoPlayer.clip = videoClip;

        // VideoSettle();
        // IsReady = true;
    }

    public void Prepare(Texture tmp_texture)
    {
        thumbnail = tmp_texture;
        transform.localScale = new Vector3(
            transform.localScale.x * thumbnail.width / thumbnail.height,
            transform.localScale.y,
            transform.localScale.z
        );

        videoRenderer = GetComponent<Renderer>();
        videoRenderer.material.mainTexture = thumbnail;
        
        offMaterial = videoRenderer.material;

        videoMaterial = new Material(Shader.Find(shaderUsed));
        videoMaterial.color = Color.white;

        //videoPlayer.clip = videoClip;

        IsPrepared = true;

        Debug.Log("video aspect ratio = " + thumbnail.width / thumbnail.height);
    }

    public void SettleVideo(){
        FileName = GetComponent<NFT_InfoRecorder>().artwork_filename + ".mp4";
        
        videoPlayer = GetComponent<VideoPlayer>();

        meshRenderer = GetComponent<MeshRenderer>();
        
        videoPlayer.playOnAwake = false;

        // This will cause our Scene to be visible through the video being played.
        //vp.targetCameraAlpha = 0.5F;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        videoPlayer.url = loadingPath + FileName;

        // Skip the first 100 frames.
        videoPlayer.frame = 100;

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, this.GetComponent<AudioSource>());
        videoPlayer.IsAudioTrackEnabled(0);
    }
    private void Start()
    {
        if (playAtStart)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    public void Play()
    {
        ApplyVideoMaterial();
        videoPlayer.Play();
    }

    public void Stop()
    {
        if(meshRenderer!=null) meshRenderer.material = offMaterial;
        if(videoPlayer!=null) videoPlayer.Stop();
    }

    public void TogglePlayStop()
    {
        bool isPlaying = !videoPlayer.isPlaying;
        SetPlay(isPlaying);
    }

    public void TogglePlayPause()
    {
        meshRenderer.material = videoMaterial;

        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    public void SetPlay(bool value)
    {
        if (value)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    private void ApplyVideoMaterial()
    {
        meshRenderer.material = videoMaterial;
    }

    private void OnValidate()
    {
            
        if (TryGetComponent(out VideoPlayer videoPlayer))
            videoPlayer.targetMaterialProperty = "_BaseMap";
    }
}