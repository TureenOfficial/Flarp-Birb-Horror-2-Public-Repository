
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CosmeticTextUI : MonoBehaviour
{
    [SerializeField] TMP_Text[] TextDetails;
        /*
        0 = HEADER
        1 = DETAILS
        */

    void Update()
    {
            switch(PlayerPrefs.GetInt("currentCosmetic", 0))
            {
                case 0:
                {
                    TextDetails[0].text = "None";
                    TextDetails[1].text = $"SPEED: 1x\nSTAMINA: 1x\nREACH: 1x\nGRAVITY: 1x";
                    break;
                }
                case 1:
                {
                    TextDetails[0].text = "DanTDM's Diamond Boots";
                    TextDetails[1].text = $"SPEED: 1.5x\nSTAMINA: 0.5x\nREACH: 1x\nGRAVITY: 1.2x\n\nMade of a strong sturdy condensed gem, Dan's boots are surely enough to boost you in the correct direction.\n\nHINT: Power the boots with Poptarts!";
                    break;
                }
                case 2:
                {
                    TextDetails[0].text = "Overfueled Poptart";
                    TextDetails[1].text = $"SPEED: 0.85x\nSTAMINA: 2x\nREACH: 0.4x\nGRAVITY: 0.9x\n\nFlavoured to perfection, The 'Overfueled Poptart' doubles your stamina as a trade for 15% of your speed.\n\nHINT: Collect all the Flarp Children";
                    break;
                }
                case 3:
                {
                    TextDetails[0].text = "Anti-Gravity Jacket";
                    TextDetails[1].text = $"SPEED: 0.95x\nSTAMINA: 0.75x\nREACH: 1.4x\nGRAVITY: 0.75x\n\nThe floaty nature of this jacket will push your boundaries, explore anywhere!\n\nHINT: Chapter 2 has a need for speed...";
                    break;
                }
            }
    }
}