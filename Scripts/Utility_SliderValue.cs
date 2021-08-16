using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Utility_SliderValue : MonoBehaviour
{
    //--- Public Variables ---//
    public Text attachedText;
    public Slider attachedSlider;



    //--- Unity Functions ---//
    private void Start()
    {
        // Init the private variables
        attachedSlider = GetComponentInParent<Slider>();

        // Set up the text initially
        updateText();
    }



    //--- Methods ---//
    public void updateText()
    {
        // Get the value from the slider
        float value = attachedSlider.value;

        // Convert to a string and format
        string newText = value.ToString("F2");

        // Set the text
        attachedText.text = newText;
    }
}
