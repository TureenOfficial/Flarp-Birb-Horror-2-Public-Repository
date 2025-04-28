using UnityEngine;

public class AppOpenURL : MonoBehaviour
{
    public void Open(string URL)
    {
        Application.OpenURL(URL);
    }
}
