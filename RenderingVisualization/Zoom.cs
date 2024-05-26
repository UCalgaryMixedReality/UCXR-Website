using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System;

public class Zoom : MonoBehaviour
{
    public GameObject myObject;
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

    public void StartZoom()
    {
        isZooming = true;
    }

    public void DoNotZoom()
    {
        isZooming = false;
    }

    float SetZoomSpeed()
    {
        return 1.0f + zoomSpeed; // Increase scale by zoom speed
    }

    void ReadTargetScaleFromFile()
    {
        string filePath = Path.Combine(Application.dataPath, "type.txt");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');

                if (parts.Length >= 2) // Ensure there are at least two parts
                {
                    // Parse "gestureTrue" value (first part)
                    if (float.TryParse(parts[0], out float gestureTrue))
                    {
                        // Parse "direction" value (second part)
                        if (float.TryParse(parts[1], out float parsedTargetScale))
                        {
                            // Now you have the parsed float values
                            targetScale = parsedTargetScale;


                            Debug.Log($"Gesture: {gestureTrue}, Target Scale: {targetScale}");
                        }
                        else
                        {
                            Debug.LogWarning($"Failed to parse 'targetScale' value in line: {line}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to parse 'gesture' value in line: {line}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid line format: {line}");
                }
            }
        }
        else
        {
            Debug.LogError($"File not found at path: {filePath}");
        }
    }
}

