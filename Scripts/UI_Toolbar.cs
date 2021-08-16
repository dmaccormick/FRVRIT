using UnityEngine;
using UnityEngine.UI;
using GracesGames.SimpleFileBrowser.Scripts;

public class UI_Toolbar : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("UI")]
    public InputField filePath;
    public DemoCaller fileBrowser;
    public Button btn_LineRenderMode;
    public Button btn_VoxelRenderMode;
    public Button btn_PointRenderMode;
    public Button btn_Screenshot;

    [Header("Distance Values")]
    public Text txt_HMDDistance;
    public Text txt_HandADistance;
    public Text txt_HandBDistance;
    public Text txt_FootADistance;
    public Text txt_FootBDistance;
    public Text txt_TotalDistance;

    [Header("Visualization System")]
    public Visualizer_DataParser dataParser;



    //--- Private Variables ---//
    string fileName;



    //--- Unity Functions ---//
    private void Start()
    {
        // Init the private variables
        fileName = "NO_FILE";
    }



    //--- Methods ---//
    public void onOpenPressed()
    {
        // Open the file browser in loading mode
        fileBrowser.OpenFileBrowser(false);
    }

    public void onFileRead(string _fullPath, string _contents)
    {
        // Update the text at the top that shows the file path
        fileName = extractFileName(_fullPath);
        filePath.text = _fullPath;

        // Pass the data to the parsing system so it can begin to set up the visualization
        dataParser.parseData(_contents);

        // Enable the other buttons
        btn_LineRenderMode.interactable = true;
        btn_VoxelRenderMode.interactable = true;
        btn_PointRenderMode.interactable = true;
        btn_Screenshot.interactable = true;

        // Emulate if the user had pressed the line render mode and get that set up for the new data
        btn_LineRenderMode.onClick.Invoke();

        // Update the distance text on the bottom of the screen
        updateDistances();
    }

    public void updateDistances()
    {
        // Get the distances for each of the data sets
        float hmdDistance = dataParser.calculateDistance(Timeline_HardwareType.HMD);
        float handADistance = dataParser.calculateDistance(Timeline_HardwareType.HAND_A);
        float handBDistance = dataParser.calculateDistance(Timeline_HardwareType.HAND_B);
        float footADistance = dataParser.calculateDistance(Timeline_HardwareType.FOOT_A);
        float footBDistance = dataParser.calculateDistance(Timeline_HardwareType.FOOT_B);

        // Calculate the total distance
        float totalDistance = hmdDistance + handADistance + handBDistance + footADistance + footBDistance;

        // Update the text values
        txt_HMDDistance.text = "HMD: " + hmdDistance + "m";
        txt_HandADistance.text = "Hand A: " + handADistance + "m";
        txt_HandBDistance.text = "Hand B: " + handBDistance + "m";
        txt_FootADistance.text = "Foot A: " + footADistance + "m";
        txt_FootBDistance.text = "Foot B: " + footBDistance + "m";
        txt_TotalDistance.text = "Total: " + totalDistance + "m";
    }



    //--- Getters ---//
    public string getFileName()
    {
        return fileName;
    }



    //--- Utility Functions ---//
    private string extractFileName(string _fullPath)
    {
        // Split up by the folder divider
        string[] tokens = _fullPath.Split('\\');

        // The last one is the file name, including the extension. Need to remove the extension
        string fileNameWithExt = tokens[tokens.Length - 1];
        string fileNameNoExt = fileNameWithExt.Split('.')[0];

        // Return the extracted file name
        return fileNameNoExt;
    }
}
