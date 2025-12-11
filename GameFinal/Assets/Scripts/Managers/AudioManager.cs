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

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;       // Background music volume (0-1 range)
    [Range(0f, 1f)]
    public float arcadeVolume = 0.5f;      // Arcade music volume (0-1 range)

    private bool isArcadePlaying = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void Update()
    {
        // Apply volume changes in real-time from Inspector
        if (!isArcadePlaying)
        {
            musicSource.volume = musicVolume;
        }
        else
        {
            arcadeMusicSource.volume = arcadeVolume;
        }
    }

    // --------------------------------------------------------
    // BACKGROUND MUSIC
    // --------------------------------------------------------
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    // --------------------------------------------------------
    // ARCADE MUSIC MINIGAME (stops background, plays arcade)
    // Call ArcadeMinigameStart() when starting the minigame
    // Call ArcadeMinigameStop() when ending the minigame
    // --------------------------------------------------------
    public void ArcadeMinigameStart()
    {
        if (arcadeMusicSource == null || arcadeMusicSource.clip == null)
        {
            Debug.LogWarning("Arcade music source or clip is not assigned!");
            return;
        }

        isArcadePlaying = true;

        // Stop background music completely
        musicSource.Stop();

        // Play arcade track with current arcade volume
        arcadeMusicSource.loop = true;  // Loop during minigame
        arcadeMusicSource.volume = arcadeVolume;
        arcadeMusicSource.Play();
    }

    public void ArcadeMinigameStop()
    {
        isArcadePlaying = false;

        // Stop arcade music
        if (arcadeMusicSource.isPlaying)
        {
            arcadeMusicSource.Stop();
        }

        // Resume background music
        PlayBackgroundMusic();
    }

   
    private System.Collections.IEnumerator RestoreMusicAfterArcade()
    {
        while (arcadeMusicSource.isPlaying)
            yield return null;
        musicSource.volume = musicVolume;
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

//using UnityEngine;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance;

//    [Header("Audio Sources")]
//    public AudioSource musicSource;        // Background music
//    public AudioSource sfxSource;          // General SFX
//    public AudioSource footstepSource;     // Footstep SFX
//    public AudioSource arcadeMusicSource;  // Arcade machine music (one-shot, non-looping)

//    [Header("Clips")]
//    public AudioClip backgroundMusic;

//    [Header("Footsteps")]
//    public AudioClip[] footstepClips;

//    [Header("Settings")]
//    [Range(0.021f, 0.1f)]
//    public float musicVolume = 0.05f;   // Background music volume (Inspector)

//    private float originalMusicVolume;    // Stores current background music volume
//    private float duckedVolume;           // Volume while arcade music plays

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        Instance = this;
//    }

//    private void Start()
//    {
//        // Set original volume from Inspector
//        originalMusicVolume = musicVolume;
//        duckedVolume = originalMusicVolume * 0.4f; // reduce to 40% during arcade music
//        PlayBackgroundMusic();
//    }

//    // --------------------------------------------------------
//    // BACKGROUND MUSIC
//    // --------------------------------------------------------
//    public void PlayBackgroundMusic()
//    {
//        if (backgroundMusic == null) return;

//        musicSource.clip = backgroundMusic;
//        musicSource.loop = true;
//        musicSource.volume = originalMusicVolume;
//        musicSource.Play();
//    }

//    // --------------------------------------------------------
//    // PLAY ARCADE MUSIC (ducks background music)
//    // --------------------------------------------------------
//    public void PlayArcadeMusic(AudioClip clip)
//    {
//        if (clip == null || arcadeMusicSource == null)
//            return;

//        // Lower background music
//        musicSource.volume = duckedVolume;

//        // Play arcade track
//        arcadeMusicSource.clip = clip;
//        arcadeMusicSource.loop = false;
//        arcadeMusicSource.Play();

//        // Restore background music after arcade track
//        StartCoroutine(RestoreMusicAfterArcade());
//    }

//    private System.Collections.IEnumerator RestoreMusicAfterArcade()
//    {
//        while (arcadeMusicSource.isPlaying)
//            yield return null;

//        musicSource.volume = originalMusicVolume;
//    }

//    // --------------------------------------------------------
//    // FOOTSTEPS
//    // --------------------------------------------------------
//    public void PlayFootstep()
//    {
//        if (footstepClips == null || footstepClips.Length == 0) return;

//        int index = Random.Range(0, footstepClips.Length);
//        footstepSource.PlayOneShot(footstepClips[index]);
//    }

//    // --------------------------------------------------------
//    // GENERAL SFX
//    // --------------------------------------------------------
//    public void PlaySFX(AudioClip clip)
//    {
//        if (clip == null) return;
//        sfxSource.PlayOneShot(clip);
//    }
//}
