using UnityEngine;
using System;

public class FBHR_MenuChapterButtons : MonoBehaviour
{
    [Header("Chapter 2")]
    public GameObject priceButtonChapter2;
    public GameObject playButtonChapter2;
    public GameObject lockChapter2;

    [Header("Chapter 3")]
    public GameObject priceButtonChapter3;
    public GameObject playButtonChapter3;
    public GameObject lockChapter3;

    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        SetupChapter2();
        SetupChapter3();
    }

    private void SetupChapter2()
    {
        bool isActive = PlayerPrefs.GetString("FBHR_chapter2Active", "false") == "true";
        bool isPaid = PlayerPrefs.GetString("chapter2paid", "false") == "true";

        Debug.Log("Chapter 2 Active? " + isActive); // Add this


        if (isPaid && isActive)
        {
            playButtonChapter2.SetActive(false);
            lockChapter2.SetActive(false);
            priceButtonChapter2.SetActive(true);
        }
        else if (!isPaid && isActive)
        {
            priceButtonChapter2.SetActive(false);
            playButtonChapter2.SetActive(true);
            lockChapter2.SetActive(false);
        }
        else if (!isPaid || !isActive)
        {
            playButtonChapter2.SetActive(false);
            priceButtonChapter2.SetActive(false);
            lockChapter2.SetActive(true);
        }
    }

    private void SetupChapter3()
    {
        bool isActive = PlayerPrefs.GetString("FBHR_chapter3Active", "false") == "true";
        bool isPaid = PlayerPrefs.GetString("chapter3paid", "false") == "true";
        Debug.Log("Chapter 3 Active? " + isActive); // Add this

        if (isPaid && isActive)
        {
            playButtonChapter3.SetActive(true);
            priceButtonChapter3.SetActive(false);
            lockChapter3.SetActive(false);
        }
        else if (!isPaid && isActive)
        {
            playButtonChapter3.SetActive(false);
            priceButtonChapter3.SetActive(true); // Optional: show price button if you want
            lockChapter3.SetActive(false);
        }
        else if (!isPaid || !isActive)
        {
            playButtonChapter3.SetActive(false);
            priceButtonChapter3.SetActive(false);
            lockChapter3.SetActive(true);
        }
    }
}
