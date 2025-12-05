using UnityEngine;

public class KeypadButton : Interactable, IInteractable
{
    public enum ButtonType
    {
        Digit,
        Delete,
        Enter
    }

    [Header("Keypad Config")]
    public KeypadController controller;
    public ButtonType type = ButtonType.Digit;
    public string digitValue = "0";  // Only used if type == Digit

    [Header("Visual Press")]
    public float pressDepth = 0.00017f;   // Small movement
    public float pressSpeed = 20f;

    private Vector3 originalPos;
    private bool animating;

    protected override void Awake()
    {
        base.Awake();
        originalPos = transform.localPosition;
    }

    public void Interact(InventoryItem heldItem)
    {
        PlayUseAudio();
        PressAnimation();

        if (controller == null)
        {
            Debug.LogWarning("KeypadButton has no controller assigned.");
            return;
        }

        switch (type)
        {
            case ButtonType.Digit:
                controller.AddDigit(digitValue);
                break;

            case ButtonType.Delete:
                controller.DeleteDigit();
                break;

            case ButtonType.Enter:
                controller.EnterCode();
                break;
        }
    }

    private void PressAnimation()
    {
        if (!animating)
            StartCoroutine(AnimatePress());
    }

    private System.Collections.IEnumerator AnimatePress()
    {
        animating = true;

        Vector3 pressedPos = originalPos + new Vector3(0f, 0f, -pressDepth);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * pressSpeed;
            transform.localPosition = Vector3.Lerp(originalPos, pressedPos, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * pressSpeed;
            transform.localPosition = Vector3.Lerp(pressedPos, originalPos, t);
            yield return null;
        }

        transform.localPosition = originalPos;
        animating = false;
    }
}
