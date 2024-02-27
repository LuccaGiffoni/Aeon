using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    [Header("Input Field")]
    [SerializeField] private TMP_InputField shareableLink_InputField;

    [Header("User Feedback")]
    [SerializeField] private TextMeshProUGUI stats;

    private const string CampaignsFolder = "SavedCampaigns";
    private const string CampaignFileExtension = ".json";

    private Campaign ActiveCampaign { get; set; }

    public void AccessCampaignByShareableLink()
    {
        if(ActiveCampaign == null || ActiveCampaign.ShareableLink != shareableLink_InputField.text)
        {
            if (LoadCampaigns(shareableLink_InputField.text))
            {
                ActiveCampaign.AddClick();
            }
        }
        else if(ActiveCampaign != null && ActiveCampaign.ShareableLink == shareableLink_InputField.text)
        {
            var clickListFeedback = ActiveCampaign.AddClick();

            if(clickListFeedback == -1)
            {
                SaveCampaignChanges();
            }
            else
            {
                stats.text = $"Clicks: {ActiveCampaign.Clicks}\nLink Ativo: {ActiveCampaign.ActiveLink}\nLinks: {ActiveCampaign.Links.Count}";
            }
        }
    }

    private bool LoadCampaigns(string shareableLink)
    {
        if (string.IsNullOrEmpty(shareableLink))
        {
            Popup.Instance.SendErrorMessage("Por favor, digite um link válido.");
            return false;
        }
        else
        {
            Directory.CreateDirectory(CampaignsFolder);

            string[] files = Directory.GetFiles(CampaignsFolder, "*" + CampaignFileExtension);

            if (files.Length <= 0)
            {
                Popup.Instance.SendErrorMessage("Não há nenhuma campanha registrada. Volte para o menu e clique em 'Criar Campanha' para criar uma.");
                return false;
            }
            else
            {
                foreach (string filePath in files)
                {
                    string json = File.ReadAllText(filePath);

                    Campaign campaign = JsonUtility.FromJson<Campaign>(json);

                    if (campaign.ShareableLink == shareableLink && campaign.Status == true)
                    {
                        ActiveCampaign = campaign;
                        stats.text = $"Clicks: {ActiveCampaign.Clicks}\nLink Ativo: {ActiveCampaign.ActiveLink}\nLinks: {ActiveCampaign.Links.Count}";
                        Debug.Log(ActiveCampaign.Guid);
                        Popup.Instance.SendSuccessMessage($"Campanha {ActiveCampaign.Title} encontrada para o link!");
                        return true;
                    }
                    else if(campaign.ShareableLink == shareableLink && campaign.Status == false)
                    {
                        Popup.Instance.SendErrorMessage($"Essa campanha já foi encerrada.");
                        return false;
                    }
                }

                Popup.Instance.SendErrorMessage("Não há nenhuma campanha com esse link. Tente novamente!");
                return false;
            }
        }
    }

    private void SaveCampaignChanges()
    {
        string campaignFilePath = Path.Combine(CampaignsFolder, $"{ActiveCampaign.Guid}{CampaignFileExtension}");

        if (File.Exists(campaignFilePath))
        {
            string json = JsonUtility.ToJson(ActiveCampaign);
            File.WriteAllText(campaignFilePath, json);

            ActiveCampaign = null;
        }
        else
        {
            Popup.Instance.SendErrorMessage($"Não foi possível encontrar o caminho para a campanha: {campaignFilePath}");
        }
    }
}