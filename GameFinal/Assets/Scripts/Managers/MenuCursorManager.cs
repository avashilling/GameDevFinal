using UnityEngine;
using UnityEngine.UI;

public class MenuCursorManager : MonoBehaviour
{
    public static MenuCursorManager Instance { get; private set; }

    [Header("Cursor")]
    public Image cursorImage;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Vector2 cursorOffset = Vector2.zero;
    public float cursorSize = 4f;

    private Vector3 targetPos;
    private bool isHovering = false;

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
        // Make sure cursor image starts aligned
        cursorImage.transform.position = Input.mousePosition + (Vector3)cursorOffset;

        // Hide system cursor
        Cursor.visible = false;
        SetDefault();
    }

    private void Update()
    {
        targetPos = Input.mousePosition + (Vector3)cursorOffset;
        cursorImage.transform.position =
            Vector3.Lerp(cursorImage.transform.position, targetPos, Time.deltaTime * 25f);
    }

    // Called by UI hover receivers
    public void SetHover(bool hovering)
    {
        if (hovering == isHovering) return;

        isHovering = hovering;

        if (hovering)
            cursorImage.sprite = hoverSprite;
        else
            SetDefault();
    }

    private void SetDefault()
    {
        cursorImage.sprite = defaultSprite;
        cursorImage.rectTransform.sizeDelta = new Vector2(cursorSize, cursorSize);
    }
}
