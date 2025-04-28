using System.Collections;
using UnityEngine;
public class SimpleCharacterControl : MonoBehaviour
{
    public float speed = 5f;
    public Camera player_camera;
    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 move = transform.right * hor + transform.forward * ver;
        transform.position += move * speed * Time.deltaTime; 
    }
}
