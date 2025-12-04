using UnityEngine;
using UnityEngine.Video;

public class VideoScreen : MonoBehaviour
{
    private VideoPlayer vp;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
    }

    public void PlayVideo()
    {
        if (vp != null)
            vp.Play();
    }
}
