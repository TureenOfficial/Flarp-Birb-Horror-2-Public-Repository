using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{

    //TEXT
    [SerializeField] TMP_Text fpsCapText;
    [SerializeField] TMP_Text curSizeText;
    [SerializeField] TMP_Text textCameraSens;
    [SerializeField] TMP_Text textAudioMult;

    //END
    [SerializeField] Animation animationClip_Delete;
    [SerializeField] UnityEngine.UI.Slider camsensSlider;
    [SerializeField] UnityEngine.UI.Slider audioVolumeSlider;
    [SerializeField] UnityEngine.UI.Slider curSizeSlider;
    [SerializeField] UnityEngine.UI.Toggle fpsToggle;
    [SerializeField] UnityEngine.UI.Toggle mbToggle;
    [SerializeField] UnityEngine.UI.Toggle ogToggle;
    [SerializeField] UnityEngine.UI.Toggle fullscreenToggle;
    [SerializeField] string fpsCounterOn;
    [SerializeField] string motionblurOn;
    [SerializeField] string ogGraphicsOn;
    [SerializeField] string isFullscreenOn;
    [SerializeField] bool isFullscreen;
    [SerializeField] bool On;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    private List<Resolution> availableResolutions = new List<Resolution>();
    [SerializeField] TMP_InputField cursorColorField;
    [SerializeField] TMP_InputField fpsCapInputField;


    private bool IsHexColor(string hex)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(hex, @"^#?[0-9A-Fa-f]{6}$");
    }

    public void SetAudioVolume(float volume)
    {
        SetMasterAudio SetMasterVolumeScript = gameObject.GetComponent<SetMasterAudio>();
        foreach (AudioSource audioSource in SetMasterVolumeScript.audioSources)
        {
            audioSource.volume = volume;
        }
    }
    public void OnResolutionChange(int index)
    {
        SetResolution(availableResolutions[index]);
        PlayerPrefs.SetInt("resIndex", index);
    }

    public void SetResolution(Resolution resolution)
    {
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
    }
    private bool ignoreFullscreenCallback = false;

    public void SetFullscreen(bool On)
    {
        if (ignoreFullscreenCallback) return;

        ignoreFullscreenCallback = true;
        isFullscreen = On;
        Screen.fullScreen = On;
        PlayerPrefs.SetString("gameFullscreen", On.ToString());
        fullscreenToggle.isOn = On;
        ignoreFullscreenCallback = false;
    }

    void Start()
    {
        List<string> options = new List<string>();
        Resolution[] resolutions = Screen.resolutions;

        foreach (Resolution res in resolutions)
        {
            string option = res.width + " x " + res.height;
            options.Add(option);
            availableResolutions.Add(res);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        int savedResIndex = PlayerPrefs.GetInt("resIndex", availableResolutions.Count - 1);
        resolutionDropdown.value = savedResIndex;
        SetResolution(availableResolutions[savedResIndex]);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
        audioVolumeSlider.onValueChanged.AddListener(SetAudioVolume);
        cursorColorField.onEndEdit.AddListener(ValidateColorInput);
        fpsCapInputField.onEndEdit.AddListener(OnFpsCapInputChanged);

        isFullscreen = PlayerPrefs.GetString("gameFullscreen", "false") == "true";
        Screen.fullScreen = isFullscreen;

        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.isOn = isFullscreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        print(isFullscreen);
        
        Load();
    }


    private void OnFpsCapInputChanged(string input)
    {
        if (float.TryParse(input, out float fpsValue))
        {
            fpsValue = Mathf.Clamp(fpsValue, 10f, 500f);
            fpsCapInputField.text = fpsValue.ToString();
            SetFPSCap(fpsValue);
        }
        else
        {
            fpsCapInputField.text = "60";
            SetFPSCap(60);
        }
    }
    private void ValidateColorInput(string input)
    {
        if (input.Length == 6 && IsHexColor(input))
        {
            // Valid hex color
        }
        else
        {
            // Reset
            cursorColorField.text = "ffffff";
        }
    }
    void Update()
    {
        textCameraSens.text = $"CAMERA SENSITIVITY | {Mathf.RoundToInt(camsensSlider.value)}x"; // CAMERA SENSITIVITY
        textAudioMult.text = $"AUDIO VOLUME | {audioVolumeSlider.value:F2}"; // AUDIO VOLUME
        curSizeText.text = $"CURSOR | SIZE {curSizeSlider.value:F1}%"; //CURSOR SIZE
    }
    public void SetFPSCap(float value)
    {
        Application.targetFrameRate = Mathf.RoundToInt(value);
        SaveSettings();
    }
    public void Load()
    {
        camsensSlider.value = PlayerPrefs.GetFloat("camSensitivity", 2);
        audioVolumeSlider.value = PlayerPrefs.GetFloat("audioMultiplier", 1);
        curSizeSlider.value = PlayerPrefs.GetFloat("curSizePercentage", 75);

        cursorColorField.text = PlayerPrefs.GetString("cursorColor", "ffffff");
        
        fpsToggle.isOn = PlayerPrefs.GetString("fpsCounter", "false") == "true";
        mbToggle.isOn = PlayerPrefs.GetString("motionBlur", "true") == "true";
        ogToggle.isOn = PlayerPrefs.GetString("ogGraphics", "false") == "true";

        int savedFpsCap = PlayerPrefs.GetInt("fpsCap", 60);
        fpsCapInputField.text = savedFpsCap.ToString();
        SetFPSCap(savedFpsCap);
    }
    public void SaveSettings()
    {
        if(fpsToggle.isOn) fpsCounterOn = "true";
        else fpsCounterOn = "false";

        if(mbToggle.isOn) motionblurOn = "true";
        else motionblurOn = "false";

        if(ogToggle.isOn) ogGraphicsOn = "true";
        else ogGraphicsOn = "false";

        if(fullscreenToggle.isOn) isFullscreenOn = "true";
        else isFullscreenOn = "false";

        if (int.TryParse(fpsCapInputField.text, out int fpsCap))
            {
                PlayerPrefs.SetInt("fpsCap", fpsCap);
            }
            else
            {
                fpsCap = 60;
                fpsCapInputField.text = "60";
                PlayerPrefs.SetInt("fpsCap", fpsCap);
            }

        PlayerPrefs.SetString("cursorColor", cursorColorField.text);
        PlayerPrefs.SetString("fpsCounter", fpsCounterOn);
        PlayerPrefs.SetString("motionBlur", motionblurOn);
        PlayerPrefs.SetString("ogGraphics", ogGraphicsOn);
        PlayerPrefs.SetString("gameFullscreen", isFullscreenOn);
        PlayerPrefs.SetFloat("audioMultiplier", audioVolumeSlider.value);
        PlayerPrefs.SetFloat("camSensitivity", camsensSlider.value);
        PlayerPrefs.SetFloat("curSizePercentage", curSizeSlider.value);
    }
    
    public void DeleteData()
    {
        if(animationClip_Delete.isPlaying)
        {
            animationClip_Delete.Stop();
            animationClip_Delete.Play("DataDeleted");
        }
        else
        {
            animationClip_Delete.Play("DataDeleted");            
        }

        //SET ALL BACK TO ORIGINAL
        PlayerPrefs.SetString("FBHR_chapter1CompletionTime", "99:99");
        PlayerPrefs.SetString("FBHR_chapter2CompletionTime", "99:99");
        PlayerPrefs.SetString("FBHR_chapter3CompletionTime", "99:99");
        PlayerPrefs.SetString("FBH2_chapter1CompletionTime", "99:99");
        PlayerPrefs.SetString("FBH2_chapter2CompletionTime", "99:99");
        PlayerPrefs.SetString("FBH2_chapter3CompletionTime", "99:99");
        PlayerPrefs.SetString("BONUS_chapter1CompletionTime", "99:99");

        PlayerPrefs.SetString("chapter2paid", "false");
        PlayerPrefs.SetString("chapter3paid", "false");
        PlayerPrefs.SetString("FBH2_chapter1paid", "false");
        PlayerPrefs.SetString("FBH2_chapter2paid", "false");
        PlayerPrefs.SetString("FBH2_chapter3paid", "false");

        PlayerPrefs.SetString("chapter1BoxPileActive", "true");


        PlayerPrefs.SetString("FBHR_chapter2Active", "false"); //DISPLAYS LOCK
        PlayerPrefs.SetString("FBHR_chapter3Active", "false");
        PlayerPrefs.SetString("FBH2_chapter1Active", "false");
        PlayerPrefs.SetString("FBH2_chapter2Active", "false");
        PlayerPrefs.SetString("FBH2_chapter3Active", "false");
        PlayerPrefs.SetString("bonusMap1_unlocked", "false");
        PlayerPrefs.SetString("bonusMap2_unlocked", "false");
        PlayerPrefs.SetString("bonusMap3_unlocked", "false");


        PlayerPrefs.SetString("ogGraphics", "false");
        PlayerPrefs.SetString("motionBlur", "true");
        PlayerPrefs.SetString("cursorColor", "ffffff");
        PlayerPrefs.SetFloat("moneyAmount", 0);
        PlayerPrefs.SetFloat("moneySpent", 0);
        PlayerPrefs.SetString("fpsCounter", "false");
        PlayerPrefs.SetFloat("curSizePercentage", 75);
        PlayerPrefs.SetFloat("camSensitivity", 2);
        PlayerPrefs.SetFloat("audioMultiplier", 1);

        PlayerPrefs.SetInt("timesPlayed", 0);
        PlayerPrefs.SetInt("timesBeaten", 0);
        PlayerPrefs.SetInt("timesDied", 0);
        PlayerPrefs.SetInt("pingTimes", 0);
        PlayerPrefs.SetInt("currentCosmetic", 0);
        PlayerPrefs.DeleteKey("EncryptedUnlockData");

        UnlockData newUnlockData = new UnlockData
        {
            IDsUnlocked = new int[] { 0 }, // Example unlocked IDs
            IDsPurchased = new int[] { 0 } // Example purchased ID
        };

        string json = JsonUtility.ToJson(newUnlockData);
        string encrypted = CryptoUtility.Encrypt(json);
        PlayerPrefs.SetString("EncryptedUnlockData", encrypted);

        PlayerPrefs.Save();

        Load();
    }
}
