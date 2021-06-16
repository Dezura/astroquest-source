using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerAndScore : MonoBehaviour
{
    public TimeSpan currentTime;
    public float currentScore = 0f;
    
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        UpdateParams();
        UpdateDisplays();
    }

    public void UpdateParams()
    {
        currentTime = currentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
    }

    public void UpdateDisplays()
    {
        timerText.text = string.Format("{0:00}:{1:00}", currentTime.Minutes, currentTime.Seconds);
        scoreText.text = ("Score: " + (int) currentScore);
    }

    public void AddScore(float amount)
    {
        currentScore += amount;
        UpdateDisplays();
    }
}
