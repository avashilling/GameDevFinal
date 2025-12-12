using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class TvLateGlitch : MonoBehaviour
{
    [Header("Video Players")]
    [SerializeField] private VideoPlayer[] videoPlayers;

    [Header("Video Settings")]
    [Range(0f, 5f)]
    [SerializeField] private float maxVideoOffset = 5f; // Max random offset for video start times

    [Header("Lights")]
    [SerializeField] private Light[] glitchLights;

    [Header("Light Glitch Settings")]
    [Range(0.01f, 0.5f)]
    [SerializeField] private float lightGlitchFrequency = 0.05f; // How often lights flicker (RAVE MODE!)
    [SerializeField] private float minLightDuration = 0.05f; // Minimum time light stays on
    [SerializeField] private float maxLightDuration = 0.3f; // Maximum time light stays on
    [Range(1, 10)]
    [SerializeField] private int simultaneousLights = 3; // How many lights can be on at once

    private bool isGlitching = false;

    private void Start()
    {
        // Make sure all lights start disabled
        foreach (Light light in glitchLights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }
    }

    /// <summary>
    /// Call this method from an external script to start the glitch effect
    /// </summary>
    public void StartGlitch()
    {
        if (isGlitching)
            return;

        isGlitching = true;

        // Start all videos with random offsets
        StartVideosWithOffset();

        // Start light glitching
        StartCoroutine(GlitchLights());
    }

    /// <summary>
    /// Call this method to stop the glitch effect
    /// </summary>
    public void StopGlitch()
    {
        isGlitching = false;

        // Stop all videos
        foreach (VideoPlayer vp in videoPlayers)
        {
            if (vp != null)
            {
                vp.Stop();
            }
        }

        // Turn off all lights
        foreach (Light light in glitchLights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
        }

        StopAllCoroutines();
    }

    private void StartVideosWithOffset()
    {
        foreach (VideoPlayer vp in videoPlayers)
        {
            if (vp != null && vp.clip != null)
            {
                // Set to loop
                vp.isLooping = true;

                // Start playing
                vp.Play();

                // Generate random offset between 0 and maxVideoOffset
                float randomOffset = Random.Range(0f, maxVideoOffset);

                // Wait until video is prepared, then seek to offset
                StartCoroutine(SeekVideoAfterPrepare(vp, randomOffset));
            }
        }
    }

    private IEnumerator SeekVideoAfterPrepare(VideoPlayer vp, float offset)
    {
        // Wait until the video is prepared
        while (!vp.isPrepared)
        {
            yield return null;
        }

        // Calculate frame to seek to based on offset
        // Make sure we don't exceed video length
        double targetTime = offset % vp.length;
        vp.time = targetTime;
    }

    private IEnumerator GlitchLights()
    {
        while (isGlitching)
        {
            // Wait for the frequency interval
            yield return new WaitForSeconds(lightGlitchFrequency);

            // Trigger multiple lights at once for RAVE MODE
            if (glitchLights.Length > 0)
            {
                int lightsToTrigger = Mathf.Min(simultaneousLights, glitchLights.Length);

                // Get random unique indices
                System.Collections.Generic.HashSet<int> selectedIndices = new System.Collections.Generic.HashSet<int>();
                while (selectedIndices.Count < lightsToTrigger)
                {
                    selectedIndices.Add(Random.Range(0, glitchLights.Length));
                }

                // Trigger each selected light
                foreach (int index in selectedIndices)
                {
                    Light selectedLight = glitchLights[index];

                    if (selectedLight != null)
                    {
                        // Start a coroutine for each light so they can have independent durations
                        StartCoroutine(FlickerLight(selectedLight));
                    }
                }
            }
        }
    }

    private IEnumerator FlickerLight(Light light)
    {
        // Turn on the light
        light.enabled = true;

        // Random duration for how long it stays on
        float duration = Random.Range(minLightDuration, maxLightDuration);

        // Wait for that duration
        yield return new WaitForSeconds(duration);

        // Turn off the light
        light.enabled = false;
    }
}