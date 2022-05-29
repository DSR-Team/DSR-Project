using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;

public class GifPlayerManager : MonoBehaviour
{
    string loadingPath = ".//Assets.//NFT_cache//";
    public string FileName = "MyGif.gif";
    //public Vector2 drawPosition;
    private float fps = 15.0f;
    bool IsPrepared = false;
    bool IsSettled = false;
    bool ToPlay = false;

    List<Texture2D> gifFrames = new List<Texture2D>();
    Renderer GifRenderer;
    Texture thumbnail;
    
    public void Prepare(Texture tmp_texture)
    {
        thumbnail = tmp_texture;
        transform.localScale = new Vector3(
            transform.localScale.x * thumbnail.width / thumbnail.height,
            transform.localScale.y,
            transform.localScale.z
        );
        GifRenderer = GetComponent<Renderer>();
        GifRenderer.material.mainTexture = thumbnail;
        
        IsPrepared = true;
        Debug.Log("Gif aspect ratio = " + thumbnail.width / thumbnail.height);
    }
    public void SettleGIF()
    {
        FileName = GetComponent<NFT_InfoRecorder>().artwork_filename + ".gif";

        var gifImage = Image.FromFile(loadingPath + FileName);
        var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
        int frameCount = gifImage.GetFrameCount(dimension);
        for (int i = 0; i < frameCount; i++)
        {
            gifImage.SelectActiveFrame(dimension, i);
            var frame = new Bitmap(gifImage.Width, gifImage.Height);
            System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
            var frameTexture = new Texture2D(frame.Width, frame.Height);

            for (int x = 0; x < frame.Width; x++)
                for (int y = 0; y < frame.Height; y++)
                {
                    System.Drawing.Color sourceColor = frame.GetPixel(x, y);
                    frameTexture.SetPixel(x, frame.Height - 1 - y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for unknwon reason, y is flipped
                }
            frameTexture.Apply();
            gifFrames.Add(frameTexture);
        }

        IsSettled = true;
        Debug.Log("Gif Settle Done");
    }

    //void OnGUI()
    //{
    //    GUI.DrawTexture(new Rect(drawPosition.x, drawPosition.y, gifFrames[0].width, gifFrames[0].height), gifFrames[(int)(Time.frameCount * fps) % gifFrames.Count]);
    //}

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


    void Update()
    {
        if(IsPrepared && IsSettled){
            if(ToPlay)
            {
                int index = (int)(Time.time * fps);
                index = index % gifFrames.Count;
                GifRenderer.material.mainTexture = gifFrames[index];
                //GetComponent<RawImage> ().texture = frames [index];
            }
            else
            {
                if(GifRenderer!=null) GifRenderer.material.mainTexture = thumbnail;
            }
        }
    }
}