using UnityEngine;
using TMPro;

public class KeypadController : MonoBehaviour
{
    [Header("Keypad Settings")]
    public string correctCode = "12345";
    public int maxLength = 5;

    [Header("UI")]
    public TMP_Text displayText;

    [Header("Optional")]
    public AudioClip buttonBeep;
    public AudioClip successSound;
    public AudioClip failSound;

    [Header("Keypad Screen")]
    public GameObject screenObject;     // The screen that turns on when powered

    private string currentInput = "";

    private bool IsPowered()
    {
        return GameManager.Instance != null
            && GameManager.Instance.batteriesInserted;
    }

    private void Start()
    {
        // Turn screen OFF at start if unpowered
        if (screenObject != null)
            screenObject.SetActive(IsPowered());

        // Blank display until powered
        if (!IsPowered() && displayText != null)
            displayText.text = "";
    }

    private void EnableScreenIfPowered()
    {
        // If powered, ensure the screen becomes visible
        if (IsPowered() && screenObject != null && !screenObject.activeSelf)
            screenObject.SetActive(true);
    }

    public void AddDigit(string digit)
    {
        if (!IsPowered())
        {
            if (displayText != null)
                displayText.text = "";
            HintManager.Instance.ShowHint("It doesn't seem to be powered on");
            return;
        }

        EnableScreenIfPowered();

        if (currentInput.Length >= maxLength)
            return;

        currentInput += digit;
        UpdateDisplay();

        if (buttonBeep != null)
            AudioManager.Instance.PlaySFX(buttonBeep);
    }

    public void DeleteDigit()
    {
        if (!IsPowered())
        {
            if (displayText != null)
                displayText.text = "";
            return;
        }

        EnableScreenIfPowered();

        if (currentInput.Length == 0)
            return;

        currentInput = currentInput.Substring(0, currentInput.Length - 1);
        UpdateDisplay();

        if (buttonBeep != null)
            AudioManager.Instance.PlaySFX(buttonBeep);
    }

    public void EnterCode()
    {
        if (!IsPowered())
        {
            if (displayText != null)
                displayText.text = "";
            return;
        }

        EnableScreenIfPowered();

        if (currentInput == correctCode)
        {
            Debug.Log("Correct code! Unlocking.");

            if (successSound != null)
                AudioManager.Instance.PlaySFX(successSound);

            GameManager.Instance.keypadCorrect = true;
            HintManager.Instance.ShowHint("It worked! I think the door just unlocked");

            OnCodeSuccess();
        }
        else
        {
            Debug.Log("Incorrect code.");

            if (failSound != null)
                AudioManager.Instance.PlaySFX(failSound);
            HintManager.Instance.ShowHint("Hm..I better figure out what the code is");
            currentInput = "";
            UpdateDisplay();

            OnCodeFail();
        }
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = currentInput;
    }

    private void OnCodeSuccess()
    {
        Debug.Log("TODO: Success logic here.");
    }

    private void OnCodeFail()
    {
        // optional animations or effects
    }
}
