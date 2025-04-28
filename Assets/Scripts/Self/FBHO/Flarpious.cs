using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Flarpious : MonoBehaviour
{
    
    private NavMeshAgent greatflarp;
    
    public Transform PlayerTarget;


    // Start is called before the first frame update
    void Start()
    {
        greatflarp = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(PlayerTarget.position);
        greatflarp.SetDestination(PlayerTarget.position);
    }
}

