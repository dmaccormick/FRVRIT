using UnityEngine;
using System.Collections.Generic;

public enum Timeline_HardwareType
{
    HMD,
    HAND_A,
    HAND_B,
    FOOT_A,
    FOOT_B,

    INVALID
}

public struct Timeline_DataPoint
{
    public Timeline_HardwareType hardwareType;
    public Vector3 position;
    public Quaternion rotation;
}

public class Timeline_DataSet
{
    public Timeline_DataSet()
    {
        hmdData = new List<Timeline_DataPoint>();
        handAData = new List<Timeline_DataPoint>();
        handBData = new List<Timeline_DataPoint>();
        footAData = new List<Timeline_DataPoint>();
        footBData = new List<Timeline_DataPoint>();
    }

    public void storeDataPoint(Timeline_DataPoint _dataPoint)
    {
        if (_dataPoint.hardwareType == Timeline_HardwareType.HMD)
            hmdData.Add(_dataPoint);
        else if (_dataPoint.hardwareType == Timeline_HardwareType.HAND_A)
            handAData.Add(_dataPoint);
        else if (_dataPoint.hardwareType == Timeline_HardwareType.HAND_B)
            handBData.Add(_dataPoint);
        else if (_dataPoint.hardwareType == Timeline_HardwareType.FOOT_A)
            footAData.Add(_dataPoint);
        else if (_dataPoint.hardwareType == Timeline_HardwareType.FOOT_B)
            footBData.Add(_dataPoint);
        else
            Debug.LogWarning("Warning: INVALID Hardware type on data point!");
    }

    public List<Timeline_DataPoint> hmdData;
    public List<Timeline_DataPoint> handAData;
    public List<Timeline_DataPoint> handBData;
    public List<Timeline_DataPoint> footAData;
    public List<Timeline_DataPoint> footBData;
}

public class Visualizer_DataParser : MonoBehaviour
{
    //--- Public Variables ---//
    private Timeline_DataSet timelineData;
    private char footA_ID;
    private char footB_ID;



    //--- Methods ----//
    public void parseData(string _rawData)
    {
        // Null out the ID's for the feet since they might change between log files
        footA_ID = ' ';
        footB_ID = ' ';

        // Split the full text file into the individual lines
        string[] lines = _rawData.Split('\n');

        // Create a data set to store all store all of the timeline data lists
        timelineData = new Timeline_DataSet();

        // Loop through all of the timeline data, parse it, and store it into the list
        for (int i = 0; i < lines.Length; i++)
        {
            // Split the line into the individual tokens
            string[] tokens = lines[i].Split(',');

            // If there aren't the correct number of tokens, just move on 
            if (tokens.Length != 8)
                continue;

            // Parse and store the data from all of the tokens. If the parsing fails, just move on to the next line
            bool successFlag = true;
            Timeline_DataPoint dataPoint = new Timeline_DataPoint();

            // Pull the type of hardware this datapoint belongs to
            Timeline_HardwareType hardwareType = Timeline_HardwareType.INVALID;
            string hardwareStr = tokens[0];

            // Determine which type of hardware from the pulled string
            if (hardwareStr == "HMD")
                hardwareType = Timeline_HardwareType.HMD;
            else if (hardwareStr == "HAND_A")
                hardwareType = Timeline_HardwareType.HAND_A;
            else if (hardwareStr == "HAND_B")
                hardwareType = Timeline_HardwareType.HAND_B;
            else if (hardwareStr.Contains("FOOT_"))
            {
                // Get the ID number for the foot
                char inFoot_ID = hardwareStr[hardwareStr.Length - 1];

                // Need to assign which ID represents which foot if they aren't assigned yet
                if (footA_ID == ' ')
                    footA_ID = inFoot_ID;
                else if (footB_ID == ' ' && footA_ID != inFoot_ID)
                    footB_ID = inFoot_ID;

                // Read in the data to the correct foot now
                if (hardwareStr == ("FOOT_" + footA_ID))
                    hardwareType = Timeline_HardwareType.FOOT_A;
                else
                    hardwareType = Timeline_HardwareType.FOOT_B;
            }

            // Store the hardware type within the data point
            dataPoint.hardwareType = hardwareType;

            // Parse the position of the data point
            successFlag = float.TryParse(tokens[1], out dataPoint.position.x);
            if (!successFlag) continue;
            successFlag = float.TryParse(tokens[2], out dataPoint.position.y);
            if (!successFlag) continue;
            successFlag = float.TryParse(tokens[3], out dataPoint.position.z);
            if (!successFlag) continue;

            // Parse the rotation of the data point
            successFlag = float.TryParse(tokens[4], out dataPoint.rotation.w);
            if (!successFlag) continue;
            successFlag = float.TryParse(tokens[5], out dataPoint.rotation.x);
            if (!successFlag) continue;
            successFlag = float.TryParse(tokens[6], out dataPoint.rotation.y);
            if (!successFlag) continue;
            successFlag = float.TryParse(tokens[7], out dataPoint.rotation.z);
            if (!successFlag) continue;

            // Flip the w value of the rotation since Unity's quaternions are perfectly flipped from the vive raw one
            dataPoint.rotation.w *= -1.0f;

            // Store the final parsed data point into the corresponding list
            timelineData.storeDataPoint(dataPoint);
        }
    }

    public float calculateDistance(Timeline_HardwareType _hardwareType)
    {
        if (_hardwareType == Timeline_HardwareType.HMD)
        {
            if (timelineData.hmdData.Count < 2)
                return 0.0f;

            float distance = 0.0f;

            for (int i = 0; i < timelineData.hmdData.Count - 1; i++)
            {
                Vector3 distVec = timelineData.hmdData[i + 1].position - timelineData.hmdData[i].position;
                distance += distVec.magnitude;
            }

            return distance;
        }
        else if (_hardwareType == Timeline_HardwareType.HAND_A)
        {
            if (timelineData.handAData.Count < 2)
                return 0.0f;

            float distance = 0.0f;

            for (int i = 0; i < timelineData.handAData.Count - 1; i++)
            {
                Vector3 distVec = timelineData.handAData[i + 1].position - timelineData.handAData[i].position;
                distance += distVec.magnitude;
            }

            return distance;
        }
        else if (_hardwareType == Timeline_HardwareType.HAND_B)
        {
            if (timelineData.handBData.Count < 2)
                return 0.0f;

            float distance = 0.0f;

            for (int i = 0; i < timelineData.handBData.Count - 1; i++)
            {
                Vector3 distVec = timelineData.handBData[i + 1].position - timelineData.handBData[i].position;
                distance += distVec.magnitude;
            }

            return distance;
        }
        else if (_hardwareType == Timeline_HardwareType.FOOT_A)
        {
            if (timelineData.footAData.Count < 2)
                return 0.0f;

            float distance = 0.0f;

            for (int i = 0; i < timelineData.footAData.Count - 1; i++)
            {
                Vector3 distVec = timelineData.footAData[i + 1].position - timelineData.footAData[i].position;
                distance += distVec.magnitude;
            }

            return distance;
        }
        else if (_hardwareType == Timeline_HardwareType.FOOT_B)
        {
            if (timelineData.footBData.Count < 2)
                return 0.0f;

            float distance = 0.0f;

            for (int i = 0; i < timelineData.footBData.Count - 1; i++)
            {
                Vector3 distVec = timelineData.footBData[i + 1].position - timelineData.footBData[i].position;
                distance += distVec.magnitude;
            }

            return distance;
        }

        return 0.0f;
    }



    //--- Setters and Getters ---//
    public Timeline_DataSet getDataSet()
    {
        return timelineData;
    }
}
