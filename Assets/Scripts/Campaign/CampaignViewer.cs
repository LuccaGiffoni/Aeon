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
            TextMeshProUGUI buttonText = campaignButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = campaign.Title;

            Button button = campaignButton.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => DeleteCampaign(campaignButton, campaign));
        }
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