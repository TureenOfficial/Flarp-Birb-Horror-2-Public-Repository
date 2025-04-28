using UnityEngine;

public class BonusLevelsBeaten : MonoBehaviour
{
    [SerializeField] GameObject[] BeatenCheckmarks;
    [SerializeField] int[] levelCheck;

    void Update()
    {
        foreach(int level in levelCheck)
        {
            if(PlayerPrefs.GetString($"bonusLevel{level}beaten", "false") == "true")
            {
                BeatenCheckmarks[level -1].SetActive(true);
            }
            else
            {
                BeatenCheckmarks[level -1].SetActive(false);
            }
        }
    }
}
