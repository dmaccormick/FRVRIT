using UnityEngine;
using UnityEngine.UI;

public class Visualizer_CameraController : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Main Camera List")]
    public Camera main_Perspective;
    public Camera main_Ortho_Top;
    public Camera main_Ortho_Bottom;
    public Camera main_Ortho_Front;
    public Camera main_Ortho_Back;
    public Camera main_Ortho_Left;
    public Camera main_Ortho_Right;
    public Text text_MainCam; 

    [Header("Side Camera List")]
    public Camera side_Ortho_Top;
    public Camera side_Ortho_Bottom;
    public Camera side_Ortho_Front;
    public Camera side_Ortho_Back;
    public Camera side_Ortho_Left;
    public Camera side_Ortho_Right;
    public Text text_SideCam1;
    public Text text_SideCam2;
    public Text text_SideCam3;



    //--- Unity Functions ---//
    private void Start()
    {
        // Init all of the main cameras
        mainToPerspective();

        // Init all of the side cameras
        side_Ortho_Bottom.enabled = false;
        side_Ortho_Back.enabled = false;
        side_Ortho_Right.enabled = false;

        // Init the side camera texts
        text_SideCam1.text = "Ortho Top";
        text_SideCam2.text = "Ortho Front";
        text_SideCam3.text = "Ortho Left";
    }



    //--- Main Camera Methods ---//
    public void mainToPerspective()
    {
        // Disable all of the ortho cameras
        main_Perspective.enabled = false;
        main_Ortho_Top.enabled = false;
        main_Ortho_Bottom.enabled = false;
        main_Ortho_Front.enabled = false;
        main_Ortho_Back.enabled = false;
        main_Ortho_Left.enabled = false;
        main_Ortho_Right.enabled = false;

        // Enable the perspective camera
        main_Perspective.enabled = true;

        // Update the camera text
        text_MainCam.text = "Controllable Perspective";
    }

    public void mainToOrtho(int _orthoCam)
    {
        // Disable the perspective camera
        main_Perspective.enabled = false;

        // Enable the correct ortho camera
        main_Ortho_Top.enabled = (_orthoCam == 0);
        main_Ortho_Bottom.enabled = (_orthoCam == 1);
        main_Ortho_Front.enabled = (_orthoCam == 2);
        main_Ortho_Back.enabled = (_orthoCam == 3);
        main_Ortho_Left.enabled = (_orthoCam == 4);
        main_Ortho_Right.enabled = (_orthoCam == 5);

        // Set the text for the camera
        if (main_Perspective.enabled)
            text_MainCam.text = "Controllable Perspective";
        else if (main_Ortho_Top.enabled)
            text_MainCam.text = "Ortho Top";
        else if (main_Ortho_Bottom.enabled)
            text_MainCam.text = "Ortho Bottom";
        else if (main_Ortho_Front.enabled)
            text_MainCam.text = "Ortho Front";
        else if (main_Ortho_Back.enabled)
            text_MainCam.text = "Ortho Back";
        else if (main_Ortho_Left.enabled)
            text_MainCam.text = "Ortho Left";
        else if (main_Ortho_Right.enabled)
            text_MainCam.text = "Ortho Right";
    }




    //--- Side Camera Methods ---//
    public void swapTopAndBottom()
    {
        // Switch from top to bottom or vice versa
        side_Ortho_Top.enabled = !side_Ortho_Top.enabled;
        side_Ortho_Bottom.enabled = !side_Ortho_Top.enabled;

        // Set the camera text
        text_SideCam1.text = (side_Ortho_Top.enabled) ? "Ortho Top" : "Ortho Bottom";
    }

    public void swapFrontAndBack()
    {
        // Switch from front to back or vice versa
        side_Ortho_Front.enabled = !side_Ortho_Front.enabled;
        side_Ortho_Back.enabled = !side_Ortho_Front.enabled;

        // Set the camera text
        text_SideCam2.text = (side_Ortho_Front.enabled) ? "Ortho Front" : "Ortho Back";
    }

    public void swapLeftAndRight()
    {
        // Switch from left to right or vice versa
        side_Ortho_Left.enabled = !side_Ortho_Left.enabled;
        side_Ortho_Right.enabled = !side_Ortho_Left.enabled;

        // Set the camera text
        text_SideCam3.text = (side_Ortho_Left.enabled) ? "Ortho Left" : "Ortho Right";
    }
}
