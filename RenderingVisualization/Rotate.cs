using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 15f;
    private List<Vector3> rotationInstructions = new List<Vector3>(); // List to store rotation instructions
    private int currentInstructionIndex = 0;
    private bool isRotating = false;
    private float proximityThreshold = 5f; // Threshold to consider the rotation complete
    private Quaternion targetRotation;

    void Start()
    {
        ReadRotationFromFile();
        StartCoroutine(ProcessInstructions());
    }

    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation is close enough to the target
            if (Quaternion.Angle(transform.rotation, targetRotation) < proximityThreshold)
            {
                isRotating = false;
                currentInstructionIndex++;
                if (currentInstructionIndex < rotationInstructions.Count)
                {
                    SetNextRotation();
                }
                else
                {
                    Debug.Log("All rotations completed.");
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

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found at path: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            if (parts.Length >= 3)
            {
                if (float.TryParse(parts[0], out float gestureTrue) &&
                    float.TryParse(parts[1], out float parsedDirectionLR) &&
                    float.TryParse(parts[2], out float parsedDirectionUD))
                {
                    rotationInstructions.Add(new Vector3(parsedDirectionLR, parsedDirectionUD, 0));
                    Debug.Log($"Rotation instruction added: LR {parsedDirectionLR}, UD {parsedDirectionUD}");
                }
                else
                {
                    Debug.LogError("Failed to parse rotation instruction: " + line);
                }
            }
            else
            {
                Debug.LogWarning("Invalid line format: " + line);
            }
        }
    }

    IEnumerator ProcessInstructions()
    {
        while (currentInstructionIndex < rotationInstructions.Count)
        {
            SetNextRotation();
            isRotating = true;
            yield return new WaitUntil(() => !isRotating);
        }
    }

    void SetNextRotation()
    {
        if (currentInstructionIndex < rotationInstructions.Count)
        {
            Vector3 instruction = rotationInstructions[currentInstructionIndex];
            Vector3 currentEulerAngles = transform.rotation.eulerAngles;

            float rotationAngleLR = instruction.x * 10f; // Default rotation angle multiplier
            float rotationAngleUD = instruction.y * 10f;

            if (instruction.x > 2)
            {
                targetRotation = Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y + rotationAngleLR, currentEulerAngles.z);
            }
            else if (instruction.y > 2)
            {
                targetRotation = Quaternion.Euler(currentEulerAngles.x + rotationAngleUD, currentEulerAngles.y, currentEulerAngles.z);
            }
            else if (instruction.x != 0)
            {
                targetRotation = Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y + rotationAngleLR, currentEulerAngles.z);
            }
            else if (instruction.y != 0)
            {
                targetRotation = Quaternion.Euler(currentEulerAngles.x + rotationAngleUD, currentEulerAngles.y, currentEulerAngles.z);
            }
            else
            {
                // If both directions are zero, don't change the target rotation
                targetRotation = transform.rotation;
            }

            Debug.Log($"Setting next rotation: {targetRotation.eulerAngles}");
        }
    }
}
