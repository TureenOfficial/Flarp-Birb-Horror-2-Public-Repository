using UnityEngine;
using GameJolt.API;
using System.Collections;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class RuntimeUtils : MonoBehaviour
{
    [SerializeField] GameObject startTransform;

    [SerializeField] string gameTitle = "FBH2";
    [SerializeField] string objective = "ESCAPE THE MAZE"; //TASK DISPLAYED IN PAUSE MENU

    [SerializeField] TimeManager timeManagerScript;
    [SerializeField] bool giveReward = true; //USED FOR IF A MAP GIVES REWARD SO THEY CANT BE USED FOR FARMING MONEY
    [SerializeField] float timesDetectedBonus = 500;
    [SerializeField] float moneyAmount = 500; //STARTING AMOUNT OF MONEY GIVEN IN EVERY CHAPTER

    [SerializeField] PostProcessVolume ppV;
    [SerializeField] MotionBlur motionBlur;
    [SerializeField] float fogDensity;
    [SerializeField] float fogDensityMultiplier;

    [SerializeField] GameObject playerMap;
    [SerializeField] GameObject playerFlashlight;
    [SerializeField] GameObject playerFlashlightSpotLight;
    [SerializeField] GameObject playerMapCanvasObject;
    [SerializeField] GameObject playerFlashlightCanvasObject;
    [SerializeField] GameObject playerCamCanvasObject;

    [SerializeField] float timeSurvived;
    [SerializeField] TMP_Text taskText;
    [SerializeField] TMP_Text timeSurvivedText;
    [SerializeField] TMP_Text collectedText;
    [SerializeField] TMP_Text statisticText;  
    [SerializeField] TMP_Text endingText;

    [SerializeField] float mapZoom;
    [SerializeField] float zoomSpeed = 0.25f;

    [SerializeField] ObjectDoor doorScript;
    [SerializeField] PlayerController playerControllerScript;

    //PUBLIC VARIABLES
    public bool photoMode;
    public bool mapMode;
    public bool FirstCollectibleEnablesCanvas;
    public bool isMapActive;
    public bool isFlashlight;
    public bool isScreenshot = true;
    public bool flashlightOn;
    public int chapterNumber;
    public string itemQuota;
    public string niceTime;
    public bool playerNotDefault = false;
    public int collectedItems;
    public int itemsNeeded;
    public string WinCondition;
    public GameObject MainCanvas;
    public GameObject DeathCanvas;
    public GameObject WinCanvas;
    public GameObject PauseCanvas;
    public GameObject MapCanvas;
    public GameObject canBeSeenEye;
    public AudioSource collectedAll_Items_AudioSource;
    public AudioSource ambienceAudioSource;
    public AudioSource flashlightAudioSource;
    public AudioClip[] ambienceClips;
    public AudioClip[] screamSoundClip;
    public FlarpBehaviour flarpBehaviourScript;
    public GameObject playerCameraObject;
    public GameObject mapCameraObject;
    public Camera mapCamera;
    public int collectablesUntilAwakening;

    //private float Chase_Timer = 0;

    [ContextMenu("UpdateItemQuota")]
    public void UpdateItemQuota()
    {
        collectedText.text = $"{itemQuota.ToUpper()}: {collectedItems}/{itemsNeeded}";
    }

    void Awake()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("fpsCap");
        if(PlayerPrefs.GetString("ogGraphics", "false") == "true")
        {
            ppV.enabled = false;
            RenderSettings.fogDensity = 0;
            fogDensityMultiplier = 0;
            fogDensity = 0;
        }
        else
        {
            ppV.enabled = true;
        }

        if(playerFlashlightCanvasObject == null) playerFlashlightCanvasObject = GameObject.Find("torchParent");
        if(playerMapCanvasObject == null) playerMapCanvasObject = GameObject.Find("mapParent");
        if(startTransform != null) this.gameObject.transform.position = startTransform.transform.position;
        string motionblurOn = PlayerPrefs.GetString("motionBlur", "true");

        if(motionblurOn == "true")
        {
            ppV.profile.TryGetSettings(out motionBlur);
            motionBlur.enabled.value = true;
        }
        else
        {
            ppV.profile.TryGetSettings(out motionBlur);
            motionBlur.enabled.value = false;
        }
    }
    public void Start()
    {
        if(taskText != null) taskText.text = $"OBJECTIVE: \n{objective}";
        if(isFlashlight)
        {
            flashlightOn = true;
        }
        switch(chapterNumber)
        {
            case 1:
            {
                PlayerPrefs.SetString("inheritedBehaviour", "Flarpy Warpy");
                break;
            }
            case 2:
            {
                PlayerPrefs.SetString("inheritedBehaviour", "Flarpy Warpia");
                break;  
            }
            case 3:
            {
                PlayerPrefs.SetString("inheritedBehaviour", "Flarpious Warpious");
                break;
            }
        }



        StartCoroutine(WaitForOverSummonCount());
        StartCoroutine(WaitForOverTotal());


        UpdateItemQuota();
        photoMode = false;
        
        collectedAll_Items_AudioSource.volume = 0.75f * PlayerPrefs.GetFloat("audioMultiplier");
        ambienceAudioSource.volume = 0.5f * PlayerPrefs.GetFloat("audioMultiplier");
        ambienceAudioSource.loop = true;
        switch(flarpBehaviourScript.behaviourToInherit)
        {
            case "Flarpy Warpy":
            {
                ambienceAudioSource.clip = ambienceClips[0];
                break;
            }
            case "Flarpy Warpia":
            {
                ambienceAudioSource.clip = ambienceClips[1];
                break;
            }
            case "Flarpious Warpious":
            {
                ambienceAudioSource.clip = ambienceClips[2];
                break;
            }
        }
        ambienceAudioSource.Play();

        collectedItems = 0;

        PauseCanvas.SetActive(false);
        DeathCanvas.SetActive(false);
        WinCanvas.SetActive(false);
        if(FirstCollectibleEnablesCanvas)
        {
            if(collectedItems > 0)
            {
                MainCanvas.SetActive(true);
            }
            else 
            {
               MainCanvas.SetActive(false);
            }
        }
    }
    public void OnDeath()
    {
        //playerControllerScript.canMove = false; Now Static (Unused)
        playerNotDefault = true;
        ambienceAudioSource.Pause();
        flarpBehaviourScript.chasingloopAudioSource.Pause();
        flarpBehaviourScript.flarpAudioSource.volume = 0.3f * PlayerPrefs.GetFloat("audioMultiplier");
        flarpBehaviourScript.flarpAudioSource.loop = false;
        switch(flarpBehaviourScript.behaviourToInherit)
        {
            case "Flarpy Warpy":
            {
                flarpBehaviourScript.flarpAudioSource.clip = screamSoundClip[0];
                break;
            }
            case "Flarpy Warpia":
            {
                flarpBehaviourScript.flarpAudioSource.clip = screamSoundClip[1];
                break;
            }
            case "Flarpious Warpious":
            {
                SceneManager.LoadScene("FBH_Remastered_ChapterSelect");
                break;
            }
        }
        flarpBehaviourScript.flarpAudioSource.Play();
        
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        Time.timeScale = 1f;
        MainCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
        DeathCanvas.SetActive(true);

        statisticText.text = $"TIME SURVIVED: {niceTime}\n{itemQuota.ToUpper()} COLLECTED: {collectedItems}/{itemsNeeded}";
    }
    public void Update()
    {
        if(playerFlashlight.gameObject != null) playerFlashlight.SetActive(isFlashlight);
        if(playerFlashlightSpotLight.gameObject != null) playerFlashlightSpotLight.SetActive(flashlightOn);
        playerMapCanvasObject.SetActive(isMapActive);
        playerFlashlightCanvasObject.SetActive(isFlashlight);
        playerCamCanvasObject.SetActive(isScreenshot);

        if(isFlashlight && flashlightOn && flarpBehaviourScript != null && flarpBehaviourScript.distanceToPlayer < flarpBehaviourScript.detectionRange && flarpBehaviourScript.sensesFlashlight && flarpBehaviourScript.gameObject.activeInHierarchy && !flarpBehaviourScript.isChasingPlayer)
        {
            canBeSeenEye.SetActive(true);
        }
        else
        {
            canBeSeenEye.SetActive(false);
        }

        if(playerControllerScript.pausedState)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }

        RenderSettings.fogDensity = fogDensity * fogDensityMultiplier;
        timeSurvived = (float)Math.Round(Time.timeSinceLevelLoad, 0);
        

        /*if(flarpBehaviourScript.isChasingPlayer)
        {
           Chase_Timer += Time.deltaTime;

            print($"{(float)Math.Round(Chase_Timer, 2)}");

            if((float)Math.Round(Chase_Timer, 2) >= 1)
         {
               moneyAmount+=10;
                Chase_Timer = 0;
             }
        }*/
        //IF FLARP CHASES YOU - MORE MONEY - MORE RISK (SCRAPPED)

        float reward = moneyAmount - (timeSurvived * 4);

        if(SceneManager.GetActiveScene().name == "B_DanDiamond")
        {
            if(reward <= 0)
            {
                OnDeath();
            }
        }
        else
        {
            if(reward <= 0 || !giveReward)
            {
                reward = 0;
            }
        }
        int minutes = Mathf.FloorToInt(timeSurvived / 60F);
        int seconds = Mathf.FloorToInt(timeSurvived - minutes * 60);
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        
        if(giveReward)
        {
            timeSurvivedText.text = $"TIME: {niceTime}\nREWARD: {reward}$";
        }
        else
        {
            timeSurvivedText.text = $"TIME: {niceTime}";
        }


        if(flarpBehaviourScript != null && flarpBehaviourScript.isChasingPlayer)
        {
            fogDensityMultiplier = 3;
            ambienceAudioSource.Pause();
        }
        else
        {
            fogDensityMultiplier = 1;
            if(!(playerNotDefault || PauseCanvas.activeInHierarchy || photoMode || mapMode || playerControllerScript.pausedState)) ambienceAudioSource.UnPause();
        }
        if(playerNotDefault || PauseCanvas.activeInHierarchy || photoMode || mapMode || playerControllerScript.pausedState)
        {
            if(ambienceAudioSource.isPlaying) ambienceAudioSource.Pause();
            Time.timeScale = 0f;
        }
        else
        {
            if(!ambienceAudioSource.isPlaying) ambienceAudioSource.UnPause();
            Time.timeScale = 1f;
        }
        if(!playerNotDefault && !mapMode && !photoMode && Input.GetKeyDown(KeyCode.Escape))
        {
            if(!PauseCanvas.activeInHierarchy)
            {
                flarpBehaviourScript.flarpAudioSource.Pause();
                flarpBehaviourScript.chasingloopAudioSource.Pause();
                playerControllerScript.pausedState = true;
                Time.timeScale = 0f;
                DeathCanvas.SetActive(false);
                PauseCanvas.SetActive(true);
            }
            else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                UnpauseGame();
            }
        }


        //SCREENSHOT MODE
        if(!playerNotDefault && Input.GetKeyDown(KeyCode.F1) && PauseCanvas.activeInHierarchy == false && !mapMode && isScreenshot)
        {
            if(photoMode == false)
            {
                flarpBehaviourScript.flarpAudioSource.Pause();
                flarpBehaviourScript.chasingloopAudioSource.Pause();
                photoMode = true;
                Time.timeScale = 0f;
                MainCanvas.SetActive(false);
            }
            else
            {
                flarpBehaviourScript.flarpAudioSource.UnPause();
                flarpBehaviourScript.chasingloopAudioSource.UnPause();
                photoMode = false;
                Time.timeScale = 1f;
                if(FirstCollectibleEnablesCanvas && collectedItems > 0)
                {
                    MainCanvas.SetActive(true);
                }
                if(!FirstCollectibleEnablesCanvas)
                {
                    MainCanvas.SetActive(true);
                }
            }
        }

        //FLASHLIGHT
        if(!playerNotDefault && flashlightOn && Input.GetKeyDown(KeyCode.F) && isFlashlight && !mapMode && !photoMode && !PauseCanvas.activeInHierarchy)
        {
            flashlightAudioSource.Play();
            flashlightOn = false;
        }
        else if(!playerNotDefault && !flashlightOn && Input.GetKeyDown(KeyCode.F) && isFlashlight && !mapMode && !photoMode && !PauseCanvas.activeInHierarchy)
        {
            flashlightAudioSource.Play();
            flashlightOn = true;
        }

        //MAP
        if(!playerNotDefault && Input.GetKeyDown(KeyCode.M) && PauseCanvas.activeInHierarchy == false && !photoMode && isMapActive) //forgot to add isMapActive
        {
            if(mapMode == false)
            {
                flarpBehaviourScript.flarpAudioSource.Pause();
                flarpBehaviourScript.chasingloopAudioSource.Pause();
                Time.timeScale = 0f;
                mapZoom = 50;
                mapCameraObject.SetActive(true);
                mapCamera.fieldOfView = 50;
                mapMode = true;
                MapCanvas.SetActive(true);
                MainCanvas.SetActive(false);
            }
            else   
            {
                flarpBehaviourScript.flarpAudioSource.UnPause();
                flarpBehaviourScript.chasingloopAudioSource.UnPause();
                Time.timeScale = 1f;
                mapCameraObject.SetActive(false);
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                mapMode = false;
                MapCanvas.SetActive(false);
                if(FirstCollectibleEnablesCanvas && collectedItems > 0 && !photoMode)
                {
                    MainCanvas.SetActive(true);
                }
                if(!FirstCollectibleEnablesCanvas && !photoMode)
                {
                    MainCanvas.SetActive(true);
                }
            }
        }


        if(Input.GetKey(KeyCode.E) && mapZoom < 120f && mapMode)
            {
                 mapZoom += zoomSpeed;
            }
        if(Input.GetKey(KeyCode.Q) && mapZoom > 20f && mapMode)
            {
                mapZoom -= zoomSpeed;
            }

        if(mapMode)
        {
            Math.Clamp(mapZoom, 20, 120);
            mapCamera.fieldOfView = mapZoom; 
        }   
    }
    public void UnpauseGame()
    {
        flarpBehaviourScript.flarpAudioSource.UnPause();
        flarpBehaviourScript.chasingloopAudioSource.UnPause();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        playerControllerScript.pausedState = false;
        if(!photoMode)
        {
            Time.timeScale = 1f;
        }
        PauseCanvas.SetActive(false);
    }

    //END

    void GiveMoney(float endingAmountMoney)
    {
        if(giveReward)
        {
            PlayerPrefs.SetFloat("moneyAmount", PlayerPrefs.GetFloat("moneyAmount", 0) + endingAmountMoney);
        }
        else
        {
            //haha no reward loser
        }
    }
    public void AddMoreMoney(float addMoney)
    {
        moneyAmount += addMoney;
    }
    public void InvokeAddToTotal()
    {
        collectedItems ++;
        UpdateItemQuota();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && flarpBehaviourScript.canKill)
        {
            OnDeath();
        }
    }
    public void Win()
    {
        flarpBehaviourScript.chasingloopAudioSource.Pause();
        flarpBehaviourScript.flarpAudioSource.Pause();
        //playerControllerScript.canMove = false; Now Static (Unused)
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        playerNotDefault = true;
        float endMoney;

        
        endMoney = moneyAmount - (float)(Math.Round(timeSurvived, 0, MidpointRounding.AwayFromZero) * 4);

        if(giveReward)
        {
            if(flarpBehaviourScript.timesDetected == 0)
            {
                endingText.text = $"TIME TAKEN: {niceTime}\nCOLLECTED ALL {itemQuota.ToUpper()}\nMONEY GAINED: {endMoney}$\nUNDETECTED BONUS: {timesDetectedBonus}$";
            }
            else
            {
                endingText.text = $"TIME TAKEN: {niceTime}\nCOLLECTED ALL {itemQuota.ToUpper()}\nMONEY GAINED: {endMoney}$";
            }
        }
        else
        {
            endingText.text = $"TIME TAKEN: {niceTime}\nCOLLECTED ALL {itemQuota.ToUpper()}";
        }
        WinCanvas.SetActive(true);

        if(endMoney <= 0)
        {
            endMoney = 0;
        }

        if(flarpBehaviourScript.timesDetected == 0)
        {
            endMoney += timesDetectedBonus;
        }

        GiveMoney(endMoney);

        if(gameTitle.ToUpper() == "FBHR")
        {
            timeManagerScript.CheckTime("");

            switch(chapterNumber)
            {
                case 1:
                {
                    if(GameJoltAPI.Instance.CurrentUser != null)
                    {
                        Trophies.TryUnlock(262865);
                    }

                    PlayerPrefs.SetString("bonusMap1_unlocked", "true");
                    PlayerPrefs.SetString("chapter2Active", "true");
                    break;
                }
                case 2:
                {
                    if(GameJoltAPI.Instance.CurrentUser != null)
                    {
                        Trophies.TryUnlock(262867);
                    }

                    PlayerPrefs.SetString("chapter3Active", "true");
                    break;
                }
                case 3:
                {
                    //activate something here
                    break;
                }
            }
        }
        else if(gameTitle.ToUpper() == "FBH2")
        {
            timeManagerScript.CheckTime("FBH2_");
            switch(chapterNumber)
            {
                case 1:
                {
                    PlayerPrefs.SetString("FBH2_chapter2Active", "true");
                    break;
                }
                case 2:
                {
                    PlayerPrefs.SetString("FBH2_chapter3Active", "true");
                    break;
                }
            }
        }
        else if(gameTitle.ToUpper() == "BONUS_")
        {
            timeManagerScript.CheckTime("Bonus_");
        }
        else if(gameTitle.ToUpper() == "TUTORIAL")
        {
            if(GameJoltAPI.Instance.CurrentUser != null)
            {
                Trophies.TryUnlock(262893);
            }
        }
    }

    public IEnumerator WaitForOverSummonCount()
    {
        yield return new WaitUntil(() =>collectedItems >= collectablesUntilAwakening);
        flarpBehaviourScript.Summon();
        MainCanvas.SetActive(true);
    }

    public IEnumerator WaitForOverTotal()
    {
        yield return new WaitUntil(() => collectedItems >= itemsNeeded);
        if(flarpBehaviourScript.HeKnowsWhenYouEatPoptarts)
        {
            flarpBehaviourScript.constantKnowledge = true;
        }
        collectedAll_Items_AudioSource.Play();
        if(WinCondition.ToLower() == "over_total")
        {
            Win();
        }
    }
}