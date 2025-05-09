using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class CosmeticsApply : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Material[] materialArray;
    [SerializeField] Material PlayerMat;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    
    void Awake()
    {
        print(PlayerPrefs.GetInt("currentCosmetic", 0));
        PlayerMat = Player.GetComponent<Material>();
        meshRenderer = Player.GetComponent<SkinnedMeshRenderer>();


        int CosmeticIDCode = PlayerPrefs.GetInt("currentCosmetic", 0);

        switch(CosmeticIDCode)
        {
            case 0:
            {
                PlayerDetails.PlayerSpeedMultiplier = 1;
                PlayerDetails.PlayerRaycastDistance = 5f;
                PlayerDetails.PlayerStaminaMultiplier = 1;
                PlayerDetails.PlayerGravityMultiplier = 1;
                break;
            }
            case 1:
            {
                PlayerDetails.PlayerSpeedMultiplier = 1.5f;
                PlayerDetails.PlayerRaycastDistance = 5f;
                PlayerDetails.PlayerStaminaMultiplier = 0.5f;
                PlayerDetails.PlayerGravityMultiplier = 1.2f;
                break;
            }
            case 2:
            {
                PlayerDetails.PlayerSpeedMultiplier = 0.85f;
                PlayerDetails.PlayerRaycastDistance = 2f;
                PlayerDetails.PlayerStaminaMultiplier = 2f;
                PlayerDetails.PlayerGravityMultiplier = 0.9f;
                break;
            }
            case 3:
            {
                PlayerDetails.PlayerSpeedMultiplier = 0.95f;
                PlayerDetails.PlayerRaycastDistance = 7f;
                PlayerDetails.PlayerStaminaMultiplier = 0.75f;
                PlayerDetails.PlayerGravityMultiplier = 0.75f;
                break;
            }
        }

    }
    void Update()
    {
        PlayerMat = materialArray[PlayerPrefs.GetInt("currentCosmetic", 0)];
        meshRenderer.material = PlayerMat;
    }
}
