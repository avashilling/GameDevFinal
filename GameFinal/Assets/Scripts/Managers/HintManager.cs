using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [Header("UI Reference")]
    public TMP_Text hintText;   // or TMP_Text

    [Header("Settings")]
    public float hintDuration = 2.5f;

    private float timer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                hintText.text = "";
            }
        }
    }

    public void ShowHint(string message)
    {
        Debug.Log("ShowHint called with message: " + message);
        hintText.text = message;
        timer = hintDuration;
    }
}
