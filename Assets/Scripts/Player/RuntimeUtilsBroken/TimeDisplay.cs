using System;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    public GameObject MoneyTextOff;
    public TMP_Text TextDisplay;
    public TMP_Text FinalTimeDisplay;
    public string niceTime;
    public float timeSurvived;

    void Update()
    {
        float timeSurvived = (float)Math.Round(Time.timeSinceLevelLoad, 0);
        int minutes = Mathf.FloorToInt(timeSurvived / 60F);
        int seconds = Mathf.FloorToInt(timeSurvived - minutes * 60);
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if(GameDetail.Instance.TimeLimit)
        {
            TextDisplay.color = new Color(1f, 0.2f, 0.25f);
            TextDisplay.text = $"TIME: {GameDetail.Instance.TimeLimitLength - timeSurvived}";
        }
        else
        {
            TextDisplay.color = new Color(1, 1, 1);
            TextDisplay.text = $"TIME: {niceTime}";
        }

        if(MoneyManager.Instance.MoneyEnabled)
        {
            TextDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -25, 0);
            MoneyTextOff.SetActive(false);
        }
        else
        {
            MoneyTextOff.SetActive(true);
            TextDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 5, 0);
        }

        FinalTimeDisplay.text = $"TIME: {niceTime}";
    }
}
