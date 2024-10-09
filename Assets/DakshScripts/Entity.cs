using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] AudioSource natureAudioSource;  // Assign the nature audio source in the inspector
    [SerializeField] AudioSource bossBattleAudioSource;  // Assign the boss battle audio source in the inspector
    [SerializeField] private float StartingHealth;
    private float health;

    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            Debug.Log(health);
            if (health <= 0f)
            {
                HandleDestruction();
            }
        }
    }

    // Public AudioSource fields for destruction and periodic sounds
    [SerializeField] private AudioSource periodicAudioSource;  // AudioSource for periodic sound
    [SerializeField] private AudioSource destructionAudioSource;  // AudioSource for destruction sound

    // Reference to the cube wall (cave entrance)
    [SerializeField] private GameObject caveEntrance;  // Assign the cube wall (cave entrance) in the inspector

    private Coroutine periodicAudioCoroutine;  // Reference to the coroutine

    void Start()
    {
        Health = StartingHealth;

        // Start playing periodic audio
        periodicAudioCoroutine = StartCoroutine(PlayPeriodicAudio());
    }

    private void HandleDestruction()
    {
        // Stop the periodic audio coroutine
        if (periodicAudioCoroutine != null)
        {
            StopCoroutine(periodicAudioCoroutine);
            periodicAudioCoroutine = null;  // Clear the reference
        }

        // Play destruction audio before destroying
        if (destructionAudioSource != null)
        {
            Debug.Log("Playing Destruction Audio");
            destructionAudioSource.Play();

            // Start moving the cave entrance up over 3 seconds
            if (caveEntrance != null)
            {
                Debug.Log("Opening Cave Entrance Slowly");
                StartCoroutine(MoveCaveEntrance());
                // Stop the boss battle audio source
                if (bossBattleAudioSource != null)
                {
                    StartCoroutine(FadeOutMusic(bossBattleAudioSource, 3f));  // Fade out over 3 seconds
                }
                // Play the nature audio source
                if (natureAudioSource != null)
                {
                    natureAudioSource.Play();
                }
            }
            else
            {
                Debug.LogWarning("Cave Entrance object not assigned!");
            }

            // Delay the destruction to allow the sound to play
            Destroy(gameObject, destructionAudioSource.clip.length + 1f);  // Add 1 second delay
        }
        else
        {
            Debug.LogWarning("Destruction Audio Source not assigned!");
            Destroy(gameObject, 1f);  // Immediately destroy if no destruction sound is assigned, with a 2 seconds delay
        }
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // Coroutine to move the cave entrance over 3 seconds
    private IEnumerator MoveCaveEntrance()
    {
        Vector3 startPosition = caveEntrance.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0f, 12f, 0f);  // Move up by 12 units
        float duration = 3f;  // Time to move (in seconds)
        float elapsed = 0f;

        // Gradually move the cave entrance up over 3 seconds
        while (elapsed < duration)
        {
            caveEntrance.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;  // Update elapsed time
            yield return null;  // Wait for the next frame
        }

        // Ensure the final position is exactly the target position
        caveEntrance.transform.position = targetPosition;
    }

    private IEnumerator PlayPeriodicAudio()
    {
        while (true)  // Loop indefinitely
        {
            if (periodicAudioSource != null)
            {
                Debug.Log("Playing Periodic Audio");
                periodicAudioSource.Play();
            }
            else
            {
                Debug.LogWarning("Periodic Audio Source not assigned!");
            }

            // Wait for 3 seconds before playing again
            yield return new WaitForSeconds(3f);
        }
    }

    void Update()
    {
        // Optional: No need for this if destruction is handled in Health property
    }
}
