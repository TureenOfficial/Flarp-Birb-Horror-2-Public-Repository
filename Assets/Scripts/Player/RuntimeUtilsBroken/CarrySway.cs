using UnityEngine;

public class CarrySway : MonoBehaviour
{
    [Header("Sway Options")]
    [SerializeField] float smoothness;
    [SerializeField] float swayAmount;

    void Start()
    {
        smoothness = PlayerDetails.CarrySmoothness;
        swayAmount = PlayerDetails.CarrySwayAmount;
    }
    void Update()
    {
        if(Time.timeSinceLevelLoad >= 1)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayAmount;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayAmount;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRot = rotationX * rotationY;    

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, smoothness * Time.deltaTime);
        }
    }
}
