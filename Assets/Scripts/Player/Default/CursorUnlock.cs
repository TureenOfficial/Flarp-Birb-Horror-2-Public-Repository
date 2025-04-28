using UnityEngine;

public class CursorUnlock : MonoBehaviour
{
    void Start()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    } 
}
