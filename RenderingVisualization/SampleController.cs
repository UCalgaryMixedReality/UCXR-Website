using UnityEngine;
using System.IO;

public class Controller : MonoBehaviour
{
    public Rotate rotateScript;
    public Zoom zoomScript;

    // Initial gesture value
    public float gesture = 10;

    private void Start()
    {
        // Read gesture type from file
        ReadTypeFromFile();
    }

    void Update()
    {
        if (gesture == 1)
        {
            // Activate rotation
            rotateScript.StartRotate();

            // Deactivate other functions
            zoomScript.DoNotZoom();
        }
        else if (gesture == 2)
        {
            // Activate zoom
            zoomScript.StartZoom();

            // Deactivate other functions
            rotateScript.DoNotRotate();
        }
    }
    void ReadTypeFromFile()
    {
        string filePath = Application.dataPath + "/type.txt"; // Path to the .txt file

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the contents of the file
            string fileContent = File.ReadAllText(filePath);

            // Parse contents
            if (float.TryParse(fileContent, out float parsedGesture))
            {
                // Assign the parsed value to direction
                gesture = parsedGesture;
                Debug.Log("Gesture read from file: " + gesture);
            }
            else
            {
                Debug.LogError("Failed to parse gesture type from file.");
            }
        }
        else
        {
            Debug.LogError("File not found.");
        }
    }
}
