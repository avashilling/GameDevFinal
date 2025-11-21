using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Header("Cursor Settings")]
    public Image cursorImage;
    public Sprite defaultCursorSprite;
    public Sprite hoverCursorSprite;
    public Vector2 cursorOffset;
    public float heldItemSize = 8f;
    public float defaultCursorSize = 4f;
    private bool isHovering = false;

    private Vector3 targetPos;

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
        // Ensure cursor image starts at the current real cursor position
        if (cursorImage != null)
        {
            cursorImage.transform.position = Input.mousePosition + (Vector3)cursorOffset;
        }

        // Hide the system cursor AFTER aligning the image
        Cursor.visible = false;

        SetCursorToDefault();

        // Subscribe to inventory change updates
        GameManager.Instance.OnInventoryChanged += UpdateHeldCursor;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnInventoryChanged -= UpdateHeldCursor;
    }

    private void Update()
    {
        targetPos = Input.mousePosition + (Vector3)cursorOffset;
        cursorImage.transform.position = Vector3.Lerp(cursorImage.transform.position, targetPos, Time.deltaTime * 25f);
    }

    public void UpdateHoverState(bool hovering)
    {
        if (GameManager.Instance.currentlyHeldItem != null)
        {
            Debug.Log("Item is being held! Skip cursor managing");
            Debug.Log("Item type is: " + GameManager.Instance.currentlyHeldItem.itemType);
            return;
        }

        if (hovering != isHovering)
        {
            isHovering = hovering;

            if (hovering)
                cursorImage.sprite = hoverCursorSprite;
            else
                SetCursorToDefault();
        }
    }



    public void SetCursorToDefault()
    {
        cursorImage.sprite = defaultCursorSprite;
        cursorImage.rectTransform.sizeDelta = new Vector2(defaultCursorSize, defaultCursorSize);
    }


    public void SetCursorToItem(Sprite itemSprite)
    {
        cursorImage.sprite = itemSprite;
        cursorImage.rectTransform.sizeDelta = new Vector2(heldItemSize, heldItemSize);
    }

    private void UpdateHeldCursor()
    {
        var held = GameManager.Instance.currentlyHeldItem;
        if (held != null && held.icon != null)
        {
            SetCursorToItem(held.icon);
        }
        else
        {
            SetCursorToDefault();
        }
    }
}
