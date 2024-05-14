using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Drag : MonoBehaviour
{
    string filePath = Application.dataPath + "/type.txt"; // Path to the text file containing the coordinates
    public float speed = 1.0f; // Speed at which the object moves

    private List<Vector3> coordinates = new List<Vector3>(); // List to store coordinates
    private int currentIndex = 0; // Index of the current coordinate

    // Initial start
    private bool isDragging = false;
    private float proximityThreshold = 0.1f; // Threshold to consider the object has reached the coordinate

    void Start()
    {
        // Read coordinates from the text file
        ReadCoordinatesFromFile();
        Debug.Log("Reading coordinates from file: " + filePath);
    }

    void Update()
    {
        if (isDragging)
        {
            // Check if there are coordinates to follow
            if (coordinates.Count == 0)
            {
                Debug.Log("No coordinates found!");
                return;
            }

            // Move the object towards the current coordinate
            Vector3 targetPosition = coordinates[currentIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            Debug.Log($"Moving towards: {targetPosition}, Current position: {transform.position}");

            // If the object has reached the current coordinate, move to the next one
            if (Vector3.Distance(transform.position, targetPosition) < proximityThreshold)
            {
                currentIndex++;
                if (currentIndex >= coordinates.Count)
                {
                    isDragging = false; // Stop dragging
                    Debug.Log("Reached the final coordinate, stopping.");
                }
                else
                {
                    Debug.Log($"Reached coordinate, moving to index: {currentIndex}");
                }
            }
        }
    }

    public void StartDrag()
    {
        isDragging = true;
        Debug.Log("Dragging started.");
    }

    public void DoNotDrag()
    {
        isDragging = false;
        Debug.Log("Dragging stopped.");
    }

    void ReadCoordinatesFromFile()
    {
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found at path: " + filePath);
            return;
        }

        // Read each line from the file and parse it as a Vector3 coordinate
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length >= 3)
            {
                if (float.TryParse(values[1], out float x) &&
                    float.TryParse(values[2], out float y))
                {
                    float z = 0;
                    coordinates.Add(new Vector3(x, y, z));
                    Debug.Log($"Coordinate added: {new Vector3(x, y, z)}");
                }

                else
                {
                    Debug.LogError("Failed to parse coordinate: " + line);
                }
            }
        }
    }
}
