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
    public GameObject screenObject;

    [Header("Door To Unlock")]
    public DoorInteractable targetDoor;    // Reference to the door this keypad unlocks

    private string currentInput = "";
    private bool hasEnteredCode = false;

    private bool IsPowered()
    {
        return GameManager.Instance != null
            && GameManager.Instance.batteriesInserted && !hasEnteredCode;
    }

    private void Start()
    {
        if (screenObject != null)
            screenObject.SetActive(IsPowered());

        if (!IsPowered() && displayText != null)
            displayText.text = "";
    }

    private void EnableScreenIfPowered()
    {
        if (IsPowered() && screenObject != null && !screenObject.activeSelf)
            screenObject.SetActive(true);
    }

    public void AddDigit(string digit)
    {
        if (!IsPowered())
        {
            if (displayText != null)
                displayText.text = "";
            if (!hasEnteredCode) { HintManager.Instance.ShowHint("It does not seem to be powered on"); }
            else
            {
                HintManager.Instance.ShowHint("it beeped, maybe the door is open");
            }
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

            if (successSound != null)
                AudioManager.Instance.PlaySFX(successSound);

            GameManager.Instance.keypadCorrect = true;
            HintManager.Instance.ShowHint("It worked. I think the door just unlocked");

            OnCodeSuccess();
        }
        else
        {
            Debug.Log("Incorrect code.");

            if (failSound != null)
                AudioManager.Instance.PlaySFX(failSound);
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
        // Unlock the door
        if (targetDoor != null)
            targetDoor.unlocked = true;
        hasEnteredCode = true;
    }

    private void OnCodeFail()
    {
        // optional animations or effects
    }
}
