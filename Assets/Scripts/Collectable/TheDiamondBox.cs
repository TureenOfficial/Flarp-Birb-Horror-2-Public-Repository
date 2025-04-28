using UnityEngine;
using System;

public class TheDiamondBox : MonoBehaviour
{
    [SerializeField] bool doesSearchText;
    [SerializeField] GameObject myTDM;
    [SerializeField] ParticleSystem particleSystemTDM;
    [SerializeField] AudioClip[] boxSounds;
    public Animator _Danimator;
    public AudioSource audioSource;
    private bool isClicked = false;

    void Start()
    {
       particleSystemTDM.Stop();
        _Danimator.SetBool("OpenBox", false);
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
                        gameObject.tag = "Untagged";
                        isClicked = true;
                        if(UnityEngine.Random.Range(1,25) == 15)
                        {
                            particleSystemTDM.Play();
                            myTDM.SetActive(true);
                            GameDetail.Instance.ScoreCurrent ++;
                            _Danimator.SetBool("OpenBox", true);
                            GameDetail.Instance.Win();
                            audioSource?.PlayOneShot(boxSounds[0]);
                        }
                        else
                        {                      
                            particleSystemTDM.Play();
                            audioSource?.PlayOneShot(boxSounds[1]);
                            myTDM.SetActive(false);   
                            _Danimator.SetBool("OpenBox", true);  
                        }
                    }
                }
            }   
        }
    }
}
