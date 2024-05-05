using UnityEngine;
using System.IO;

public class Rotate : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 30f;

    // Default direction
    public float direction = 30;

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
            // Check if going in clockwise (+) or counterclockwise (-) direction
            Debug.Log(Mathf.Sign(direction));
            float sign = Mathf.Sign(direction);

            // Calculate rotation direction using default 15 degrees
            float rotationAngle = sign * targetRotationAngle;

            // Rotate the object around its Y-axis
            float rotationAmount = sign * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);

            // Check if the object has rotated enough to reach or exceed the target rotation angle
            Quaternion targetRotation = Quaternion.Euler(0f, rotationAngle, 0f);
            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            // Debug logs to check angle difference and target rotation angle
            Debug.Log("Angle Difference: " + angleDifference);
            Debug.Log("Target Rotation Angle: " + rotationAngle);

            // Check if the angle difference is within a small tolerance of the target rotation angle
            if (angleDifference <= 1f)
            {
                // Stop rotating
                enabled = false;
                Debug.Log("Rotation complete. Object stopped rotating.");
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
        string filePath = Application.dataPath + "/gestures.txt"; // Path to the .txt file
                                                                  // File reads a (+) value if swiped left, and (-) value is swiped right

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the contents of the file
            string fileContent = File.ReadAllText(filePath);

            // Parse contents
            if (float.TryParse(fileContent, out float parsedDirection))
            {
                // Assign the parsed value to direction
                direction = parsedDirection;
                Debug.Log("Direction read from file: " + direction);
            }
            else
            {
                Debug.LogError("Failed to parse direction speed from file.");
            }
        }
        else
        {
            Debug.LogError("File not found.");
        }
    }
}
