using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    [Header("Money")]
    [SerializeField] float MoneyStatic;
    [SerializeField] bool MoneyDrains;
    [SerializeField] public bool MoneyEnabled;
    [SerializeField] public float Reward;

    [Header("UI")]
    [SerializeField] public TMP_Text MoneyDisplay;

    [Header("Time")]
    float timeSurvived = 0;

    void Update()
    {
        timeSurvived = (float)Math.Round(Time.timeSinceLevelLoad, 0);

        if (!MoneyEnabled)
        {
            MoneyDisplay.gameObject.SetActive(false);
        }
        else
        {
            MoneyDisplay.gameObject.SetActive(true);

            if(MoneyDrains && Time.timeScale != 0f)
            {
                Reward = MoneyStatic - (timeSurvived * 4);
                MoneyDisplay.text = $"REWARD: {Reward}$";
            }

            if(Reward < 0)
            {
                MoneyDisplay.text = $"REWARD: 0$";
                Reward = 0;
            }
        }


    }

    public void AddTotal(float AdditionAmount)
    {
        MoneyStatic += AdditionAmount;
    }

    public void FinalizeAddition()
    {
        PlayerPrefs.SetFloat("moneyAmount", PlayerPrefs.GetFloat("moneyAmount", 0) + Reward);
    }
}
