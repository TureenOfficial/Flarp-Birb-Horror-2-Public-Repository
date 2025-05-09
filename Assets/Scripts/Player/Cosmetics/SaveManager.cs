using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static UnlockData UnlockState;

    void Awake()
    {
        LoadOrCreateUnlockData();
    }

    public void LoadOrCreateUnlockData()
    {
        string encrypted = PlayerPrefs.GetString("EncryptedUnlockData");

        if (string.IsNullOrEmpty(encrypted))
        {
            Debug.Log("No saved unlock data. Creating default...");

            UnlockData defaultData = new UnlockData
            {
                IDsUnlocked = new int[] { 0 },
                IDsPurchased = new int[] { }
            };

            string json = JsonUtility.ToJson(defaultData);
            encrypted = CryptoUtility.Encrypt(json);
            PlayerPrefs.SetString("EncryptedUnlockData", encrypted);
            PlayerPrefs.Save();
        }

        string decryptedJson = CryptoUtility.Decrypt(encrypted);
        UnlockState = JsonUtility.FromJson<UnlockData>(decryptedJson);
    }
}