using UnityEngine;
using System.Collections;
using GameJolt.API;
using GameJolt.UI;
using GameJolt.API.Objects;
using TMPro;
using UnityEngine.UI;

public class GameJoltScript : MonoBehaviour
{
    [SerializeField] GameObject LogInButton;
    [SerializeField] GameObject UserButton;
    [SerializeField] GameObject LogOutButton;
    [SerializeField] bool playerSignedIn = false;
    [SerializeField] string playerName;


    void Update()
    {
        playerSignedIn = GameJoltAPI.Instance.HasSignedInUser;
        if(!playerSignedIn)
        {
            LogInButton.SetActive(true);
            UserButton.SetActive(false);
            LogOutButton.SetActive(false);
        }
        else if(playerSignedIn)
        {
            playerName = GameJoltAPI.Instance.CurrentUser.Name;
            TMP_Text playerNameText = UserButton.GetComponentInChildren<TMP_Text>();

            playerNameText.text = playerName;
             
            UserButton.SetActive(true);
            LogInButton.SetActive(false);
            LogOutButton.SetActive(true);
        }

        
    }

    public void LogOut()
    {
        GameJoltAPI.Instance.CurrentUser = null;
    }

    public void LogIn()
    {
        GameJoltUI.Instance.ShowSignIn();
    }
    public void Trophies()
    {
        GameJoltUI.Instance.ShowTrophies();
    }
}
