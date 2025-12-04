using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;       // Background music
    public AudioSource sfxSource;         // General sound effects
    public AudioSource footstepSource;    // Footstep one-shots

    [Header("Clips")]
    public AudioClip backgroundMusic;

    [Header("Footsteps")]
    public AudioClip[] footstepClips;     // Randomized footstep SFX pool

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    // --------------------------------------------------------
    // MUSIC
    // --------------------------------------------------------
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // --------------------------------------------------------
    // FOOTSTEPS (one-shot random)
    // --------------------------------------------------------
    public void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        footstepSource.PlayOneShot(footstepClips[index]);
    }

    // --------------------------------------------------------
    // GENERAL SFX
    // --------------------------------------------------------
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
