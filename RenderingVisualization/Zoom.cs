using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Zooming4 : MonoBehaviour
{
    public GameObject myObject;
    // public float targetScale = 2.0f;
    public float targetScale;
    public float zoomSpeed = 0.1f;

    private bool isZooming = true;

    void Start()
    {
        // program starts by reading targetScale value from .txt file
        ReadTargetScaleFromFile();
    }

    void Update()
    {
        if (targetScale != 0)
        {

            if (isZooming)
            {
                // getting current scale of the object
                Vector3 currentScale = myObject.transform.localScale;

                // calculating new scale using the zoom factor
                float newScale = currentScale.x * SetZoomSpeed();

                // stopping the new scale from exceeding the target scale
                newScale = Mathf.Min(newScale, targetScale);

                // apply the scale
                myObject.transform.localScale = new Vector3(newScale, newScale, newScale);

                // checking if the object has reached the target scale
                if (Mathf.Approximately(newScale, targetScale))
                {
                    isZooming = false; // stop zooming once zoomed in sufficiently
                    Debug.Log("Object reached target scale.");
                }
            }
        }


    }

    float SetZoomSpeed()
    {
        return 1.0f + zoomSpeed; // Increase scale by zoom speed
    }


    void ReadTargetScaleFromFile()
    {
        string filePath = Application.dataPath + "/targetScale.txt"; // path to the .txt file

        // checking if file exists
        if (File.Exists(filePath))
        {
            // reading the contents of the file as a string
            string fileContent = File.ReadAllText(filePath);

            // attempting to parse the content as a float
            if (float.TryParse(fileContent, out float parsedTargetScale))
            {
                // assign the value from the .txt file to targetScale variable
                targetScale = parsedTargetScale;
                // print success message
                Debug.Log("Target scale (read from file): " + targetScale);
            }
            else
            {
                // could not parse scaling factor as a float
                Debug.LogError("Failed to parse target scale from file.");
            }
        }
        else
        {
            // file not found error
            Debug.LogError("targetScale.txt file not found.");
        }
    }
}