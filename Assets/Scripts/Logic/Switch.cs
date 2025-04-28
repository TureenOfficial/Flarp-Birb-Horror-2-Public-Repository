using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    [SerializeField] private string Operation;
    [SerializeField] RuntimeUtils playerScript;
    [SerializeField] UnityEvent Event;
    [SerializeField] GameObject[] objectToDeactivate;
    [SerializeField] GameObject[] objectToActivate;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool doesCheck = true;

    [Header("For Ping: ")]
    [SerializeField] string Text;
    [SerializeField] float TimeOn;
    [SerializeField] bool DeactivateAfterPing;

    void Start()
    {
        if(doesCheck)
        {
            foreach (GameObject obj in objectToActivate)
            {
                    obj.SetActive(false);
            }
        }
    }
    void Deactivate_object()
    {
        foreach (GameObject obj in objectToDeactivate)
        {
            obj.SetActive(false);
        }
    }
    void Activate_object()
    {
        foreach (GameObject obj in objectToActivate)
        {
                obj.SetActive(true);
        }
    }
    public void OnButtonClick()
    {
        if(audioSource != null) audioSource.Play();

        switch(Operation.ToLower())
        {
            case "win":
            {
                GameDetail.Instance.Win();
                break;
            }
            case "deactivate_object":
            {
                Deactivate_object();
                break;
            }
            case "activate_object":
            {
                Activate_object();
                break;
            }
            case "both_object":
            {
                Deactivate_object();
                Activate_object();
                break;
            }
            case "event":
            {
                Event.Invoke(); 
                break;
            }
            case "ping":
            {
                InfoPing.Instance.Ping(Text, TimeOn, DeactivateAfterPing);
                break;
            }
            case "ping_event":
            {
                Event.Invoke(); 
                InfoPing.Instance.Ping(Text, TimeOn, DeactivateAfterPing);
                break;
            }
        }
    }
}
