using UnityEngine;

public class SetMasterAudio : MonoBehaviour
{
    public AudioSource[] audioSources;
    public bool audioVolumeFollowsSource = false;

    void Start()
    {
        if(audioSources != null)
        {
                foreach (AudioSource aud in audioSources)
                {       
                        if(audioVolumeFollowsSource)
                        {
                            aud.volume *= PlayerPrefs.GetFloat("audioMultiplier");
                        }
                        else
                        {
                            aud.volume = 1 * PlayerPrefs.GetFloat("audioMultiplier");
                        }
                }
        }
    }
}
