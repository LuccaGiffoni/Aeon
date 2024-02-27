using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Windows.Input;

public class CampaignCreator : MonoBehaviour
{
    private const string CampaignsFolder = "SavedCampaigns";
    private const string CampaignFileExtension = ".json";

    [Header("InputFields")]
    [SerializeField] private TMP_InputField title_InputField;
    [SerializeField] private TMP_InputField clicks_InputField;
    [SerializeField] private TMP_InputField link_InputField;

    [Header("Link")]
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private Transform scrollviewContent;

    private readonly Dictionary<int, string> Links = new();
    public string MaxClicks { get; set; }
    public string Title { get; set; }
    public string ShareableLink { get; set; }

    public void SaveLinkToList()
    {
        var newLink = Instantiate(linkPrefab, scrollviewContent).GetComponent<Link>();
        Links.Add(newLink.gameObject.GetInstanceID(), newLink.SetText(link_InputField.text));

        newLink.LinkDeleted += OnLinkDeleted;
        link_InputField.text = "";
    }

    private void OnLinkDeleted(int linkId)
    {
        Links.Remove(linkId);
    }

    public void CreateCampaign()
    {
        if (string.IsNullOrEmpty(Title))
        {
            Popup.Instance.SendErrorMessage("O título da campanha não pode ser vazio.");
        }
        else
        {
            if (Convert.ToInt32(MaxClicks) <= 0)
            {
                Popup.Instance.SendErrorMessage("Não é possível salvar uma campanha com limite de cliques igual ou menor que zero.");
            }
            else
            {
                if (Links.Count <= 0)
                {
                    Popup.Instance.SendErrorMessage("Não é possível salvar uma campanha sem nenhum link cadastrado.");
                }
                else
                {
                    var newCampaign = new Campaign(Title, Links.Values.ToList(), Convert.ToInt32(MaxClicks));
                    newCampaign.ShareableLink = "https://" + Title.Replace(" ", "") + "-" + newCampaign.Guid + ".com";
                    ShareableLink = newCampaign.ShareableLink.ToString();
                    Popup.Instance.SendSuccessMessage($"Clique para copiar o link compartilhável da sua campanha:\n" + ShareableLink);
                    Popup.Instance.button.onClick.AddListener(CopyLinkToClipboard);

                    SaveCampaign(newCampaign);
                }
            }
        }
    }

    private void SaveCampaign(Campaign campaign)
    {
        if (!Directory.Exists(CampaignsFolder))
        {
            Directory.CreateDirectory(CampaignsFolder);
        }

        string json = JsonUtility.ToJson(campaign);

        string filePath = Path.Combine(CampaignsFolder, campaign.Guid.ToString() + CampaignFileExtension);
        File.WriteAllText(filePath, json);

        ClearUI();
    }

    private void ClearUI()
    {
        title_InputField.text = "";
        clicks_InputField.text = "";
        link_InputField.text = "";

        Title = "";
        MaxClicks = "";
        Links.Clear();

        foreach(Transform t in scrollviewContent)
        {
            Destroy(t.gameObject);
        }
    }

    private void CopyLinkToClipboard()
    {
        GUIUtility.systemCopyBuffer = ShareableLink;
    }
}