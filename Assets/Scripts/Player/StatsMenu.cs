using TMPro;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    [Header("Game Statistics")]
    [SerializeField] private TMP_Text GameStatistics;
    [SerializeField] private TMP_Text MiscStatistics;

    void Update()
    {
        GameStatistics.text = $"Times played level: {PlayerPrefs.GetInt("timesPlayed", 0)}\nTimes beaten level: {PlayerPrefs.GetInt("timesBeaten", 0)}\nTimes died: {PlayerPrefs.GetInt("timesDied")}\nBalance: {PlayerPrefs.GetFloat("moneyAmount", 0)}$\nMoney spent: {PlayerPrefs.GetFloat("moneySpent", 0)}$";
        MiscStatistics.text = $"Times activated info: {PlayerPrefs.GetInt("pingTimes", 0)}";
    }
}
