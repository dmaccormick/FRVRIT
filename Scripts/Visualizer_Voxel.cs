using UnityEngine;

public class Visualizer_Voxel : MonoBehaviour
{
    public Color minColor;
    public Color maxColor;
    public int pointsRequiredForMax;

    private Material mat;
    private int numPoints = 0;

    public void addPoint()
    {
        numPoints++;

        if (mat == null)
            mat = GetComponent<Renderer>().material;

        // Lerp to the correct colour
        float t = (float)numPoints / (float)pointsRequiredForMax;

        if (t > 1.0f)
            t = 1.0f;

        mat.color = Color.Lerp(minColor, maxColor, t);
    }

    public bool checkIfPointInVoxel(Vector3 _point)
    {
        Vector3 minVertex = this.transform.position;
        minVertex.x -= this.transform.localScale.x / 2.0f;
        minVertex.y -= this.transform.localScale.y / 2.0f;
        minVertex.z -= this.transform.localScale.z / 2.0f;

        Vector3 maxVertex = this.transform.position;
        maxVertex.x += this.transform.localScale.x / 2.0f;
        maxVertex.y += this.transform.localScale.y / 2.0f;
        maxVertex.z += this.transform.localScale.z / 2.0f;

        if (_point.x >= minVertex.x && _point.x <= maxVertex.x)
        {
            if (_point.y >= minVertex.y && _point.y <= maxVertex.y)
            {
                if (_point.z >= minVertex.z && _point.z <= maxVertex.z)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    public void removeIfUnused()
    {
        if (numPoints == 0)
            this.gameObject.SetActive(false);
    }
}
