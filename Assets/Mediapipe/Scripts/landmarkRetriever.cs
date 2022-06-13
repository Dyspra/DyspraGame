using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;

public class landmarkRetriever : MonoBehaviour
{
    public webCameraTexture webCam = null;
    private Texture2D text = null;
    private Bitmap_p bmp = null;
    // Start is called before the first frame update
    void Start()
    {
        webCam = new WebCamTexture();
        webCam.Play();
        originalFeedTexture = new Texture2D(webCam.width, webCam.height);
        originalFeedTexture.SetPixels(webCam.GetPixels());
        originalFeedTexture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        if (webCameraTexture == null)
        {
            Application.Quit()
        }
        originalFeedTexture.SetPixels(webCam.GetPixels());
        originalFeedTexture.Apply();
        bmp = ConvertTexture(originalFeedTexture);
    }

    static Bitmap_p ConvertTexture(Texture2D texture)
    {
        var original = texture.GetPixels32();
        byte[] result = new byte[texture.width * texture.height];

        for (int i = 0; i < original.Length; i++)
        {
            if (original[i].r + original[i].g + original[i].b < Treshold * 255 * 3)
            {
                result[i] = 1;
            }
            else
            {
                result[i] = 0;
            }
        }

        bm = new Bitmap_p(texture.width, texture.height)
        {
            data = result
        };

        return bm;
    }

}
