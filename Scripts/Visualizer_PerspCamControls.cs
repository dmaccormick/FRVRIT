using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Visualizer_PerspCamControls : MonoBehaviour
{
    //--- Public Variables ---//
    public float movementSpeed;
    public float rotationSpeed;
    public GameObject mainView;



    //--- Private Variables ---//
    private Camera attachedCamera;



    //--- Unity Functions ---//
    private void Start()
    {
        // Init the private variables
        attachedCamera = GetComponent<Camera>();
    }

    void Update()
    {
        // If the main camera isn't enabled, we shouldn't be able to control it
        if (!attachedCamera.enabled)
            return;

        // Check if the mouse is over the main view. If it is, we can move around
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Set up the pointer data
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            // Check what is below the pointer
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            // See if the main view is one of the objects under the pointer
            if (raycastResults.Count > 0)
            {
                foreach (RaycastResult raycastResult in raycastResults)
                {
                    // If the main view is under the pointer, we can control the camera
                    if (raycastResult.gameObject == mainView)
                    {
                        // X and Z movement comes from WASD
                        float xMovement = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;
                        float zMovement = Input.GetAxisRaw("Vertical") * movementSpeed * Time.deltaTime;
                        float yMovement = 0.0f;

                        // Y movement comes from space and LCTRL
                        if (Input.GetKey(KeyCode.Space))
                            yMovement = movementSpeed * Time.deltaTime;
                        else if (Input.GetKey(KeyCode.LeftControl))
                            yMovement = -movementSpeed * Time.deltaTime;

                        // Move along all of the axes
                        this.transform.position += transform.TransformVector(new Vector3(xMovement, yMovement, zMovement));

                        // The user can rotate the camera by holding down left click
                        if (Input.GetMouseButton(0))
                        {
                            // Rotate yaw and pitch, but not roll
                            float yawMovement = Input.GetAxisRaw("Mouse X") * rotationSpeed;
                            float pitchMovement = -Input.GetAxisRaw("Mouse Y") * rotationSpeed; // Inverted to fix actual inversion

                            // Perform the rotations
                            // This has an issue where gimbal lock is a possibility
                            this.transform.localEulerAngles += new Vector3(pitchMovement, yawMovement, 0.0f);
                        }
                    }
                }
            }
        }   
    }
}
