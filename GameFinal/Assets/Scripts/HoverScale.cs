using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleAmount = 1.05f;
    public float speed = 10f;

    private Vector3 originalScale;
    private bool hovering;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        Vector3 target = hovering ? originalScale * scaleAmount : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;

        // Tell the menu cursor it's hovering
        if (MenuCursorManager.Instance != null)
            MenuCursorManager.Instance.SetHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        // Tell the menu cursor it's NOT hovering
        if (MenuCursorManager.Instance != null)
            MenuCursorManager.Instance.SetHover(false);
    }
}
