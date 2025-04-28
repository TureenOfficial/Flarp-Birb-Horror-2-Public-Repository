using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [Header("Misc")]
    [SerializeField] Animator _EyeAnim;
    [SerializeField] New_FlarpBehaviour flarpBehaviour;
    [SerializeField] public static bool canMove = true;
    public bool pausedState;

    [Header("Stamina")]
    [SerializeField] public float playerStamina;
    [SerializeField] float staminaDrainAmt = 25;
    [SerializeField] float staminaMax = 100;
    [SerializeField] Slider staminaBarVisualiser;
    public GameObject staminaIcon;
    public RawImage staminaIconImage;

    [Header("Speed")]
    [SerializeField] float walkingSpeed = 4;
    [SerializeField] float runningSpeed = 7;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float gravity = 20.0f;

    [Header("Vision")]
    [SerializeField] float lookSpeed;
    [SerializeField] float lookXLimit = 30.0f;
    [SerializeField] AudioSource jumpAudio;

    bool isRunning;
    float curSpeedX => canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
    float curSpeedY => canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;

    [Header("Camera")]
    public Camera playerCamera;
    public CharacterController characterController;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float Running_Timer = 0;

    void Awake()
    {
        staminaIcon.GetComponent<RawImage>();
        staminaIcon = GameObject.Find("staminaIcon");
        if(staminaIconImage == null) staminaIconImage = staminaIcon.GetComponent<RawImage>();
        if(staminaBarVisualiser == null) staminaBarVisualiser = GameObject.Find("StaminaBar").GetComponent<Slider>();
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "fbhr_Chapter3")
        {
            PlayerDetails.PlayerSpeedMultiplier = 1.25f;
        }


        canMove = true;
        playerStamina = staminaMax;
        
        characterController = GetComponent<CharacterController>();
        // Lock cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    void Update()
    {

        //What is canMove?
        if(!PlayerMap.Instance.mapActive || !PauseMenu.GamePaused || GameDetail.Instance.gameActive)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }


        if(!GameDetail.Instance.playerHasStamina)
        {
            staminaDrainAmt = 0;
            playerStamina = staminaMax;
            staminaBarVisualiser.gameObject.SetActive(false);
        }

        if(!GameDetail.Instance.gameActive || PauseMenu.GamePaused)
        {
            canMove = false;
        }

        staminaBarVisualiser.value = playerStamina;

        staminaIconImage.color = new Color(255,255,255,1 * (playerStamina/100));
        if(flarpBehaviour != null && _EyeAnim != null) 
        {
            _EyeAnim.speed = 1 + (30 / flarpBehaviour.distanceToPlayer);
        }
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            characterController.Move(moveDirection * Time.deltaTime);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if(canMove)
        {
            lookSpeed =  PlayerPrefs.GetFloat("camSensitivity", 2);
            Math.Clamp(lookSpeed, 1, 4);
        }
        else
        {
            lookSpeed = 0;
        }
    }
    void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }
        else
        { 
            if (Input.GetKey(KeyCode.LeftShift) && (Math.Abs(curSpeedX) > 0 ||  Math.Abs(curSpeedY) > 0))
            {
                if(playerStamina > 0)
                {
                    isRunning = true;
                    Running_Timer = 0;
                    playerStamina -= staminaDrainAmt * Time.deltaTime;
                }
                else
                {
                        isRunning = false;
                }
            }
            else
            {
                    isRunning = false;
            }

            if(!isRunning)
            {
                Running_Timer += Time.deltaTime;  
                if (Running_Timer > 1f)  
                {
                    if (playerStamina < staminaMax) playerStamina += (staminaDrainAmt + 5) * Time.deltaTime;
                }
            }
            
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

            float movementDirectionY = moveDirection.y;

            moveDirection = ((forward * curSpeedX) + (right * curSpeedY)) * PlayerDetails.PlayerSpeedMultiplier;

            if(flarpBehaviour != null && flarpBehaviour.isChasingPlayer)
            {
                runningSpeed = walkingSpeed + (flarpBehaviour.distanceToPlayer / 10);
            }
            else
            {
                runningSpeed = 8f;
            }

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                jumpAudio.Play();
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded && Time.timeScale != 0f && GameDetail.Instance.gameActive && !PauseMenu.GamePaused)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }
    }
}