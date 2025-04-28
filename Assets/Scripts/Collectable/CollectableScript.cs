using System;
using TMPro;
using UnityEngine;
using GameJolt.API;

public class CollectableScript : MonoBehaviour
{
    public RuntimeUtils utils;
    [SerializeField] bool giveAchievement = true; 
    [SerializeField] bool doesIdleSound = false;
    [SerializeField] float moneyToGive = 60;
    [SerializeField] BoxCollider pCollider;
    [SerializeField] bool isBillboard;
    [SerializeField] bool showsText;
    [SerializeField] TMP_Text collectText;
    private Transform playerTransform;
    private Vector3 negPlayerPosition;
    [SerializeField] AudioSource idleSound;
    [SerializeField] AudioSource collectionSound;
    [SerializeField] Light LightObject;
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        utils = GameObject.Find("Player").GetComponent<RuntimeUtils>();
        if(showsText)
        {
            collectText.gameObject.SetActive(true);
            collectText.text = $"COLLECT ALL {utils.itemsNeeded} {utils.itemQuota.ToUpper()}";
        }
        else
        {
            collectText.gameObject.SetActive(false);
        }

    }
    void OnTriggerEnter(Collider pCollider)
    {
        if(pCollider.CompareTag("Player"))
        {
            utils.AddMoreMoney(moneyToGive);
            utils.InvokeAddToTotal();
            collectionSound.Play();
            if(idleSound != null) idleSound.Stop();
            Destroy(this.gameObject.GetComponent<SpriteRenderer>());
            Destroy(LightObject);
            Destroy(this.gameObject.GetComponent<BoxCollider>());
            if (showsText && collectText != null)
            {
                Destroy(collectText.gameObject);
            }

            if(giveAchievement && GameJoltAPI.Instance.CurrentUser != null)
            {
                Trophies.TryUnlock(262892);
            }
        }
    }
    void Update()
    {
        if(doesIdleSound && idleSound != null)
        {
            if(utils.playerNotDefault || utils.PauseCanvas.activeInHierarchy || utils.photoMode || utils.mapMode)
            {
                idleSound.Pause();
            }
            else
            {
                idleSound.UnPause();
            }
        }

        if(isBillboard)
        {
            this.gameObject.transform.LookAt(playerTransform);
        }
        if(showsText && collectText != null)
        {
            collectText.gameObject.transform.LookAt(playerTransform);
            collectText.gameObject.transform.Rotate(0f, 180f, 0f);
        }
    }
}
