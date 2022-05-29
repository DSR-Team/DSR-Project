using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerManager : MonoBehaviour
{
    
    string loadingPath = ".//Assets.//NFT_cache//";
    public string FileName = "myVideo.mp4";
    UnityEngine.Video.VideoPlayer vp;
    bool ToPlay = false;
    bool IsPrepared = false;
    bool IsSettled = false;
    Texture thumbnail;
    Renderer videoRenderer;

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

        IsPrepared = true;
        Debug.Log("video aspect ratio = " + thumbnail.width / thumbnail.height);
    }
    public void SettleVideo()
    {
        FileName = GetComponent<NFT_InfoRecorder>().Title + ".mp4";

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        vp = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        vp.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        vp.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;

        // This will cause our Scene to be visible through the video being played.
        //vp.targetCameraAlpha = 0.5F;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        vp.url = loadingPath + FileName;

        // Skip the first 100 frames.
        vp.frame = 100;

        // Restart from beginning when done.
        vp.isLooping = true;

        // Each time we reach the end, we slow down the playback by a factor of 10.
        vp.loopPointReached += EndReached;

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        //vp.Play();

        IsSettled = true;
        Debug.Log("Video Settle Done");

    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }

    public void SetPlay(bool value)
    {
        if (value)
        {
            ToPlay = true;
        }
        else
        {
            ToPlay = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPrepared && IsSettled)
        {
            if (ToPlay)
            {
                vp.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
                vp.Play();
            }
            else
            {
                vp.Stop();
                vp.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
            }
        }
    }
}
