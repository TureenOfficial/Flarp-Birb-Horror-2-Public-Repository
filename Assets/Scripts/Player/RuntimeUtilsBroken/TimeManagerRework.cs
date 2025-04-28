using System;
using UnityEngine;
using GameJolt.API;

public class TimeManagerRework : MonoBehaviour
{
    public int chapterNumber = 1;
    public string niceTime = "02:30";
    public TimeDisplay timeDisplay;

    void Awake()
    {
        timeDisplay = gameObject.GetComponent<TimeDisplay>();
    }
    public void CheckTime(string gameTitle)
    {
        chapterNumber = GameDetail.Instance.GameChapterNumber;
        niceTime = timeDisplay.niceTime;

        string bestTime = PlayerPrefs.GetString($"{gameTitle}_chapter{chapterNumber}CompletionTime", "99:99");

        int bestTimeInSeconds = ConvertTimeToSeconds(bestTime);
        int niceTimeInSeconds = ConvertTimeToSeconds(niceTime);

        if(niceTimeInSeconds <= 60)
        {
            if(GameObject.FindObjectOfType<GameJoltAPI>() != null && GameJoltAPI.Instance.CurrentUser != null)
            {
                Trophies.TryUnlock(262866);
            }
        }
        if (niceTimeInSeconds < bestTimeInSeconds)
        {

            PlayerPrefs.SetString($"{gameTitle}_chapter{chapterNumber}CompletionTime", niceTime);

            PlayerPrefs.Save();

            Debug.Log($"New best time saved: {niceTime}");
        }
    }

    int ConvertTimeToSeconds(string time)
    {
        time = time.Trim();

        if (string.IsNullOrEmpty(time) || !time.Contains(":"))
        {
            Debug.LogError("Invalid time format. Expected MM:SS.");
            return int.MaxValue;
        }

        string[] timeParts = time.Split(':');
        
        if (timeParts.Length != 2)
        {
            Debug.LogError("Invalid time format. Expected MM:SS.");
            return int.MaxValue;
        }

        try
        {
            int minutes = int.Parse(timeParts[0]);
            int seconds = int.Parse(timeParts[1]);
            return (minutes * 60) + seconds;
        }
        catch (FormatException)
        {
            Debug.LogError("Time string contains invalid numeric values.");
            return int.MaxValue;
        }
    }
}