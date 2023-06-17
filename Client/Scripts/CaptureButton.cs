using UnityEngine;
using UnityEngine.UI;
using NativeGalleryNamespace;
using System;

public class CaptureButton : MonoBehaviour
{
    public RawImage rawImage;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(CaptureImage);
    }

    private void CaptureImage()
    {
        Texture2D imageTexture = new Texture2D(rawImage.texture.width, rawImage.texture.height);
        imageTexture.SetPixels(((WebCamTexture)rawImage.texture).GetPixels());
        imageTexture.Apply();

        string fileName = "CameraShy_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"; // Set your desired file name here
        string folderName = "MyGame"; // Set your desired folder name here

        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(imageTexture, folderName, fileName, (success, path) =>
        {
            if (success)
            {
                Debug.Log("Image captured and saved: " + path);
            }
            else
            {
                Debug.Log("Failed to save image");
            }

            Destroy(imageTexture);
        });

        Debug.Log("Permission: " + permission);
    }
}
