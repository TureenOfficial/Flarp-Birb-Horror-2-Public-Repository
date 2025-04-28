using UnityEngine;

public class BoxPile : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetString("chapter1BoxPileActive", "false") == "true")
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
