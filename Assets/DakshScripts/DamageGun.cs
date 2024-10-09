using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGun : MonoBehaviour
{
    public float Damage;
    public float BulletRange;
    private Transform PlayerCamera;

    // Add an AudioSource field for the shooting sound
    [SerializeField] private AudioSource shootAudioSource; // AudioSource for shoot sound

    private void Start()
    {
        PlayerCamera = Camera.main.transform;
    }

    public void Shoot()
    {
        // Play shoot audio
        if (shootAudioSource != null)
        {
            Debug.Log("Playing Shoot Audio");
            shootAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Shoot Audio Source not assigned!");
        }

        Ray gunRay = new Ray(PlayerCamera.position, PlayerCamera.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Entity enemy))
            {
                enemy.Health -= Damage;
            }
        }
    }
}
