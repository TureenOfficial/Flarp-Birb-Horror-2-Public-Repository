using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using GameJolt.API;

public class CollectableRework : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] Light LightSource;
    [SerializeField] Color LightColor = new Color(255,255,255,255);
    [SerializeField] float LightStrength = 1.2f;

    [Header("Sprite")]
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Sprite Sprite;

    [Header("Logic")]
    [SerializeField] TMP_Text collectText;
    [SerializeField] bool topText;
    [SerializeField] int PlusAmount;
    [SerializeField] bool Billboard;
    [SerializeField] Transform playerTransform;

    [Header("Achievement")]
    [SerializeField] bool Achievement;
    [SerializeField] int AchievementID;

    [Header("Score")]
    [SerializeField] bool DoesReward;
    [SerializeField] float RewardAmount;


    [Header("Audio")]
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip[] AudioClips;
    [SerializeField] int[] ClipIndexes;

    void Start()
    {
        if(!topText) collectText.gameObject.SetActive(false);
        if(LightSource == null) LightSource = gameObject.GetComponentInChildren<Light>();
        if(collectText == null && topText) collectText = gameObject.GetComponentInChildren<TMP_Text>();
    }
    public void OnTriggerEnter(Collider other)
    {
        GameDetail.Instance.ScoreCurrent += PlusAmount;
        
        foreach (int index in ClipIndexes)
        {
            Source.PlayOneShot(AudioClips[index]);
        }

        if(DoesReward)
        {
            MoneyManager.Instance.AddTotal(RewardAmount);
        }

        if(Achievement && GameObject.FindObjectOfType<GameJoltAPI>() != null && GameJoltAPI.Instance.CurrentUser != null)
        {
            Trophies.TryUnlock(AchievementID);
        }
        SpriteRenderer.enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        collectText.enabled = false;
        LightSource.enabled = false;
    }

    void Update()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if(Billboard)
        {
            this.transform.LookAt(playerTransform);
        }   

        if(topText)
        {
            collectText.text = $"COLLECT ALL {GameDetail.Instance.ScoreNeeded} {GameDetail.Instance.collectableName.ToUpper()}";
            collectText.gameObject.transform.LookAt(playerTransform);
            gameObject.transform.Rotate(0f, 180f, 0f);
        }


        if(LightSource)
        {
            LightSource.intensity = LightStrength;
            LightSource.color = LightColor;
        }
    }
}
