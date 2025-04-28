using TMPro;
using UnityEngine;

public class FlashlightObject : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] bool UIEnabled;
    [SerializeField] GameObject FlashlightUI;

    [Header("Audio")]
    [SerializeField] AudioSource flashlightObjectAudio;

    [Header("Mechanics")]
    [SerializeField] public bool flashlightActive;

    [Header("Object Management")]
    [SerializeField] GameObject Flashlight_GameObject;
    [SerializeField] GameObject Flashlight_SpotLight;

    [Header("Input Handling")]
    [SerializeField] KeyCode FlashlightKeyCode = KeyCode.F;

    void Start()
    {
        if(GameDetail.Instance.playerHasFlashlight)
        {
            Flashlight_GameObject.SetActive(true);
            FlashlightUI.SetActive(true);
            flashlightActive = false;
        }
        else
        {
            Flashlight_GameObject.SetActive(false);
            FlashlightUI.SetActive(false);
            flashlightActive = false;
        }
    }
    void Update()
    {
        if(!GameDetail.Instance.playerHasFlashlight || PlayerMap.Instance.mapActive) return;
        else
        {
            if(flashlightActive && Input.GetKeyDown(FlashlightKeyCode))
            {
                flashlightObjectAudio.Play();
                flashlightActive = false;
            }
            else if(!flashlightActive && Input.GetKeyDown(FlashlightKeyCode))
            {
                flashlightObjectAudio.Play();
                flashlightActive = true;
            }

            Flashlight_SpotLight.SetActive(flashlightActive);
            if(UIEnabled)
            {
                FlashlightUI.GetComponentInChildren<TMP_Text>().text = FlashlightKeyCode.ToString().ToUpper();
                FlashlightUI.SetActive(true);
            }
            else
            {
                FlashlightUI.SetActive(false);
            }
        }
    }
}
