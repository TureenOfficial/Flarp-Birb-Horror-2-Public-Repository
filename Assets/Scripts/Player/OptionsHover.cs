using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsHover : MonoBehaviour
{
    [SerializeField] TMP_Text DisplayString;
    [SerializeField] string _Text;
    public void DisplayText(string Text)
    {
         _Text = Text;   
    }

    void Update()
    {
        DisplayString.text = _Text;
    }
}
