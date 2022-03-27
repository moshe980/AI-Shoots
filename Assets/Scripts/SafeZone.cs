using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<FieldOfView>().isSafe = true;
        }

    }

}
