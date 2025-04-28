using UnityEngine;

public class KillHandling : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            GameDetail.Instance.Lose();
        }
    }
}
