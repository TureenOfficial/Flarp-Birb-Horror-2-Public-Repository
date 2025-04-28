using UnityEngine;
using GameJolt.API;
using System.Collections;
using System;

public class AchievementTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public int trophyCode;


    public void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            if (GameJoltAPI.Instance?.gameObject != null) TryUnlockTrophy(trophyCode);
        }       
    }

    void TryUnlockTrophy(int id, string description = "")
    {
        UnityEngine.Debug.Log($"[GameJolt] Tried to unlock trophy: {description} ({id})");
        Trophies.TryUnlock(id);
    }
}

