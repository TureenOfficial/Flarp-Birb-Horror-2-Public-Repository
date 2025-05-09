using UnityEngine;
using System.Linq;

public static class UnlockManager
{
    public static void AddNewUnlockedID(int newID)
    {
        string encryptedJson = PlayerPrefs.GetString("EncryptedUnlockData");

        if (string.IsNullOrEmpty(encryptedJson))
        {
            CreateDefaultUnlockData();
            encryptedJson = PlayerPrefs.GetString("EncryptedUnlockData");
        }

        string decryptedJson = CryptoUtility.Decrypt(encryptedJson);
        UnlockData unlockData = JsonUtility.FromJson<UnlockData>(decryptedJson);

        if (!unlockData.IDsUnlocked.Contains(newID))
        {
            unlockData.IDsUnlocked = unlockData.IDsUnlocked.Append(newID).ToArray();
            Debug.Log($"New unlocked ID added: {newID}");
        }

        string updatedJson = JsonUtility.ToJson(unlockData);
        string encryptedUpdatedJson = CryptoUtility.Encrypt(updatedJson);

        PlayerPrefs.SetString("EncryptedUnlockData", encryptedUpdatedJson);
        PlayerPrefs.Save();
    }

    private static void CreateDefaultUnlockData()
    {
        UnlockData defaultData = new UnlockData
        {
            IDsUnlocked = new int[] { 0 },
            IDsPurchased = new int[] { 0 }
        };

        string json = JsonUtility.ToJson(defaultData);
        string encrypted = CryptoUtility.Encrypt(json);

        PlayerPrefs.SetString("EncryptedUnlockData", encrypted);
        PlayerPrefs.Save();
    }
}