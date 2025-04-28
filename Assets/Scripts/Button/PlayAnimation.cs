using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animation animation_;
    public string animation_toPlay;
    public void Play()
    {
        animation_.Play(animation_toPlay);
    } 
}
