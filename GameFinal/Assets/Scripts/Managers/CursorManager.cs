using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

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
    public float hoverCursorSize = 5f;

    [Header("Instruction Text")]
    public TextMeshProUGUI instructionText;

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
        GameManager.Instance.OnInventoryChanged += UpdateInstructionText;

        // Initialize instruction text
        UpdateInstructionText();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnInventoryChanged -= UpdateHeldCursor;
            GameManager.Instance.OnInventoryChanged -= UpdateInstructionText;
        }
    }

    private void Update()
    {
        targetPos = Input.mousePosition + (Vector3)cursorOffset;
        cursorImage.transform.position = Vector3.Lerp(cursorImage.transform.position, targetPos, Time.deltaTime * 25f);
    }

    public void UpdateHoverState(bool hovering)
    {
        // Only skip cursor changes if actually holding an item
        if (GameManager.Instance.currentlyHeldItem != null)
        {
            return;
        }

        if (hovering != isHovering)
        {
            isHovering = hovering;
            if (hovering)
            {
                cursorImage.sprite = hoverCursorSprite;
                cursorImage.rectTransform.sizeDelta = new Vector2(hoverCursorSize, hoverCursorSize);
            }
            else
            {
                SetCursorToDefault();
            }
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

    private void UpdateInstructionText()
    {
        if (instructionText == null)
            return;

        var held = GameManager.Instance.currentlyHeldItem;
        var inventory = GameManager.Instance.inventory;

        // If holding an item
        if (held != null)
        {
            instructionText.text = "Left click to use, right click to drop";
        }
        // If inventory has items but not holding any
        else if (inventory != null && inventory.Count > 0)
        {
            instructionText.text = "Left click an item to hold it";
        }
        // No items at all
        else
        {
            instructionText.text = "";
        }
    }
}