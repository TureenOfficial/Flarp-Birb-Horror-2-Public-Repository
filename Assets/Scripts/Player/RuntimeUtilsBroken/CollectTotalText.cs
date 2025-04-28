using TMPro;
using UnityEngine;

public class CollectTotalText : MonoBehaviour
{
    [SerializeField] TMP_Text Text;

    [SerializeField] string DefaultTopText = "Poptarts";
    [SerializeField] string TotalCollectedText = "0/7";

    void Update()
    {
        DefaultTopText = GameDetail.Instance.collectableName.ToUpper();
        TotalCollectedText = $"{GameDetail.Instance.ScoreCurrent}/{GameDetail.Instance.ScoreNeeded}";   

        Text.text = $"{DefaultTopText}: {TotalCollectedText}";
    }
}
