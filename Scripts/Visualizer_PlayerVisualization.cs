using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct Timeline_Visualization
{
    public LineRenderer lineRenderer;
    public Color lineColor;
    public GameObject rendereredObject;
    public List<Timeline_DataPoint> dataPoints;

    public void updateLineRenderer(float _percentage)
    {
        int numPoints = Mathf.CeilToInt(_percentage * dataPoints.Count) + 1;

        if (numPoints >= dataPoints.Count)
            return;

        lineRenderer.positionCount = numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            lineRenderer.SetPosition(i, dataPoints[i].position);
        }

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    public void updateObjectRender(float _percentage)
    {
        int index = Mathf.CeilToInt(_percentage * dataPoints.Count);

        if (dataPoints.Count == 0)
        {
            rendereredObject.transform.position = Vector3.zero;
            rendereredObject.transform.rotation = Quaternion.identity;
        }

        if (index >= dataPoints.Count)
            return;

        rendereredObject.transform.position = dataPoints[index].position;
        rendereredObject.transform.rotation = dataPoints[index].rotation;
    }

    public void toggleObjectRender(bool _active)
    {
        rendereredObject.GetComponentInChildren<MeshRenderer>().enabled = _active;
    }

    public void toggleLineRender(bool _active)
    {
        lineRenderer.enabled = _active;
    }

    public void updateRender(float _percentage)
    {
        updateLineRenderer(_percentage);
        updateObjectRender(_percentage);
    }

    public void reset()
    {
        lineRenderer.positionCount = 0;
        rendereredObject.transform.position = Vector3.zero;
        rendereredObject.transform.rotation = Quaternion.identity;
    }
}

public class Visualizer_PlayerVisualization : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Visualization Objects")]
    public Timeline_Visualization hmdVisualiation;
    public Timeline_Visualization handAVisualization;
    public Timeline_Visualization handBVisualization;
    public Timeline_Visualization footAVisualization;
    public Timeline_Visualization footBVisualization;

    [Header("Toggle Controls")]
    public Toggle tog_hmdObj;
    public Toggle tog_hmdTrail;
    public Toggle tog_HandAObj;
    public Toggle tog_HandATrail;
    public Toggle tog_HandBObj;
    public Toggle tog_HandBTrail;
    public Toggle tog_FootAObj;
    public Toggle tog_FootATrail;
    public Toggle tog_FootBObj;
    public Toggle tog_FootBTrail;

    [Header("Playback Controls")]
    public Slider slider_LineWidth;
    public Slider slider_Timeline;
    public Slider slider_PlaybackDuration;



    //--- Private Variables ---//
    private Timeline_DataSet timelineData;
    private float renderPlaybackTime;
    private float baseLineRenderWidth;
    private bool isPlayingBack;



    //--- Unity Functions ---//
    private void Start()
    {
        // Init the private variables
        renderPlaybackTime = 0.0f;
        baseLineRenderWidth = hmdVisualiation.lineRenderer.startWidth;
        isPlayingBack = false;
    }

    private void Update()
    {
        if (isPlayingBack)
        {
            renderPlaybackTime += Time.deltaTime;

            if (renderPlaybackTime >= slider_PlaybackDuration.value)
            {
                renderPlaybackTime = slider_PlaybackDuration.value;
                isPlayingBack = false;
            }

            float newPercentage = renderPlaybackTime / slider_PlaybackDuration.value;

            updatePlayerVisualization(newPercentage);
            slider_Timeline.value = newPercentage;
        }
    }



    //--- Methods ---//
    public void startPlayerVisualization()
    {
        // Reset visualization
        hmdVisualiation.reset();
        handAVisualization.reset();
        handBVisualization.reset();
        footAVisualization.reset();
        footBVisualization.reset();

        // Reset sliders
        slider_LineWidth.value = 1.0f;
        slider_Timeline.value = 0.0f;
        slider_PlaybackDuration.value = 10.0f;
        isPlayingBack = false;

        // Grab all of the data from the parser
        timelineData = GameObject.FindObjectOfType<Visualizer_DataParser>().getDataSet();
        hmdVisualiation.dataPoints = timelineData.hmdData;
        handAVisualization.dataPoints = timelineData.handAData;
        handBVisualization.dataPoints = timelineData.handBData;
        footAVisualization.dataPoints = timelineData.footAData;
        footBVisualization.dataPoints = timelineData.footBData;

        // Init the visualization to the starting point
        updatePlayerVisualization(0.0f);
    }

    public void updatePlayerVisualization(float _percentage)
    {
        // Update all of the render sets to the start of the 
        hmdVisualiation.updateRender(_percentage);
        handAVisualization.updateRender(_percentage);
        handBVisualization.updateRender(_percentage);
        footAVisualization.updateRender(_percentage);
        footBVisualization.updateRender(_percentage);
    }

    public void onTimeLineValueChanged()
    {
        updatePlayerVisualization(slider_Timeline.value);
    }

    public void onTimeLineVisualOptionsChanged()
    {
        // Enable or disable the object or trails for each of the visualizations
        // Controlled by the checkboxes in the parameter window on the left side of the screen
        hmdVisualiation.toggleObjectRender(tog_hmdObj.isOn);
        hmdVisualiation.toggleLineRender(tog_hmdTrail.isOn);

        handAVisualization.toggleObjectRender(tog_HandAObj.isOn);
        handAVisualization.toggleLineRender(tog_HandATrail.isOn);

        handBVisualization.toggleObjectRender(tog_HandBObj.isOn);
        handBVisualization.toggleLineRender(tog_HandBTrail.isOn);

        footAVisualization.toggleObjectRender(tog_FootAObj.isOn);
        footAVisualization.toggleLineRender(tog_FootATrail.isOn);

        footBVisualization.toggleObjectRender(tog_FootBObj.isOn);
        footBVisualization.toggleLineRender(tog_FootBTrail.isOn);
    }

    public void onLineWidthChanged()
    {
        // Get the new line scale and multiply it by the base line renderer width
        float newLineScale = slider_LineWidth.value * baseLineRenderWidth;

        // Pass the new line width to the renderers
        hmdVisualiation.lineRenderer.widthMultiplier = newLineScale;
        handAVisualization.lineRenderer.widthMultiplier = newLineScale;
        handBVisualization.lineRenderer.widthMultiplier = newLineScale;
        footAVisualization.lineRenderer.widthMultiplier = newLineScale;
        footBVisualization.lineRenderer.widthMultiplier = newLineScale;
    }

    public void startPlayback()
    {
        renderPlaybackTime = slider_Timeline.value * slider_PlaybackDuration.value;
        isPlayingBack = true;
    }

    public void pausePlayback()
    {
        isPlayingBack = false;
    }
}
