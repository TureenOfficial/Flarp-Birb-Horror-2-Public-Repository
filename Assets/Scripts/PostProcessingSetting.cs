using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingSetting : MonoBehaviour
{
    [SerializeField] PostProcessVolume ppv;
    void Start()
    {
        ppv = GameObject.Find("postProcessing").GetComponent<PostProcessVolume>();
        if(GameObject.Find("postProcessing") == null) ppv = GameObject.Find("ppV").GetComponent<PostProcessVolume>();

        if(PlayerPrefs.GetString("ogGraphics", "false") == "true")
        {

            ppv.enabled = false;
        }
        else
        {
            ppv.enabled = true;
        }
    }
}
