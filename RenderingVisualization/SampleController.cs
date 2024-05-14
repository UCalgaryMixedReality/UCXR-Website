using UnityEngine;
using System.IO;
using System;

public class Controller : MonoBehaviour
{
    public Rotate rotateScript;
    public Zoom zoomScript;
    public Drag dragScript;

    // Initial type value
    public float typeis;
    public float value1;
    public float value2;

    private void Start()
    {
        // Read gesture type from file
        ReadTypeFromFile();
    }

    void Update()
    {
        Debug.Log("Current Gesture: " + typeis);

        if (typeis == 1)
        {
            // Activate zoom
            zoomScript.StartZoom();
            Debug.Log("Zoom script activated");

            // Deactivate other functions
            rotateScript.DoNotRotate();
            dragScript.DoNotDrag();
        }
        else if (typeis == 2)
        {
            // Activate rotation
            rotateScript.StartRotate();
            Debug.Log("Rotate script activated");

            // Deactivate other functions
            zoomScript.DoNotZoom();
            dragScript.DoNotDrag();
        }
        else if (typeis == 3)
        {
            // Activate drag
            dragScript.StartDrag();
            Debug.Log("Drag script activated");

            // Deactivate other functions
            rotateScript.DoNotRotate();
            zoomScript.DoNotZoom();
        }
        else
        {
            Debug.LogWarning("Invalid gesture type: " + typeis);
        }
    }

    void ReadTypeFromFile()
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
                    // Parse "gesture" value (first part)
                    if (float.TryParse(parts[0], out float parsedTypeis))
                    {
                        typeis = parsedTypeis;
                        Debug.Log($"Read Gesture: {typeis}");

                        if (typeis == 1)
                        {
                            // Parse "type" value (second part)
                            if (float.TryParse(parts[1], out float parsedValue1)) // Zoom
                            {
                                value1 = parsedValue1;
                                Debug.Log($"Gesture: {typeis}, Value: {value1}");
                            }
                            else
                            {
                                Debug.LogWarning($"Failed to parse 1st value in line: {line}");
                            }
                        }

                        // Increase in parts depending on gesture
                        if (typeis > 1) // Rotate, select, drag
                        {
                            if (parts.Length > 2 && float.TryParse(parts[2], out float parsedValue2))
                            {
                                value2 = parsedValue2;
                                Debug.Log($"Gesture: {typeis}, 2nd value: {value2}");
                            }
                            else
                            {
                                Debug.LogWarning($"Failed to parse 2nd value in line: {line}");
                            }
                        }
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
