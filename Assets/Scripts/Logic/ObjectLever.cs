using System.Xml.Serialization;
using UnityEngine;

public class ObjectLever  : MonoBehaviour
{
    [Header("Switch Logic")]
    [SerializeField] public Switch switchLogic; 
    [SerializeField] public int leverObject;
    [SerializeField] private bool isClicked = false;
    [SerializeField] float MoneyAdded = 30;

    [Header("Sprite")]
    [SerializeField] public SpriteRenderer srLever;
    [SerializeField] public Sprite[] spritesLever;

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;


    void Start()
    {
        srLever.sprite = spritesLever[0];
        audioSource.volume = 1 * PlayerPrefs.GetFloat("audioMultiplier");
    }
    void Update()
    {
        if (isClicked)
            return;


        if (Input.GetMouseButtonDown(0)) 
        {
            if(GameDetail.Instance.gameActive && !PlayerMap.Instance.mapActive && !PauseMenu.GamePaused)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, PlayerDetails.PlayerRaycastDistance) && GameDetail.Instance.gameActive && !PlayerMap.Instance.mapActive)
                {
                    if (hit.collider == GetComponent<Collider>())
                    {
                        MoneyManager.Instance.AddTotal(MoneyAdded);
                        audioSource.Play();
                        srLever.sprite = spritesLever[1];
                        isClicked = true;
                        gameObject.tag = "Untagged";
                        switchLogic.OnButtonClick();
                    }
                }
            }
        }
    }


}