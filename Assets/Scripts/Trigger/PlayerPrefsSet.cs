using UnityEngine;

public class PlayerPrefsSet : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] string Type;
    [SerializeField] string PlayerPref;

    [Header("Value")]
    [SerializeField] string PlayerPrefStringValue;
    [SerializeField] int PlayerPrefIntValue;
    [SerializeField] int PlayerPrefFloatValue;
    
    public void Interaction()
    {
       switch(Type.ToLower())
       {
            case "string":
            {
                PlayerPrefs.SetString(PlayerPref, PlayerPrefStringValue);
                break;
            }
            case "int":
            {
                PlayerPrefs.SetInt(PlayerPref, PlayerPrefIntValue);
                break;
            }   
            case "float":
            {
                PlayerPrefs.SetFloat(PlayerPref, PlayerPrefFloatValue);
                break;
            }
       }
    }
}
