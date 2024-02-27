using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Link : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI linkText;
    [SerializeField] private Button button;

    private string link;

    public event System.Action<int> LinkDeleted;

    private void Start()
    {
        button.onClick.AddListener(DeleteLink);
    }

    public string SetText(string linkUrl)
    {
        link = linkUrl;
        linkText.text = link;

        return linkText.text;
    }

    private void DeleteLink()
    {
        LinkDeleted?.Invoke(gameObject.GetInstanceID());
        Destroy(gameObject);
    }
}