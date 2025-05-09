using System;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] Vector3 TeleportPosition;
    [SerializeField] Transform PlayerTransform;

    [Header("On Trigger")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool doesSound;
    [SerializeField] bool isOnTrigger;
    [SerializeField] bool OOBTrigger = true;

    public void OnTriggerEnter(Collider collider)
    {
        if(isOnTrigger)
        {
            TP();
        }
    }
    public void TP()
    {
        if(OOBTrigger)
        {
            InfoPing.Instance.Ping("Out of bounds!", 5, false);
        }
        if(doesSound)
        {
            audioSource.Play();
        }
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerTransform.GetComponent<CharacterController>().enabled = false;
        PlayerTransform.GetComponent<CharacterController>().detectCollisions = false;
        PlayerTransform.position = TeleportPosition;
        PlayerTransform.GetComponent<CharacterController>().enabled = true;
        PlayerTransform.GetComponent<CharacterController>().detectCollisions = true;
    }
}
