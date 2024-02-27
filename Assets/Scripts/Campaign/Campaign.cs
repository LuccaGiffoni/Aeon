using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Campaign
{
    public string id;
    public Guid Guid
    {
        get
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var parsedId))
            {
                id = Guid.NewGuid().ToString();
            }

            return Guid.Parse(id);
        }
        set { id = value.ToString(); }
    }
    public string Title;
    public List<string> Links = new();
    public string ShareableLink;
    public string ActiveLink { get; set; }
    public int MaxClicks;
    public int Clicks;
    public bool Status;

    public Campaign(string title, List<string> links, int maxClicks)
    {
        Guid = Guid.NewGuid();
        Title = title;
        Links = links;
        MaxClicks = maxClicks;
        Clicks = 0;
        Status = true;
        index = 0;
        ActiveLink = Links[0];
    }

    private int index = 0;

    public int AddClick()
    {
        if(Clicks < MaxClicks)
        {
            Clicks++;

            if(ActiveLink != Links[index])
            {
                ActiveLink = Links[index];
            }
        }
        else
        {
            if (ActiveLink == Links[^1])
            {
                Popup.Instance.SendErrorMessage("O número máximo de cliques na campanha foi atingido e esse era o último link disponível. A campanha foi encerrada!");
                Status = false;
                Clicks = -1;
            }
            else
            {
                Popup.Instance.SendSuccessMessage($"Link de campanha {ActiveLink} encerrado. Redirecionando para o próximo link.");

                index++;
                ActiveLink = Links[index];
                ResetClicksCount();
            }
        }

        return Clicks;
    }

    public int ResetClicksCount()
    {
        Clicks = 0;
        return Clicks;
    }
}