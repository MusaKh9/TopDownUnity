using UnityEngine;
//Written By Musa Khokhar
public class CameraSwitch : MonoBehaviour
{
    [System.Serializable] //this allows the array to accessed in the inspector
    public class CameraHotspot
    {
        public Transform hotspotPosition;  // Empty GameObject marking camera area
        public Camera linkedCamera;        // Camera to activate
        public float activationDistance = 5f; //float for distance to activate 
        [HideInInspector] public bool isActive; //this is not shown in inspector but is true or false if the mini game is active 
    }

    public Camera defaultCamera; //reference to camera
    public CameraHotspot[] cameraHotspots; //reference for array of camera activation objects

    public float switchCooldown = 0.5f;    // Prevent rapid switching so it has a cooldown to how fast cameras switch

    private Camera activeCamera; //reference to the active camera
    private float lastSwitchTime; //float to store the last switch time


    void Start()
    {
        //this sets default camera active and hides linked cameras
        activeCamera = defaultCamera;
        foreach (CameraHotspot hotspot in cameraHotspots)
        {
            hotspot.linkedCamera.gameObject.SetActive(false);
        }
        defaultCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        //prevents switch if enough time hasn't passed
        if (Time.time - lastSwitchTime < switchCooldown) return;

        CameraHotspot nearestHotspot = null; //declares a variable for the nearest camera activation point
        float closestDistance = Mathf.Infinity; //stores the closet distance to infinity initally

        // Find closest valid hotspot
        foreach (CameraHotspot hotspot in cameraHotspots)
        {
            float distance = Vector3.Distance(transform.position, hotspot.hotspotPosition.position);

            if (distance < hotspot.activationDistance && distance < closestDistance)
            {
                closestDistance = distance;
                nearestHotspot = hotspot;
            }
        }

        // Switch camera only if entering new hotspot
        if (nearestHotspot != null && activeCamera != nearestHotspot.linkedCamera)
        {
            SwitchCamera(nearestHotspot.linkedCamera);
        }
    }


    void SwitchCamera(Camera newCamera)
    {
        // Disable all cameras
        defaultCamera.gameObject.SetActive(false);
        foreach (CameraHotspot hotspot in cameraHotspots)
        {
            hotspot.linkedCamera.gameObject.SetActive(false);
        }

        // Enable new camera
        newCamera.gameObject.SetActive(true);
        activeCamera = newCamera;
        lastSwitchTime = Time.time;

        // Update camera tags for click detection
        newCamera.tag = "MainCamera";
        defaultCamera.tag = "Untagged";
    }
}