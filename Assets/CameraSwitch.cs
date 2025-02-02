using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // Array to hold your cameras
    private int currentCameraIndex = 0; // Index to track the current active camera

    void Start()
    {
        // Ensure only the first camera is active at the start
        SwitchCamera(0);
    }

    void Update()
    {
        // Check for key presses to switch cameras
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCamera(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCamera(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCamera(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchCamera(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchCamera(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchCamera(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SwitchCamera(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SwitchCamera(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SwitchCamera(8);
        }
    }

    void SwitchCamera(int index)
    {
        // Check if the index is within the bounds of the cameras array
        if (index >= 0 && index < cameras.Length)
        {
            // Disable all cameras
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(false);
            }

            // Enable the selected camera
            cameras[index].gameObject.SetActive(true);
            currentCameraIndex = index;
        }
    }
}