using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text moneyDisplayText;
    void Update()
    {
        moneyDisplayText.text = $"MONEY: {PlayerPrefs.GetFloat("moneyAmount")}$";
    }
}
