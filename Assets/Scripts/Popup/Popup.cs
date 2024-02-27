using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class Popup : MonoBehaviour
{
    public static Popup Instance;

    [Header("Sprites")]
    [SerializeField] private Sprite successBackground;
    [SerializeField] private Sprite failureBackground;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Image image;
    [SerializeField] public Button button;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

        gameObject.SetActive(false);          
    }

    public void SendErrorMessage(string text)
    {
        image.sprite = failureBackground;
        gameObject.SetActive(true);
        message.text = text;
    }

    public void SendSuccessMessage(string text)
    {
        image.sprite = successBackground;
        gameObject.SetActive(true);
        message.text = text;
    }
}