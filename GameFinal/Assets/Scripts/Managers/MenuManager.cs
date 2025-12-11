//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class MenuManager : MonoBehaviour
//{
//    public void LoadScene(string sceneName)
//    {
//        SceneManager.LoadScene(sceneName);

//    }
//}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Optional Audio")]
    public AudioClip enterSceneSound;   // Sound to play when the scene loads
    public float volume = 1f;           // Volume for the sound

    private static bool hasPlayedSound = false; // Ensures it plays only once

    private void Start()
    {
        // Play sound only once
        if (enterSceneSound != null && !hasPlayedSound)
        {
            AudioSource.PlayClipAtPoint(enterSceneSound, Camera.main.transform.position, volume);
            hasPlayedSound = true;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
