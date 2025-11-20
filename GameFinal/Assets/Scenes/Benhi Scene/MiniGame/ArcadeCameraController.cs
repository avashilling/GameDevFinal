using UnityEngine;

public class ArcadeCameraController : MonoBehaviour
{
    public Transform defaultPos;
    public Transform arcadePos;
    public Camera mainCamera;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    public void MoveToArcadeView()
    {
        if (arcadePos != null && mainCamera != null)
            mainCamera.transform.position = arcadePos.position;
    }

    public void MoveToDefaultView()
    {
        if (defaultPos != null && mainCamera != null)
            mainCamera.transform.position = defaultPos.position;
    }
}
