using UnityEngine;
using System.IO;
using System;
using UnityEditor.Experimental.GraphView;

public class Rotate : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 30f;

    // Default direction
    public float directionLR;
    public float directionUD;

    // Target rotation angle (in degrees)
    private float targetRotationAngle = 15f;

    // Initial start
    private bool isRotating = false;

    private void Start()
    {
        // Read rotation angle from file
        ReadRotationFromFile();
    }

    void Update()
    {
        if (isRotating)
        {
            float rotationAngleLR = Mathf.Sign(directionLR) * targetRotationAngle;
            float rotationAngleUD = Mathf.Sign(directionUD) * targetRotationAngle;
            float rotationAmountLR = Mathf.Sign(directionLR) * rotationSpeed * Time.deltaTime;
            float rotationAmountUD = Mathf.Sign(directionUD) * rotationSpeed * Time.deltaTime;

            if (directionLR != 0)
            {
                // Rotate the object around its Y-axis, aka left (+) or right (-)
                transform.Rotate(Vector3.up, rotationAmountLR);

                // Check if the object has rotated enough to reach or exceed the target rotation angle
                Quaternion targetRotation = Quaternion.Euler(0f, rotationAngleLR, 0f);
                float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

                if (angleDifference <= 1.2f)
                {
                    // Stop rotating
                    enabled = false;
                    Debug.Log("Rotation complete. Object stopped rotating.");
                }
            }
            else if (directionUD != 0)
            {
                // Rotate the object around its X-axis, aka up (+) or down (-)
                transform.Rotate(Vector3.right, rotationAmountUD);

                // Check if the object has rotated enough to reach or exceed the target rotation angle
                Quaternion targetRotation = Quaternion.Euler(rotationAngleUD, 0f, 0f);
                float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

                if (angleDifference <= 1.2f)
                {
                    // Stop rotating
                    enabled = false;
                    Debug.Log("Rotation complete. Object stopped rotating.");
                }
            }
        }
    }

    public void StartRotate()
    {
        isRotating = true;
    }

    public void DoNotRotate()
    {
        isRotating = false;
    }
    void ReadRotationFromFile()
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
                        if (float.TryParse(parts[1], out float parsedDirectionLR))
                        {
                            if (parsedDirectionLR != 0)
                            {
                                // Now you have the parsed float values
                                directionLR = parsedDirectionLR;
                                
                                Debug.Log($"Gesture: {gestureTrue}, Orientation: L/R, Direction: {directionLR}");
                            }
                            else
                            {
                                if (float.TryParse(parts[2], out float parsedDirectionUD))
                                {
                                    directionUD = parsedDirectionUD;
                                    Debug.Log($"Gesture: {gestureTrue}, Orientation: U/D, Direction: {directionUD}");

                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Failed to parse 'direction' value in line: {line}");
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
