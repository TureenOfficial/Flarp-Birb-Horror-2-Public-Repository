using System.Diagnostics;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using GameJolt.API;
using System.Collections.Generic;

public class GameDetail : MonoBehaviour
{
    //Made in v0.4.0 to manage game details seperately due to over complicated RuntimeUtils

    public static GameDetail Instance { get; set; }


    [Header("Misc")]
    [SerializeField] public GameObject LoadTransitionObject;
    [SerializeField] bool LoadingTransition = true;
    [SerializeField] public AudioSource[] Sources;
    [SerializeField] public GameObject[] UIParents;
    [SerializeField] public GameObject FlarpBirb;

    private void Awake()
    {
        if(LoadingTransition)
        {
            LoadTransitionObject?.SetActive(true);
        }
        else
        {
            LoadTransitionObject?.SetActive(false);
        }

        Sources = FindObjectsOfType<AudioSource>();
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [Header("Win Screen")]
    [SerializeField] public Texture LevelCompleteImage;
    [SerializeField] public RawImage Image;
    [SerializeField] public TMP_Text Win_GameTitleText;
    [SerializeField] public TMP_Text End_MoneyText;
    [SerializeField] public List<string> Bonuses;
    [SerializeField] TMP_Text bonusText;

    [Header("Loss Screen")]
    [SerializeField] AudioSource LossAudioSource;
    [SerializeField] public TMP_Text[] TimeLossDetails;

    [Header("Game")]
    [SerializeField] public string GameTitle;
    [SerializeField] public int GameChapterNumber = 0;
    [SerializeField] public bool gameActive;
    [SerializeField] public string GameObjective;
    [SerializeField] public New_FlarpBehaviour FlarpBeh;

    [Header("Time")]
    [SerializeField] public float TimeInGame;
    [SerializeField] public bool TimeLimit = false;
    [SerializeField] public float TimeLimitLength;
    
    [Header("Player")]
    [SerializeField] public bool playerHasFlashlight;
    [SerializeField] public bool playerHasMap;
    [SerializeField] public bool playerHasCamera;
    [SerializeField] public bool playerHasStamina;

    [Header("Score")]
    [SerializeField] public int ScoreNeeded = 7;
    [SerializeField] public int ScoreCurrent = 0;
    [SerializeField] public string collectableName = "Poptarts";

    public void FlashlightActive(bool Activate)
    {
        playerHasFlashlight = Activate;
    }
    public void MapActive(bool Activate)
    {
        playerHasMap = Activate;
    }
    public void CameraActive(bool Activate)
    {
        playerHasCamera = Activate;
    }
    public void StaminaActive(bool Activate)
    {
        playerHasStamina = Activate;
    }
    public void SetAmbientColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        RenderSettings.ambientLight = new Color(r / 255f, g / 255f, b / 255f);
    }
    public void SetScoreNeeded(int score)
    {
        ScoreNeeded = score;
    }



    void Start()
    {
        PlayerPrefs.SetInt("timesPlayed", PlayerPrefs.GetInt("timesPlayed", 0) + 1);
        Bonuses.Clear();
        Time.timeScale = 1;
        UIParents[UIParents.Length -1].SetActive(false);
        UIParents[1].SetActive(false);
        gameActive = true;   
    }
    void Update()
    {
        TimeInGame += Time.deltaTime;
        if(ScoreCurrent > ScoreNeeded)
        {
            ScoreCurrent = ScoreNeeded;
        }   

        if(TimeLimit)
        {
            if(TimeInGame > TimeLimitLength)
            {
                Lose();
            }
        }


    }

    public void Lose()
    {
        UIParents[0].SetActive(false);
        UIParents[1].SetActive(true); //DEFINED AS LOSING CANVAS

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 0f;

        var timeDisplay = gameObject.GetComponent<TimeDisplay>();
        TimeLossDetails[0].text = $"TIME: {timeDisplay.niceTime}";
        TimeLossDetails[1].text = $"COLLECTED: {ScoreCurrent}/{ScoreNeeded} {collectableName}";

        var pauseMenu = gameObject.GetComponent<PauseMenu>();
        pauseMenu.PauseAudio();

        PlayerPrefs.SetInt("timesDied", PlayerPrefs.GetInt("timesDied", 0) + 1);

        LossAudioSource.enabled = true;
        LossAudioSource.volume = 0.2f * PlayerPrefs.GetFloat("audioMultiplier");
        gameActive = false;
    }

