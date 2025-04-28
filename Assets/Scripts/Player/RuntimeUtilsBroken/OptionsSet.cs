using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OptionsSet : MonoBehaviour
{
    [SerializeField] GameObject FPSObject;
    [SerializeField] PostProcessVolume[] ppV;
    [SerializeField] MotionBlur motionBlur;

    void Update()
    {
        if(PlayerPrefs.GetString("fpsCounter", "false") == "true")
        {
            FPSObject.SetActive(true);
        }
        else
        {
            FPSObject.SetActive(false);
        }

        if(PlayerPrefs.GetString("ogGraphics", "false") == "true")
        {
            foreach (PostProcessVolume volume in ppV)
            {
                volume.enabled = false;
            }
            RenderSettings.fogDensity = 0;
        }
        else
        {
            foreach (PostProcessVolume volume in ppV)
            {
                volume.enabled = true;
            }
        }

        string motionblurOn = PlayerPrefs.GetString("motionBlur", "true");

        if(motionblurOn == "true")
        {
            foreach (PostProcessVolume volume in ppV)
            {
                volume.profile.TryGetSettings(out motionBlur);
            }
            motionBlur.enabled.value = true;
        }
        else
        {
            foreach (PostProcessVolume volume in ppV)
            {
                volume.profile.TryGetSettings(out motionBlur);
            }
            motionBlur.enabled.value = false;
        }
    }
}
