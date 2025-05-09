using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mono.Cecil;
using TMPro;

[System.Serializable]
public class UnlockData
{
    public int[] IDsUnlocked;
    public int[] IDsPurchased;
}

public class CosmeticUI : MonoBehaviour
{
    public float Price;
    public string CosmeticName;
    public int CosmeticID;
    public GameObject[] L_P_Object;
    public TMP_Text ButtonPurchaseText;
    private bool isUnlocked;
    private bool isPurchased;


    void Start()
    {
        string jsonString = PlayerPrefs.GetString("EncryptedUnlockData");
        string decryptedJson = CryptoUtility.Decrypt(jsonString);

        UnlockData data = JsonUtility.FromJson<UnlockData>(decryptedJson);
        isUnlocked = data.IDsUnlocked.Contains(CosmeticID);
        isPurchased = data.IDsPurchased.Contains(CosmeticID);
    }

    void Update()
    {
        var Button = gameObject.GetComponent<Button>();
        if(isUnlocked && isPurchased)
        {
            L_P_Object[0].SetActive(false);
            L_P_Object[1].SetActive(false);
            Button.interactable = true;
        }
        else if(isUnlocked && !isPurchased)
        {
            L_P_Object[0].SetActive(false);
            L_P_Object[1].SetActive(true);
            ButtonPurchaseText.text = $"{Price}$";
            Button.interactable = false;
        }
        else if (!isPurchased && !isUnlocked)
        {
            L_P_Object[0].SetActive(true);
            L_P_Object[1].SetActive(false);
            Button.interactable = false;
        }
    }
    public void Press()
    {
        if (isUnlocked && !isPurchased)
        {
            Debug.Log($"You can now buy {CosmeticName}!");
            Purchase();
        }
        else if (isPurchased)
        {
            PlayerPrefs.SetInt("currentCosmetic", CosmeticID);
            PlayerPrefs.Save();
            Debug.Log($"{CosmeticName} selected!");
        }
        else
        {
            Debug.Log($"{CosmeticName} is locked.");
        }
    }

    public void Purchase()
    {
        if (isUnlocked && !isPurchased && PlayerPrefs.GetFloat("moneyAmount", 0) >= Price)
        {
            PlayerPrefs.SetFloat("moneyAmount",PlayerPrefs.GetFloat("moneyAmount", 0) - Price);

            string encryptedJson = PlayerPrefs.GetString("EncryptedUnlockData");
            string decryptedJson = CryptoUtility.Decrypt(encryptedJson);
            UnlockData data = JsonUtility.FromJson<UnlockData>(decryptedJson);

            var updatedList = data.IDsPurchased.ToList();
            if (!updatedList.Contains(CosmeticID))
            {
                updatedList.Add(CosmeticID);
                data.IDsPurchased = updatedList.ToArray();

                string updatedJson = JsonUtility.ToJson(data);
                string reEncrypted = CryptoUtility.Encrypt(updatedJson);

                PlayerPrefs.SetString("EncryptedUnlockData", reEncrypted);
                PlayerPrefs.Save();

                isPurchased = true;
            }
        }
    }
}
