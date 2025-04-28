using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine;
public class ObjectDoor : MonoBehaviour
{
    [SerializeField] string Condition;
    [SerializeField] UnityEvent OnDoorClicked;
    [SerializeField] AudioSource audioSourceDoor;
    [SerializeField] AudioClip[] audioClipsDoor;
    public bool doorUnlocked;
    private bool isClicked = false;

    public void Start()
    {
        doorUnlocked = false;
    }


    void Update()
    {
        if (isClicked)
            return;

        if(doorUnlocked)
        {
            this.gameObject.tag = "lightCursor";
        }
        else
        {
            this.gameObject.tag = "lockedCursor";
        }

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
                        DoorTried();
                    }
                }
            }
        }


        if(Condition == "Collect All Objects" || Condition == "")
        {
            if(GameDetail.Instance.ScoreCurrent >= GameDetail.Instance.ScoreNeeded)
            {
                doorUnlocked = true;
            }
            else
            {
                doorUnlocked = false;    
            }
        }
    }

    public void DoorTried()
    {
        if(doorUnlocked == false)
        {
            audioSourceDoor.PlayOneShot(audioClipsDoor[0]);
        }
        else if(doorUnlocked == true)
        {
            OnDoorClicked?.Invoke();
            audioSourceDoor.PlayOneShot(audioClipsDoor[1]);
            isClicked=true;
            GameDetail.Instance.Win();
        }
    }
}