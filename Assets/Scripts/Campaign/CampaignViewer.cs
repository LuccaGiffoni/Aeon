using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignViewer : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform campaignListContent;
    [SerializeField] private GameObject campaignButtonPrefab;

    [Header("Sprites")]
    [SerializeField] private Sprite activeStatus;
    [SerializeField] private Sprite finishedStatus;

    private List<Campaign> allCampaigns;

    private void Start()
    {
        LoadAllCampaigns();
        DisplayCampaigns();
    }

    private void LoadAllCampaigns()
    {
        allCampaigns = new List<Campaign>();

        string[] files = Directory.GetFiles("SavedCampaigns", "*.json");

        if(files.Length < 1)
        {
            Popup.Instance.SendErrorMessage("Não existe nenhuma campanha cadastrada.");
        }
        else
        {
            foreach (string filePath in files)
            {
                string json = File.ReadAllText(filePath);
                Campaign campaign = JsonUtility.FromJson<Campaign>(json);
                allCampaigns.Add(campaign);
            }
        }
    }

    private void DisplayCampaigns()
    {
        foreach (Transform child in campaignListContent)
        {
            Destroy(child.gameObject);
        }
        foreach (Campaign campaign in allCampaigns)
        {
            GameObject campaignButton = Instantiate(campaignButtonPrefab, campaignListContent);

            foreach(var bg in campaignButton.GetComponentsInChildren<Image>())
            {
                if (bg.CompareTag("Status"))
                {
                    if (campaign.Status) bg.sprite = activeStatus;
                    else bg.sprite = finishedStatus;
                }
            }

            foreach(var tmp in campaignButton.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (tmp.CompareTag("Title"))
                {
                    tmp.text = campaign.Title;
                }
                else if (tmp.CompareTag("Status"))
                {
                    if (campaign.Status)
                    {
                        tmp.text = "Ativa";
                        tmp.color = Color.black;
                    }
                    else
                    {
                        tmp.text = "Encerrada";
                        tmp.color = Color.white;
                    }
                }
            }

            foreach (var btn in campaignButton.GetComponentsInChildren<Button>())
            {
                if (btn.CompareTag("Delete"))
                {
                    btn.onClick.AddListener(() => DeleteCampaign(campaignButton, campaign));
                }
                else if (btn.CompareTag("Copy"))
                {
                    btn.onClick.AddListener(() => CopyShareableLinkToClipboard(campaign));
                }
            }
        }
    }

    private void CopyShareableLinkToClipboard(Campaign campaign)
    {
        GUIUtility.systemCopyBuffer = campaign.ShareableLink;
        Popup.Instance.SendSuccessMessage("Link compartilhável da campanha copiado com sucesso!");
    }

    private void DeleteCampaign(GameObject campaignButton, Campaign campaign)
    {
        string filePath = Path.Combine("SavedCampaigns", campaign.id + ".json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Destroy(campaignButton);

            LoadAllCampaigns();
            DisplayCampaigns();

            Popup.Instance.SendSuccessMessage($"Campanha {campaign.Title} excluída com sucesso.");
        }
        else
        {
            Popup.Instance.SendSuccessMessage($"Não foi possível encontrar o caminho para a campanha com ID {campaign.id}.");
        }
    }
}