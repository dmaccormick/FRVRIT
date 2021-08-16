using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Visualizer_VoxelVisualization : MonoBehaviour
{
    public GameObject voxelPrefab;

    public Vector3 minPoint;
    public Vector3 maxPoint;
    public int numXAxis;
    public int numYAxis;
    public int numZAxis;
    public int maxNumForFullColour;

    public Toggle tog_hmdHeatmap;
    public Toggle tog_HandAHeatmap;
    public Toggle tog_HandBHeatmap;
    public Toggle tog_FootAHeatmap;
    public Toggle tog_FootBHeatmap;
    public Slider slider_numXAxis;
    public Slider slider_numYAxis;
    public Slider slider_numZAxis;
    public Slider slider_maxNumForFullColour;

    private Timeline_DataSet dataSet;

    public void cleanUpHeatmap()
    {
        // Remove any of the existing heatmap voxels
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        for (int i = 0; i < children.Count; i++)
        {
            Destroy(children[i].gameObject);
        }
    }

    public void startHeatmapVisualization()
    {
        tog_hmdHeatmap.isOn = true;
        tog_HandAHeatmap.isOn = false;
        tog_HandBHeatmap.isOn = false;
        tog_FootAHeatmap.isOn = false;
        tog_FootBHeatmap.isOn = false;
        updateHeatmapVisualization();
    }

    public void createVisualization(Timeline_HardwareType _hardwareType)
    {
        // Clean up the existing heatmap objects
        cleanUpHeatmap();

        // Create the new heatmap
        dataSet = GameObject.FindObjectOfType<Visualizer_DataParser>().getDataSet();

        for (int i = 0; i < numXAxis; i++)
        {
            for (int j = 0; j < numYAxis; j++)
            {
                for (int k = 0; k < numZAxis; k++)
                {
                    // Calculate the scale on each axis (distance between points / number on that axis = scale)
                    Vector3 spawnScale = Vector3.one;
                    spawnScale.x = (maxPoint.x - minPoint.x) / (float)numXAxis;
                    spawnScale.y = (maxPoint.y - minPoint.y) / (float)numYAxis;
                    spawnScale.z = (maxPoint.z - minPoint.z) / (float)numZAxis;

                    // Calculate the spawn position
                    Vector3 spawnPos = Vector3.zero;
                    spawnPos.x = Mathf.Lerp(minPoint.x, maxPoint.x, (float)i / (float)numXAxis);
                    spawnPos.x += (spawnScale.x / 2.0f);
                    spawnPos.y = Mathf.Lerp(minPoint.y, maxPoint.y, (float)j / (float)numYAxis);
                    spawnPos.y += (spawnScale.y / 2.0f);
                    spawnPos.z = Mathf.Lerp(minPoint.z, maxPoint.z, (float)k / (float)numZAxis);
                    spawnPos.z += (spawnScale.z / 2.0f);

                    // Spawn the voxel
                    GameObject voxelInstance = Instantiate(voxelPrefab, spawnPos, Quaternion.identity, this.transform);
                    voxelInstance.transform.localScale = spawnScale;
                    Visualizer_Voxel voxelController = voxelInstance.GetComponent<Visualizer_Voxel>();
                    voxelController.pointsRequiredForMax = maxNumForFullColour;

                    // Colour the voxel according to which data set it matches and if points fall into this voxel
                    List<Timeline_DataPoint> dataPoints;
                    if (_hardwareType == Timeline_HardwareType.HMD)
                        dataPoints = dataSet.hmdData;
                    else if (_hardwareType == Timeline_HardwareType.HAND_A)
                        dataPoints = dataSet.handAData;
                    else if (_hardwareType == Timeline_HardwareType.HAND_B)
                        dataPoints = dataSet.handBData;
                    else if (_hardwareType == Timeline_HardwareType.FOOT_A)
                        dataPoints = dataSet.footAData;
                    else
                        dataPoints = dataSet.footBData;

                    for (int l = 0; l < dataPoints.Count; l++)
                    {
                        if (voxelController.checkIfPointInVoxel(dataPoints[l].position))
                            voxelController.addPoint();
                    }

                    // If the voxel is empty, don't show it
                    voxelController.removeIfUnused();
                }
            }
        }
    }

    public void updateHeatmapVisualization()
    {
        numXAxis = (int)slider_numXAxis.value;
        numYAxis = (int)slider_numYAxis.value;
        numZAxis = (int)slider_numZAxis.value;
        maxNumForFullColour = (int)slider_maxNumForFullColour.value;

        if (tog_hmdHeatmap.isOn)
            createVisualization(Timeline_HardwareType.HMD);
        else if (tog_HandAHeatmap.isOn)
            createVisualization(Timeline_HardwareType.HAND_A);
        else if (tog_HandBHeatmap.isOn)
            createVisualization(Timeline_HardwareType.HAND_B);
        else if (tog_FootAHeatmap.isOn)
            createVisualization(Timeline_HardwareType.FOOT_A);
        else if (tog_FootBHeatmap.isOn)
            createVisualization(Timeline_HardwareType.FOOT_B);
    }
}
