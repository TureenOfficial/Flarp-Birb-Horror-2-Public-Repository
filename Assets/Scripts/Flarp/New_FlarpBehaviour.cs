using System;
using GameJolt.API;
using UnityEngine;
using UnityEngine.AI;

public class New_FlarpBehaviour : MonoBehaviour
{
    //MISC
    [SerializeField] public static int timesPlayedNoticeAudio;
    public bool isChasingPlayer;
    [SerializeField] GameObject ChasingCanvas;
    [SerializeField] GameObject[] RandomPoints;
    public NavMeshAgent flarpNavMesh;
    public string behaviourToInherit;
    [SerializeField] bool isBillboard;
    [SerializeField] bool LOSSpeedUp;
    public bool constantKnowledge;
    public bool sensesFlashlight = false;
    public bool HeKnowsWhenYouEatPoptarts;
    [SerializeField] Transform playerTransform;

    //END

    //---------------

    //VARIABLE
    [SerializeField] Vector3 directionToPlayer;
    public float SavedDetectionRange;
    public float detectionRange;
    public float distanceToPlayer;
    public int timesDetected;
    [SerializeField] float movementSpeed;
    [SerializeField] float speedMax;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] flarpSprite;
    public bool doNotInherit;
    public bool canKill = true;
    public AudioSource runForYourLife;
    public AudioSource flarpAudioSource;
    public AudioSource noticeAudioSource;
    public AudioSource chasingloopAudioSource;
    public AudioClip[] flarpClip;
    public AudioClip[] noticeClips;
    public AudioClip chasingloop;
    private bool hasDetectedOnce = false;
    //END

    //---------------

    public void Summon()
    {
        this.gameObject.SetActive(true);
        runForYourLife.Play();
    }


    void Start()
    {
        SavedDetectionRange = detectionRange;
        chasingloopAudioSource.clip = chasingloop;
        this.gameObject.SetActive(false);
        runForYourLife.volume = 1f * PlayerPrefs.GetFloat("audioMultiplier");
        noticeAudioSource.volume = 1f * PlayerPrefs.GetFloat("audioMultiplier");
        chasingloopAudioSource.volume = 0.75f * PlayerPrefs.GetFloat("audioMultiplier"); 
        flarpAudioSource.volume = 0.05f * PlayerPrefs.GetFloat("audioMultiplier");
        
        flarpNavMesh = gameObject.GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        
        if(doNotInherit == false) spriteRenderer.sprite = Array.Find(flarpSprite, sprite => sprite.name == behaviourToInherit);
        Summon();
    }
    void Update()
    {
        if(PlayerDetails.PlayerInvincible)
        {
            canKill = false;
        }
        else
        {
            canKill = true;
        }

        flarpNavMesh.speed = movementSpeed;
        if(isBillboard)
        {
            gameObject.transform.LookAt(playerTransform);
        }

        if(LOSSpeedUp == true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player") && movementSpeed < speedMax)
                {
                    movementSpeed += 0.02f;
                }
            }
        }


        if (isChasingPlayer && !hasDetectedOnce)
        {
            timesDetected++;
            hasDetectedOnce = true;

            if(GameObject.FindObjectOfType<GameJoltAPI>() != null && GameJoltAPI.Instance.CurrentUser != null)
            {
                Debug.Log("Tried to unlock trophy: 262864");
                Trophies.TryUnlock(262864);
            }
        }

        if (!isChasingPlayer && hasDetectedOnce)
        {
            hasDetectedOnce = false;
        }
    }
    
    void NewDestination()
    {
        if (!flarpNavMesh.isOnNavMesh || (flarpNavMesh.remainingDistance <= flarpNavMesh.stoppingDistance && !flarpNavMesh.hasPath))
        {
            GameObject RandomPoint = RandomPoints[UnityEngine.Random.Range(0, RandomPoints.Length)];
            Vector3 randomDestination = RandomPoint.transform.position;
            if(RandomPoint.activeInHierarchy == true)
            {
                            flarpNavMesh.SetDestination(randomDestination);
            }
        }
    }

    void FixedUpdate()
    {
        ChasingCanvas.SetActive(isChasingPlayer);
        directionToPlayer = (playerTransform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        chasingloopAudioSource.pitch = (1+distanceToPlayer/100);

        if(behaviourToInherit == "Flarpy Warpia")
        {
            if(!playerTransform.gameObject.GetComponent<FlashlightObject>().flashlightActive)
            {
                detectionRange = 15;
            }
            else
            {
                detectionRange = 20;
            }
        }
        if(playerTransform.gameObject.GetComponent<FlashlightObject>().flashlightActive && playerTransform.position.y < flarpNavMesh.height && distanceToPlayer < detectionRange && sensesFlashlight)
        {
            flarpNavMesh.SetDestination(playerTransform.position);
        }

        if (constantKnowledge)
        {
            if (!chasingloopAudioSource.isPlaying) chasingloopAudioSource.Play();
            isChasingPlayer = true;
            if(playerTransform.position.y < flarpNavMesh.height)
            {
                flarpNavMesh.SetDestination(playerTransform.position);
            }
        }
        else
        {
            RaycastHit found;
            if (distanceToPlayer <= detectionRange)
            {
                if (Physics.Raycast(transform.position, directionToPlayer, out found, detectionRange))
                {
                    if (found.collider.CompareTag("Player"))
                    {
                        if (!chasingloopAudioSource.isPlaying) chasingloopAudioSource.Play();
                        isChasingPlayer = true;
                        flarpNavMesh.SetDestination(playerTransform.position);
                        if (isChasingPlayer && timesPlayedNoticeAudio <= 0)
                        {
                            timesPlayedNoticeAudio++;
                            if (!noticeAudioSource.isPlaying)
                                noticeAudioSource.PlayOneShot(noticeClips[0]);
                        }
                    }
                }
            }
            else
            {

                if (distanceToPlayer > detectionRange)
                {
                    if(distanceToPlayer < detectionRange + 40 && playerTransform.position.y > flarpNavMesh.height)
                    {
                        if (Physics.Raycast(transform.position, directionToPlayer, out found, detectionRange))
                        {
                            if (!found.collider.CompareTag("Player"))
                            {
                                if (isChasingPlayer)
                                {
                                    chasingloopAudioSource.Stop();
                                    if (!noticeAudioSource.isPlaying)
                                        noticeAudioSource.PlayOneShot(noticeClips[1]);
                                    timesPlayedNoticeAudio = 0;
                                    flarpNavMesh.ResetPath();
                                    isChasingPlayer = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(isChasingPlayer)
                        {
                            chasingloopAudioSource.Stop();
                            if (!noticeAudioSource.isPlaying)
                                noticeAudioSource.PlayOneShot(noticeClips[1]);
                            timesPlayedNoticeAudio = 0;
                            flarpNavMesh.ResetPath();
                            isChasingPlayer = false;
                        }
                    }
                    if (!isChasingPlayer)
                    {
                        NewDestination();
                    }
                }
            }
        }   
        if (!isChasingPlayer && flarpNavMesh.remainingDistance <= flarpNavMesh.stoppingDistance)
        {
            NewDestination();
        }
    }
}
