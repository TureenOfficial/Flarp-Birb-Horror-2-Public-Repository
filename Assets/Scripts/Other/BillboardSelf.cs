using UnityEngine;

public class BillboardSelf : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.LookAt(GameObject.Find("Player").transform);
    }
}
