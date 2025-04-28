using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using System;
using UnityEngine.Events;


public class Trigger : MonoBehaviour
{
    //Property of Chronoware Studios

    [Header("Carry Out: ")]
    [SerializeField] UnityEvent OnDeactivate;
    [SerializeField] UnityEvent OnActivate;
    [SerializeField] UnityEvent OnPing;
    [SerializeField] GameObject[] DeactivationObjects;
    [SerializeField] GameObject[] ActivationObjects;

    [Header("Only For Both: ")]
    [SerializeField] bool ForActivation;

    [Header("Only For Ping: ")]
    [SerializeField] string PingText;
    [SerializeField] float OnScreenTimePing;

    [HideInInspector]
    public int styleIndex;
    [HideInInspector]
    public string[] Style = new string[] {"Activate", "Deactivate", "Both", "InfoPing"};


    public void OnTriggerEnter(Collider other)
    {
        switch(styleIndex)
        {
            case 0: //ACTIVATE 
            {
                OnActivate.Invoke();
                foreach(GameObject gameObject in ActivationObjects)
                {
                    gameObject.SetActive(true);
                }
                break;
            }
            case 1:
            {
                OnDeactivate.Invoke();
                foreach(GameObject gameObject in DeactivationObjects)
                {
                    gameObject.SetActive(false);
                }
                break;
            }
            case 2:
            {
                OnActivate.Invoke();
                OnDeactivate.Invoke();

                if(ForActivation) //ACTIVATES OBJECTS, DEACTIVATES OBJECTS
                {
                    foreach(GameObject gameObject in ActivationObjects)
                    {
                        gameObject.SetActive(true);
                    }
                    foreach(GameObject gameObject in DeactivationObjects)
                    {
                        gameObject.SetActive(false);
                    }
                }
                else //DEACTIVATES OBJECTS, ACTIVATES OBJECTS
                {
                    foreach(GameObject gameObject in ActivationObjects)
                    {
                        gameObject.SetActive(false);
                    }
                    foreach(GameObject gameObject in DeactivationObjects)
                    {
                        gameObject.SetActive(true);
                    }
                }
                break;
            }
            case 3:
            {
                OnPing.Invoke();
                InfoPing.Instance.Ping(PingText, OnScreenTimePing, true);
                break;
            }
        }
    }
}
