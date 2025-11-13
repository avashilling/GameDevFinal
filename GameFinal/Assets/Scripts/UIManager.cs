using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Navigation Arrows")]
    public Button leftArrow;
    public Button rightArrow;
    public Button backArrow;

    [Header("Inventory UI")]
    [Tooltip("Assign 4 transparent clickable Images positioned over your hotbar slots")]
    public Image[] slotImages; // Just the 3 overlay images
    private Button[] slotButtons; // auto-added runtime for click detection

    private void Start()
    {
        // Subscribe to GameManager events
        if (GameManager.Instance != null)
            GameManager.Instance.OnInventoryChanged += RefreshInventoryUI;

        // Dynamically add Button components to each image to detect clicks
        slotButtons = new Button[slotImages.Length];
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] == null) continue;

            Button btn = slotImages[i].gameObject.GetComponent<Button>();
            if (btn == null)
                btn = slotImages[i].gameObject.AddComponent<Button>();

            int index = i;
            btn.onClick.AddListener(() =>
            {
                if (index < GameManager.Instance.inventory.Count)
                    GameManager.Instance.SelectItem(index);
            });

            slotButtons[i] = btn;
        }

        RefreshInventoryUI();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnInventoryChanged -= RefreshInventoryUI;
    }

    private void Update()
    {
        var node = GameManager.Instance.currentNode;
        if (node == null) return;

        leftArrow.gameObject.SetActive(node.left != null);
        rightArrow.gameObject.SetActive(node.right != null);
        backArrow.gameObject.SetActive(node.back != null);
    }

    public void OnClickArrow(string direction)
    {
        if (GameManager.Instance.currentNode == null) return;

        var node = GameManager.Instance.currentNode;

        switch (direction)
        {
            case "left": node.left?.Arrive(); break;
            case "right": node.right?.Arrive(); break;
            case "back": node.back?.Arrive(); break;
        }
    }

    private void RefreshInventoryUI()
    {
        if (GameManager.Instance == null) return;

        List<InventoryItem> inv = GameManager.Instance.inventory;
        InventoryItem held = GameManager.Instance.currentlyHeldItem;

        for (int i = 0; i < slotImages.Length; i++)
        {
            Image img = slotImages[i];
            if (img == null) continue;

            // Set icon or clear it
            if (i < inv.Count)
            {
                img.sprite = inv[i].icon;
                img.color = Color.white;
            }
            else
            {
                img.sprite = null;
                img.color = new Color(1, 1, 1, 0); // transparent
            }

            // Optional: subtle highlight for selected slot
            var outline = img.GetComponent<Outline>();
            if (outline == null)
                outline = img.gameObject.AddComponent<Outline>();

            outline.enabled = (i < inv.Count && inv[i] == held);
            outline.effectColor = new Color(0.5f, 0.6f, 1f);
            outline.effectDistance = new Vector2(2f, -2f);
        }
    }
}
