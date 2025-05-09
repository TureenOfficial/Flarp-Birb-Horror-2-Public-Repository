using UnityEngine;
using TMPro;

public class CollectableDoor : MonoBehaviour
{
    [SerializeField] TMP_Text CollectableText;
    [SerializeField] bool Unlocked;

    [SerializeField] int ScoreNeeded;

    void Start()
    {
        
    }

    void Update()
    {
        if(Unlocked) this.gameObject.SetActive(false);
        CollectableText.text = $"{GameDetail.Instance.ScoreCurrent}/{ScoreNeeded}";  

        if(GameDetail.Instance.ScoreCurrent >= ScoreNeeded)
        {
            Unlocked = true;
        }
    }
}