    void HandleFBHRProgression(int chapter)
    {
        switch (chapter)
        {
                case 1:
                    UnlockManager.AddNewUnlockedID(1);
                    PlayerPrefs.SetString("bonusMap1_unlocked", "true");
                    PlayerPrefs.SetString("FBHR_chapter2Active", "true"); // Unlock Chapter 2
                    if (GameJoltAPI.Instance?.gameObject != null) TryUnlockTrophy(262865, "Chapter 1 Completed");
                    break;

                case 2:
                UnlockManager.AddNewUnlockedID(2);

                if(TimeInGame < 80)
                {   
                    UnlockManager.AddNewUnlockedID(3);
                }
                    PlayerPrefs.SetString("FBHR_chapter3Active", "true"); // Unlock Chapter 3
                    UnityEngine.Debug.Log("Beat chapter 2, unlocking Chapter 3");
                    if (GameJoltAPI.Instance?.gameObject != null) TryUnlockTrophy(262867, "Chapter 2 Completed");
                    break;

                case 3:
                    PlayerPrefs.SetString("chapter1BoxPileActive", "false"); // Special condition
                    UnityEngine.Debug.Log("Tried to unlock trophy: CHAPTER 3 Trophy");
                    if (GameJoltAPI.Instance?.gameObject != null) TryUnlockTrophy(265643, "Chapter 3 Completed");
                    if (GameJoltAPI.Instance?.gameObject != null) TryUnlockTrophy(265644, "FBHR Completed");
                    break;
        }
    }

    void TryUnlockTrophy(int id, string description = "")
    {
        UnityEngine.Debug.Log($"[GameJolt] Tried to unlock trophy: {description} ({id})");
        Trophies.TryUnlock(id);
    }

    public void Win()
    {
        PlayerController.canMove = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        PlayerPrefs.SetInt("timesBeaten", PlayerPrefs.GetInt("timesBeaten", 0) + 1);
        Time.timeScale = 0f;
        var timeManagerRework = gameObject.GetComponent<TimeManagerRework>();

        timeManagerRework.CheckTime(GameTitle);

        UIParents[0].SetActive(false); //Default UI False
        UIParents[UIParents.Length -1].SetActive(true); //Win UI (Last) True

        if (Win_GameTitleText == null) Win_GameTitleText = UIParents[UIParents.Length -1].GetComponent<TMP_Text>();
        if(Image == null) Image = UIParents[UIParents.Length -1].GetComponent<RawImage>();

        Image.texture = LevelCompleteImage;
        Win_GameTitleText.text = $"{GameTitle} CHAPTER {GameChapterNumber}";


        switch (GameTitle)
        {
                case "FBHR":
                    HandleFBHRProgression(GameChapterNumber);
                    break;

                case "TUTORIAL":
                    if (GameJoltAPI.Instance?.gameObject != null) TryUnlockTrophy(262893, "Tutorial Completion");
                    break;
    
        }


        //MANAGE BONUSES

        // Stealth Bonus
       if (FlarpBeh?.timesDetected <= 0)
        {
            GrantBonus(350, "350$ | Finished Undetected", 264647);
        }

        // Speedrun Bonus
        if (Time.timeSinceLevelLoad <= 60)
        {
            GrantBonus(100, "100$ | Beat level in under 1 minute", 262866);
        }

        if(InfoPing.Instance.TimesActivated <= 0)
        {
            GrantBonus(200, "200$ | Never gained info");
        }

        void GrantBonus(int amount, string description, int trophyId = -1)
        {
            Bonuses.Add(description);
            MoneyManager.Instance.Reward += amount;


            if (GameJoltAPI.Instance?.gameObject != null && trophyId > 0)
            {
                Trophies.TryUnlock(trophyId);
            }
        }


        //


        //END

        var pauseMenu = gameObject.GetComponent<PauseMenu>();
        pauseMenu.PauseAudio();

        

        if(MoneyManager.Instance.MoneyEnabled)
        {
            foreach(string Item in Bonuses)
            {
                bonusText.text += $"\n+ {Item}";
            }
            End_MoneyText.text = $"{MoneyManager.Instance.Reward}$";
            MoneyManager.Instance.FinalizeAddition();
        }
        
        if (MoneyManager.Instance.MoneyEnabled == false || Bonuses.Count == 0)
        {
            bonusText.text += $"\nNO BONUSES";
        }

        gameActive = false; 
        PlayerPrefs.Save();
    }



}
