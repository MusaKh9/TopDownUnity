using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnter : MonoBehaviour
{
    // Reference to the empty GameObject you want to teleport to
    public GameObject targetObject;

    // This method is called when another collider enters the trigger zone
    private void OnTriggerrEnter(Collider other)
    {
        // Check if the object entering the trigger is the player (or any specific object)
        if (other.CompareTag("Player")) // Make sure your player has the tag "Player"
        {
            // Teleport the player to the target object's position
            Teleport(other.gameObject);
        }
    }

    void Teleport(GameObject objectToTeleport)
    {
        if (targetObject != null)
        {
            // Set the object's position to the target object's position
            objectToTeleport.transform.position = targetObject.transform.position;
        }
    }
}