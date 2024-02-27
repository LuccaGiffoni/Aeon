using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockTime;
    [SerializeField] private bool useSeconds;

    private void Start()
    {
        if (clockTime == null)
        {
            clockTime = GetComponent<TextMeshProUGUI>();
        }

        InvokeRepeating(nameof(UpdateClock), 0f, 1f);
    }

    private void UpdateClock()
    {
        DateTime currentTime = DateTime.Now;

        if (useSeconds) clockTime.text = currentTime.ToString("HH:mm:ss");
        else clockTime.text = currentTime.ToString("HH:mm");
    }
}