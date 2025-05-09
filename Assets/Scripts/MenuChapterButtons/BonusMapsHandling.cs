using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusMapsHandling : MonoBehaviour
{
    [SerializeField] bool[] HideDetails;
    [SerializeField] Button[] BonusLevelsButton;
    [SerializeField] RawImage[] LevelImage;
    [SerializeField] TMP_Text[] BonusLevel_Text;
    [SerializeField] TMP_Text[] BonusLevel_Name;

    [SerializeField] Texture[] LockedTextures;
    [SerializeField] Texture[] UnlockedTextures;

    void Start()
    {
        if(PlayerPrefs.GetString("bonusMap1_unlocked", "false") == "true")
        {
            BonusLevelsButton[0].interactable = true;
            BonusLevel_Text[0].text = "PLAY";
            LevelImage[0].texture = UnlockedTextures[0];
        }
        else
        {

            BonusLevelsButton[0].interactable = false;
            BonusLevel_Text[0].text = "LOCKED";
            if(HideDetails[0])
            {
                LevelImage[0].texture = LockedTextures[0];
                BonusLevel_Name[0].text = "???";
            }
        }

        if(PlayerPrefs.GetString("bonusMap2_unlocked", "false") == "true")
        {
            BonusLevelsButton[1].interactable = true;
            BonusLevel_Text[1].text = "PLAY";
            LevelImage[1].texture = UnlockedTextures[1];
        }
        else
        {
            BonusLevelsButton[1].interactable = false;
            BonusLevel_Text[1].text = "LOCKED";

            if(HideDetails[1])
            {
                LevelImage[1].texture = LockedTextures[1];
                BonusLevel_Name[1].text = "???";
            }
        }
    }
}
