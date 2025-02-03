using UnityEngine;

public class ProximityCameraSystem : MonoBehaviour
{
    [System.Serializable]
    public class CameraHotspot
    {
        public Transform hotspotPosition;  // Empty GameObject marking camera area
        public Camera linkedCamera;        // Camera to activate
        public float activationDistance = 5f;
        [HideInInspector] public bool isActive;
    }

    [Header("Cameras")]
    public Camera defaultCamera;
    public CameraHotspot[] cameraHotspots;

    [Header("Settings")]
    public float switchCooldown = 0.5f;    // Prevent rapid switching

    private Camera activeCamera;
    private float lastSwitchTime;

    void Start()
    {
        activeCamera = defaultCamera;
        foreach (CameraHotspot hotspot in cameraHotspots)
        {
            hotspot.linkedCamera.gameObject.SetActive(false);
        }
        defaultCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Time.time - lastSwitchTime < switchCooldown) return;

        CameraHotspot nearestHotspot = null;
        float closestDistance = Mathf.Infinity;

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