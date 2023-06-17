using UnityEngine;
using UnityEngine.UI;
using OpenCvSharp;
using OpenCvSharp.Face;
using OpenCvSharp.Util;
using OpenCvSharp.Tracking;
using OpenCvSharp.Detail;
using OpenCvSharp.Aruco;
using OpenCvSharp.ML;
using System;

public class CameraController : MonoBehaviour
{
    public RawImage rawImage;  // Reference to the RawImage component for displaying the camera feed

    private WebCamTexture webCamTexture;  // Reference to the WebCamTexture
    private void Start()
    {

    }

    private void OnEnable()
    {
        // Check for camera permissions (Android only)
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif

        // Start the camera
        StartCamera();
        // Initialize face cascade and recognizer
    }

    private void OnDisable()
    {
        // Stop the camera
        StopCamera();
    }

    private void StartCamera()
    {
        // Get the available devices (cameras)
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No cameras found on the device.");
            return;
        }

        // Select the desired camera (you can use the first camera by default)
        WebCamDevice selectedCamera = devices[0];

        // Create a new WebCamTexture using the selected camera
        webCamTexture = new WebCamTexture(selectedCamera.name);

        // Assign the WebCamTexture to the RawImage component's texture
        rawImage.texture = webCamTexture;

        // Start the camera
        webCamTexture.Play();
    }

    private void StopCamera()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            // Stop the camera
            webCamTexture.Stop();
        }
    }

}
