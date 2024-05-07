using UnityEngine;
using System.IO;

public class CameraController : MonoBehaviour
{
    public float speed = 5f; // Speed of camera movement
    public string filePath = @"C:\Users\Kunjp\ARmed_Project\Assets\Myfolder\Data.txt"; // File path to read coordinates
    public float verticalSpeed = 2f; // Speed of vertical movement

    private Vector3[] targetPositions; // Array to store target positions for camera movement
    private int currentPositionIndex = 0; // Index to track current target position
    private bool isMoving = false; // Flag to indicate if camera is moving

    void Start()
    {
        ReadCoordinatesFromFile(); // Read coordinates from file
    }

    void Update()
    {
        if (isMoving)
        {
            MoveCameraToNextPosition(); // Move camera if it is in motion
        }
    }

    void ReadCoordinatesFromFile()
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            targetPositions = new Vector3[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] coordinates = lines[i].Split(',');
                if (coordinates.Length >= 2)
                {
                    float x = float.Parse(coordinates[0]);
                    float y = float.Parse(coordinates[1]);
                    // Set z coordinate to 0 explicitly
                    targetPositions[i] = new Vector3(x, y, 0);
                }
                else
                {
                    Debug.LogError("Coordinates format in file is incorrect.");
                }
            }

            StartMoving(); // Start moving the camera
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading coordinates from file: " + e.Message);
        }
    }

    void StartMoving()
    {
        if (targetPositions.Length > 0)
        {
            isMoving = true;
            MoveCameraToNextPosition(); // Start moving towards the first position
        }
        else
        {
            Debug.LogError("No coordinates found in the file.");
        }
    }

    void MoveCameraToNextPosition()
    {
        if (currentPositionIndex < targetPositions.Length)
        {
            Vector3 nextPosition = targetPositions[currentPositionIndex];
            // Move camera towards target position using Lerp
            transform.position = Vector3.Lerp(transform.position, new Vector3(nextPosition.x, nextPosition.y, transform.position.z), speed * Time.deltaTime);

            // Check if camera is close enough to target position
            if (Vector3.Distance(transform.position, new Vector3(nextPosition.x, nextPosition.y, transform.position.z)) < 0.01f)
            {
                currentPositionIndex++; // Move to the next position
            }
        }
        else
        {
            isMoving = false; // Stop moving when all positions are reached
            Debug.Log("End of file reached.");
        }
    }

    void FixedUpdate()
    {
        // Optional: Implement vertical movement
        float verticalInput = Input.GetAxis("Vertical");
        float verticalMovement = verticalInput * verticalSpeed * Time.fixedDeltaTime;
        transform.Translate(0f, verticalMovement, 0f);
    }
}
