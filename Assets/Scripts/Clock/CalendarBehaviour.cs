using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CalendarBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI calendarText;

    private void Start()
    {
        DateTime currentDate = DateTime.Now;
        CultureInfo ptCulture = new CultureInfo("pt-BR");

        int dayOfMonth = currentDate.Day;
        string monthName = currentDate.ToString("MMMM", ptCulture);
        string dayOfWeek = currentDate.ToString("dddd", ptCulture);
        int year = currentDate.Year;

        calendarText.text = $"{dayOfWeek},\n{dayOfMonth} de {monthName} de {year}";
    }
}