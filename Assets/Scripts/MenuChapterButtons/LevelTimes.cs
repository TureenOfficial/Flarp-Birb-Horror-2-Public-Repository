using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimes : MonoBehaviour
{
    public TMP_Text l1_record;
    public TMP_Text l2_record;
    public TMP_Text l3_record;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "FBH_Remastered_ChapterSelect")
        {
            LoadMainTimes();
        }
        else if (sceneName == "MainMenu")
        {
            LoadBonusTimes();
        }
    }

    void LoadMainTimes()
    {
        l1_record.gameObject.SetActive(true);

        // Chapter 1
        DisplayTime("FBHR_chapter1CompletionTime", l1_record);

        // Chapter 2
        if (PlayerPrefs.GetString("FBHR_chapter2Active") == "true" && PlayerPrefs.GetString("chapter2paid") == "true")
        {
            l2_record.gameObject.SetActive(true);
            DisplayTime("FBHR_chapter2CompletionTime", l2_record, true);
        }
        else
        {
            l2_record.gameObject.SetActive(false);
        }

        // Chapter 3
        if (PlayerPrefs.GetString("FBHR_chapter3Active") == "true" && PlayerPrefs.GetString("chapter3paid") == "true")
        {
            l3_record.gameObject.SetActive(true);
            DisplayTime("FBHR_chapter3CompletionTime", l3_record);
        }
        else
        {
            l3_record.gameObject.SetActive(false);
        }
    }

    void LoadBonusTimes()
    {
        if (PlayerPrefs.GetString("bonusMap1_unlocked") == "true")
        {
            l1_record.gameObject.SetActive(true);
            DisplayTime("BONUS_chapter1CompletionTime", l1_record);
        }
        else
        {
            l1_record.gameObject.SetActive(false);
        }
    }

    void DisplayTime(string key, TMP_Text textComponent, bool multiline = false)
    {
        string time = PlayerPrefs.GetString(key);
        bool noRecord = string.IsNullOrEmpty(time) || time == "99:99";

        string prefix = multiline ? "BEST TIME:\n" : "BEST TIME: ";
        textComponent.text = noRecord ? $"{prefix}NO RECORD" : $"{prefix}{time}";
    }
}
