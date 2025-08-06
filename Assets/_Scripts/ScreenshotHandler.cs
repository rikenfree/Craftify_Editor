using UnityEngine;
using System.IO;
using System.Collections;

public class ScreenshotHandler : MonoBehaviour
{
    public Camera screenshotCamera;
    public RenderTexture RT;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TakeScreenshot();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            takePics();
        }
    }

    public void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        // Wait for the end of frame to ensure all rendering is done
        yield return new WaitForEndOfFrame();

        // Create a RenderTexture
        //RenderTexture renderTexture = new RenderTexture(width, height, 24);
        //screenshotCamera.targetTexture = RT;
        //screenshotCamera.Render();

        // Create a Texture2D with the width and height of the RenderTexture
        Texture2D screenshot = new Texture2D(RT.width, RT.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, RT.width, RT.height);
        screenshot.ReadPixels(rect, 0, 0);
        screenshot.Apply();

        // Clear the target texture and render texture
        //screenshotCamera.targetTexture = null;
        //Destroy(renderTexture);

        // Encode texture into PNG
        byte[] bytes = screenshot.EncodeToPNG();
        string filename = Path.Combine(Application.persistentDataPath, "screenshot.png");


        // Write to file
        File.WriteAllBytes(filename, bytes);
        Debug.Log($"Screenshot saved to: {filename}");
    }

    public void takePics()
    {

        Camera cam = screenshotCamera;
        int resWidth = cam.pixelWidth;
        int resHeight = cam.pixelHeight;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt;
        cam.Render();
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
        CapeAddonHandler1.Instance.coppedImageTexture = screenShot;
        RenderTexture.active = rt;
        screenShot.ReadPixels(cam.pixelRect, 0, 0);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Path.Combine(Application.persistentDataPath, "screenshot.png");
        Debug.Log("ScreenShotPath ::> " + filename);
        System.IO.File.WriteAllBytes(filename, bytes);
        cam.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
        cam.targetTexture = RT;

    }
}
