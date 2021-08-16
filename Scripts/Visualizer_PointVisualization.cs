using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Visualizer_PointVisualization : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("General")]
    public GameObject pointPrefab;

    [Header("Control UI")]
    public Toggle tog_Head;
    public Toggle tog_HandA;
    public Toggle tog_HandB;
    public Toggle tog_FootA;
    public Toggle tog_FootB;
    public Slider slider_PointScale;

    [Header("Colours")]
    public Color col_Head;
    public Color col_HandA;
    public Color col_HandB;
    public Color col_FootA;
    public Color col_FootB;

    [Header("Cloud Object Meshes")]
    public MeshFilter head_Mesh;
    public MeshFilter handA_Mesh;
    public MeshFilter handB_Mesh;
    public MeshFilter footA_Mesh;
    public MeshFilter footB_Mesh;



    //--- Private Variables ---//
    private Timeline_DataSet timelineData;



    //--- Methods ---//
    public void startPointVisualization()
    {
        // Get the timeline data
        timelineData = GameObject.FindObjectOfType<Visualizer_DataParser>().getDataSet();

        // Create the meshes and assign them to the gameobjects
        createAllMeshes();

        // Update the scale of all of the points
        updatePointScale();
    }

    public void createAllMeshes()
    {
        // Create the meshes and assign them to the gameobjects
        head_Mesh.mesh = createMeshFromData(Timeline_HardwareType.HMD, col_Head);
        handA_Mesh.mesh = createMeshFromData(Timeline_HardwareType.HAND_A, col_HandA);
        handB_Mesh.mesh = createMeshFromData(Timeline_HardwareType.HAND_B, col_HandB);
        footA_Mesh.mesh = createMeshFromData(Timeline_HardwareType.FOOT_A, col_FootA);
        footB_Mesh.mesh = createMeshFromData(Timeline_HardwareType.FOOT_B, col_FootB);

        // Turn on and off each of the meshes in the scene
        toggleClouds();
    }

    public Mesh createMeshFromData(Timeline_HardwareType _hardwareType, Color _pointColour)
    {
        // Get the corresponding points data
        List<Timeline_DataPoint> pointData = null;
        if (_hardwareType == Timeline_HardwareType.HMD)
            pointData = timelineData.hmdData;
        else if (_hardwareType == Timeline_HardwareType.HAND_A)
            pointData = timelineData.handAData;
        else if (_hardwareType == Timeline_HardwareType.HAND_B)
            pointData = timelineData.handBData;
        else if (_hardwareType == Timeline_HardwareType.FOOT_A)
            pointData = timelineData.footAData;
        else if (_hardwareType == Timeline_HardwareType.FOOT_B)
            pointData = timelineData.footBData;

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Create the arrays that represent the mesh data 
        Vector3[] meshPoints = new Vector3[pointData.Count];
        int[] indicies = new int[pointData.Count];
        Color[] colors = new Color[pointData.Count];

        // Assign the data to the arrays
        for (int i = 0; i < pointData.Count; i++)
        {
            meshPoints[i] = pointData[i].position;
            indicies[i] = i;
            colors[i] = _pointColour;
        }

        // Assign the arrays to the mesh
        mesh.vertices = meshPoints;
        mesh.SetIndices(indicies, MeshTopology.Points, 0);
        mesh.colors = colors;

        // Return the newly created mesh
        return mesh;
    }
    
    public void cleanupPoints()
    {
        // Hide each of the meshes
        head_Mesh.gameObject.SetActive(false);
        handA_Mesh.gameObject.SetActive(false);
        handB_Mesh.gameObject.SetActive(false);
        footA_Mesh.gameObject.SetActive(false);
        footB_Mesh.gameObject.SetActive(false);
    }

    public void updatePointScale()
    {
        // Get the new size from the slider
        float newPointSize = slider_PointScale.value;

        // Change the scale of the points in the heatmap shaders
        // --- NOTE: This requires the renderer to be set to OpenGL --- //
        head_Mesh.GetComponent<MeshRenderer>().material.SetFloat("_PointSize", newPointSize);
        handA_Mesh.GetComponent<MeshRenderer>().material.SetFloat("_PointSize", newPointSize);
        handB_Mesh.GetComponent<MeshRenderer>().material.SetFloat("_PointSize", newPointSize);
        footA_Mesh.GetComponent<MeshRenderer>().material.SetFloat("_PointSize", newPointSize);
        footB_Mesh.GetComponent<MeshRenderer>().material.SetFloat("_PointSize", newPointSize);
    }

    public void toggleClouds()
    {
        // Turn on or off each indivdual cloud
        head_Mesh.gameObject.SetActive(tog_Head.isOn);
        handA_Mesh.gameObject.SetActive(tog_HandA.isOn);
        handB_Mesh.gameObject.SetActive(tog_HandB.isOn);
        footA_Mesh.gameObject.SetActive(tog_FootA.isOn);
        footB_Mesh.gameObject.SetActive(tog_FootB.isOn);
    }
}
