using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput = 0;

    public float rotationSpeed = 8.0f;
    public float zMovementSpeed = 0.1f;
    public float panMovementSpeed = 0.05f;
    public float zLimitForwards = 2.5f;
    public float zLimitBackwards = 0.3f;
    public float xPanLimit = 0.8f;
    public float yPanLimit = 1.0f;

    public float yOrigin = 0.0f; // y position of the player at the start of the game
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MovePlayer();
        PanPlayer();
    }

    /**
    * @brief Rotate the player if the mouse button is pressed or there is a touch
    */
    void RotatePlayer()
    {
        // Check if the mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            // Get the mouse translation
            horizontalInput = Input.GetAxis("Mouse X");
        }
        else if (Input.touchCount > 0) // Check if there is a touch
        {
            // Get the touch translation
            horizontalInput = Input.GetTouch(0).deltaPosition.x;
        }
        else
        {
            // Reset the translation
            horizontalInput = 0;
        }

        // Rotate the player
        transform.Rotate(Vector3.up *  - horizontalInput * rotationSpeed);
    }

    /**
    * @brief Move the player along the world x axiz (zoom on player)
    */
    void MovePlayer()
    {

        // Mouse wheel scroll
        if (Input.mouseScrollDelta.y != 0) {
            // Check if the player is too close to the camera
            if (transform.position.z >= -zLimitForwards && Input.mouseScrollDelta.y < 0) {
                // Move the player closer along the world z axis
                transform.Translate(Vector3.back * zMovementSpeed, Space.World);
            } else if (transform.position.z <= zLimitBackwards && Input.mouseScrollDelta.y > 0) {
                // Move the player far along the world z axis
                transform.Translate(Vector3.forward * zMovementSpeed, Space.World);
            }
        }

        // Touch zoom
        if (Input.touchCount == 2) {
            // Get the touch positions
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Get the touch positions in the previous frame
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Get the previous distance between the touches
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;

            // Get the current distance between the touches
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Get the difference between the current and previous distance
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Check if the player is too close to the camera
            if (transform.position.z >= -zLimitForwards && deltaMagnitudeDiff < 0) {
                // Move the player closer along the world z axis
                transform.Translate(Vector3.back * zMovementSpeed, Space.World);
            } else if (transform.position.z >= zLimitBackwards && deltaMagnitudeDiff > 0) {
                // Move the player far along the world z axis
                transform.Translate(Vector3.forward * zMovementSpeed, Space.World);
            }
        }

    }

    /**
    * @brief Pan the player along the world y and z axiz
    */
    void PanPlayer()
    {
        // Mouse right click
        if (Input.GetMouseButton(1))
        {
            // Get the mouse translation
            float verticalInput = Input.GetAxis("Mouse Y") * panMovementSpeed;
            float horizontalInput = Input.GetAxis("Mouse X") * panMovementSpeed;

            // Pan the player
            // Check if the movement goes out the pan limits to prevent the player to exit the frame
            if (transform.position.x + horizontalInput < xPanLimit && transform.position.x + horizontalInput > -xPanLimit)
            {
                transform.Translate(Vector3.right * horizontalInput, Space.World);
            }
            if (transform.position.y + verticalInput < yOrigin + yPanLimit && transform.position.y + verticalInput > yOrigin - yPanLimit)
            {
                transform.Translate(Vector3.up * verticalInput, Space.World);
            }
        }
    }
}
