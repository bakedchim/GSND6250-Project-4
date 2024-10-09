using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportation : MonoBehaviour
{
    public GameObject teleportationPlane; // Drag your cave floor object here in the inspector
    public float boundaryMargin = 1.0f; // Adjust this value for how much space to leave on each side

    private Vector3 planeSize; // To store the size of the teleportation plane

    void Start()
    {
        // Get the actual world-space size of the plane
        if (teleportationPlane != null)
        {
            Collider planeCollider = teleportationPlane.GetComponent<Collider>();

            if (planeCollider != null)
            {
                // Use the bounds to get the full size in world space
                planeSize = planeCollider.bounds.size;

                // Start the teleportation process
                InvokeRepeating(nameof(TeleportBoss), 0f, 3f);
            }
            else
            {
                Debug.LogError("Teleportation plane does not have a valid Collider.");
            }
        }
        else
        {
            Debug.LogError("Teleportation plane is not assigned!");
        }
    }

    void TeleportBoss()
    {
        // Calculate a random position within the full bounds of the plane, accounting for the boundary margin
        float randomX = Random.Range(-planeSize.x / 2 + boundaryMargin, planeSize.x / 2 - boundaryMargin);
        float randomZ = Random.Range(-planeSize.z / 2 + boundaryMargin, planeSize.z / 2 - boundaryMargin);

        // Maintain the current Y position of the boss
        Vector3 newPosition = teleportationPlane.transform.position + new Vector3(randomX, transform.position.y, randomZ);

        // Update the boss position
        transform.position = newPosition;
    }
}