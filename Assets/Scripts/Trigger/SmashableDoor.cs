using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmashableDoor : MonoBehaviour
{
    [SerializeField] TMP_Text Text;
    [SerializeField] GameObject DeactivateSmashWall;
    [SerializeField] AudioSource SmashSFX;
    [SerializeField] bool NeedsScore;
    [SerializeField] bool ScoreSpecific;
    [SerializeField] int ScoreNeeded;
    void Start()
    {
        DeactivateSmashWall.SetActive(true); 
    }

    void OnTriggerEnter(Collider other)
    {
        if(NeedsScore && ScoreSpecific)
        {
            if(GameDetail.Instance.ScoreCurrent >= GameDetail.Instance.ScoreNeeded)
            {
                Smash();
            }
        }
        else if (NeedsScore && !ScoreSpecific)
        {
            if(GameDetail.Instance.ScoreCurrent >= ScoreNeeded)
            {
                Smash();
            }
        }
        else
        {
            Smash();
        }
    }

    void Update()
    {
        if(NeedsScore && ScoreSpecific)
        {
            Text.gameObject.SetActive(true);
            Text.text = $"{GameDetail.Instance.ScoreCurrent} / {GameDetail.Instance.ScoreNeeded}";
        }   
        else if (NeedsScore && !ScoreSpecific)
        {
            Text.gameObject.SetActive(true);
            Text.text = $"{GameDetail.Instance.ScoreCurrent} / {ScoreNeeded}";
        }
        else
        {
            Text.gameObject.SetActive(false);
        }
    }

    void Smash()
    {
            SmashSFX.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false; 
            DeactivateSmashWall.SetActive(false);           
    }

}
