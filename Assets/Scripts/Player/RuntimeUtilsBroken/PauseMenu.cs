using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject ForcePauseUI;
    [SerializeField] GameObject ForceDefaultUI;
    
    [Header("Input Management")]
    [SerializeField] KeyCode PauseMenuKey = KeyCode.Escape;
    [SerializeField] public static bool GamePaused;

    [Header("Audio")]
    [SerializeField] List<AudioSource> AudioSources;

    void Start()
    {
        foreach (AudioSource source in GameDetail.Instance.Sources)
        {
            AudioSources.Add(source);
        }
        Unpause();
    }
    void Update()
    {
        if(Input.GetKeyDown(PauseMenuKey) && GameDetail.Instance.gameActive && 
            !PlayerMap.Instance.mapActive && PhotoModeScript.Instance.PhotoModeActive)
        {
            GamePaused = true;
            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            PauseAudio();

            ForcePauseUI.SetActive(true); //Pause UI
            ForceDefaultUI.SetActive(false); //Main UI
        }    
        else if(Input.GetKeyDown(PauseMenuKey) && GameDetail.Instance.gameActive && 
            !PlayerMap.Instance.mapActive && PhotoModeScript.Instance.PhotoModeActive)
        {
            Unpause();   
        }
    }

    public void PauseAudio()
    {
        foreach (AudioSource source in AudioSources)
        {
                if(source.gameObject.activeInHierarchy && source.enabled && source.gameObject.name != "LossAudioSource" && source.gameObject.name != "EXIT_DOOR" && source.gameObject.name != "GameManager")
                {
                    source.Pause();
                }
        }
    }
    public void Unpause()
    {
        GamePaused = false;
        Time.timeScale = 1f;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;


        foreach (AudioSource source in AudioSources)
        {
            if(source.gameObject.activeInHierarchy && source.enabled && source.gameObject.name != "LossAudioSource")
            {
                    source.UnPause();
            }
        }

        ForcePauseUI.SetActive(false);
        ForceDefaultUI.SetActive(true);
    }
}
