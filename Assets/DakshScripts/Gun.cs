using UnityEngine.Events;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public float FireCooldown;
    public bool automatic;
    public Light gunLight;         // Reference to the spotlight
    public float lightDuration = 0.1f; // How long the light stays on

    private float CurrentCooldown;
    private float lightTimer;      // Timer to control the light duration

    void Start()
    {
        CurrentCooldown = FireCooldown;
        if (gunLight != null)
        {
            gunLight.enabled = false; // Start with the light off
        }
    }

    void Update()
    {
        // Handle automatic vs manual fire
        if (automatic)
        {
            if (Input.GetMouseButton(0) && CurrentCooldown <= 0f)
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && CurrentCooldown <= 0f)
            {
                Shoot();
            }
        }

        // Decrease the cooldown
        CurrentCooldown -= Time.deltaTime;

        // Handle light duration timer
        if (gunLight != null && gunLight.enabled)
        {
            lightTimer -= Time.deltaTime;
            if (lightTimer <= 0f)
            {
                gunLight.enabled = false; // Turn off the light after the duration
            }
        }
    }

    private void Shoot()
    {
        OnGunShoot?.Invoke();
        CurrentCooldown = FireCooldown;

        // Turn on the spotlight
        if (gunLight != null)
        {
            gunLight.enabled = true;
            lightTimer = lightDuration; // Reset the light timer
        }
    }
}
