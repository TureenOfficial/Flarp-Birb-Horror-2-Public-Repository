using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Misc")]
    public RuntimeUtils utils;

    [Header("Audio")]
    public AudioSource playerAud;
    public AudioClip[] audClip;

    [Header("Character Controller")]
    public CharacterController characterController;
    
    [Header("Walking/Running Bool")]
    private bool isWalking;
    private bool isRunning;

    [Header("Timer")]
    private float footstepCooldown = 0.5f; //Time between footsteps
    private float lastFootstepTime = -1f;

    private void Start()
    {
        playerAud.volume = 0.9f * PlayerPrefs.GetFloat("audioMultiplier");
    }

    private void Update()
    {
        if(PlayerController.canMove == false)
        {
            playerAud.enabled = false;
        }
        else
        {
            playerAud.enabled = true;
        }

        isRunning = Input.GetKey(KeyCode.LeftShift) && gameObject.GetComponent<PlayerController>().playerStamina > 0;

        if (utils != null && utils.playerNotDefault)
        {
            isWalking = false;
        }
        else
        {
            isWalking = Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f;
        }

        if (isWalking && !playerAud.isPlaying && characterController.isGrounded)
        {
            if (Time.time - lastFootstepTime >= footstepCooldown * Time.deltaTime)
            {
                playerAud.PlayOneShot(audClip[Random.Range(0, audClip.Length)]);
                lastFootstepTime = Time.time;
            }
        }

        if (!isWalking && playerAud.isPlaying)
        {
            playerAud.Stop();
        }
        playerAud.pitch = isRunning ? 1.3f : 1f;
    }
}
