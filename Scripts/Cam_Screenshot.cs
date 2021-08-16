// Based off: https://www.youtube.com/watch?v=lT-SRLKUe5k&t=335s

using UnityEngine;

public class Cam_Screenshot : MonoBehaviour
{
    //--- Private Variables ---//
    private Camera cam;
    private bool shouldTakeScreenshot;
    private int screenshotWidth;
    private int screenshotHeight;



    //--- Unity Functions ---//
    private void Start()
    {
        // Init the private variables
        cam = GetComponent<Camera>();
        shouldTakeScreenshot = false;
        screenshotWidth = 1920;
        screenshotHeight = 1080;
    }

    private void OnPostRender()
    {
        // If the screenshot is queued up, we should export it and then resume normal rendering
        if (shouldTakeScreenshot)
        {
            // Convert the camera texture into a Texture2D
            RenderTexture tex = cam.targetTexture;
            Texture2D img = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, screenshotWidth, screenshotHeight);
            img.ReadPixels(rect, 0, 0);

            // Convert the texture into raw bytes and export them to the file path
            byte[] rawData = img.EncodeToPNG();
            string path = createImageFileName();
            System.IO.File.WriteAllBytes(path, rawData);

            // Go back to normal render mode
            shouldTakeScreenshot = false;
            RenderTexture.ReleaseTemporary(tex);
            cam.targetTexture = null;
        }
    }



    //--- Methods ---//
    public void takeScreenshot()
    {
        // Should take a screenshot after the next frame is rendered
        shouldTakeScreenshot = true;

        // Assign a temporary render texture to render the screenshot to
        cam.targetTexture = RenderTexture.GetTemporary(screenshotWidth, screenshotHeight, 16);
    }



    //--- Utility Functions ---//
    string createImageFileName()
    {
        // Place the file in the screenshots folder
        string filePath = Application.dataPath + "/Screenshots/";

        // Add the name of the log file that was read in
        UI_Toolbar toolbar = GameObject.FindObjectOfType<UI_Toolbar>();
        string logFileName = toolbar.getFileName();
        filePath += "/";
        filePath += logFileName;
        filePath += "_";

        // Name the file using the following format: logFileName_day-month-year_hh-mm-ss.png
        System.DateTime currentTime = System.DateTime.Now;
        string timeFormat = "dd-mm-yyyy_HH-mm-ss"; 
        string timeStr = currentTime.ToString(timeFormat);
        filePath += timeStr;
        filePath += ".png";

        // Return the final path
        return filePath;
    }
}
