using UnityEngine;
using TMPro;

public class MoneyCalculation : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip[] lockSounds; // [0] = fail, [1] = success
    [SerializeField] private AudioSource lockSound;

    [Header("UI Elements")]
    [SerializeField] string GameTitle;
    [SerializeField] private GameObject purchaseObject;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private TMP_Text textTMP_to;
    [SerializeField] private TMP_Text textTMP_po;

    [Header("Settings")]
    [SerializeField] private float price = 0f;
    [SerializeField] private string chapterName = "chapter2";

    private void Start()
    {
        if (textTMP_to == null && textObject != null)
            textTMP_to = textObject.GetComponent<TMP_Text>();

        if (textTMP_po == null && purchaseObject != null)
            textTMP_po = purchaseObject.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        bool isPaid = PlayerPrefs.GetString($"{chapterName}paid") == "true";
        bool isActive = PlayerPrefs.GetString($"{GameTitle}_{chapterName}Active") == "true";

        if (isPaid && isActive)
        {
            ShowPlayButton();
        }
        else if (!isPaid && isActive)
        {
            ShowPurchaseButton();
        }
        else if (!isActive)
        {
            HidePurchaseButton();
        }
    }

    private void ShowPlayButton()
    {
        if (textTMP_to != null)
            textTMP_to.text = "PLAY";

        if (textObject != null)
            textObject.SetActive(true);

        if (purchaseObject != null)
            purchaseObject.SetActive(false);

        if(lockObject != null)
            lockObject.SetActive(false);
    }

    private void ShowPurchaseButton()
    {
        if (textTMP_po != null)
            textTMP_po.text = price > 0 ? $"{price}$" : "FREE";

        if (textObject != null)
            textObject.SetActive(false);

        if (purchaseObject != null)
            purchaseObject.SetActive(true);

        if(lockObject != null)
            lockObject.SetActive(false);
    }

    private void HidePurchaseButton()
    {
        if (textObject != null)
            textObject.SetActive(false);

        if (purchaseObject != null)
            purchaseObject.SetActive(false);

        if(lockObject != null)
            lockObject.SetActive(true);
    }

    public void AttemptPurchase(string chapterPurchased)
    {
        float currentMoney = PlayerPrefs.GetFloat("moneyAmount", 0);

        if (currentMoney >= price)
        {
            PlayerPrefs.SetFloat("moneyAmount", currentMoney - price);
            PlayerPrefs.SetString($"{chapterPurchased}paid", "true");

            ShowPlayButton();

            if (lockSound != null && lockSounds.Length > 1)
                lockSound.PlayOneShot(lockSounds[1]); // success
        }
        else
        {
            if (lockSound != null && lockSounds.Length > 0)
                lockSound.PlayOneShot(lockSounds[0]); // fail
        }
    }
}
