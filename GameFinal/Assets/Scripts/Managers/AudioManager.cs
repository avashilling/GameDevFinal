using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;        // Background music
    public AudioSource sfxSource;          // General SFX
    public AudioSource footstepSource;     // Footstep SFX
    public AudioSource arcadeMusicSource;  // Arcade machine music (one-shot, non-looping)

    [Header("Clips")]
    public AudioClip backgroundMusic;

    [Header("Footsteps")]
    public AudioClip[] footstepClips;

    private float originalMusicVolume = 1.0f;
    private float duckedVolume = 0.2f;     // How low the background music gets during arcade music

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
    // BACKGROUND MUSIC
    // --------------------------------------------------------
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = originalMusicVolume;
        musicSource.Play();
    }

    // --------------------------------------------------------
    // PLAY ARCADE MUSIC (ducks background music)
    // --------------------------------------------------------
    public void PlayArcadeMusic(AudioClip clip)
    {
        if (clip == null || arcadeMusicSource == null)
            return;

        // Lower background music
        originalMusicVolume = musicSource.volume;
        musicSource.volume = duckedVolume;

        // Play arcade track
        arcadeMusicSource.clip = clip;
        arcadeMusicSource.loop = false;
        arcadeMusicSource.Play();

        // Start coroutine to restore volume
        StartCoroutine(RestoreMusicAfterArcade());
    }

    private System.Collections.IEnumerator RestoreMusicAfterArcade()
    {
        // Wait until arcade music is done
        while (arcadeMusicSource.isPlaying)
            yield return null;

        // Restore volume
        musicSource.volume = originalMusicVolume;
    }

    // --------------------------------------------------------
    // FOOTSTEPS
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
