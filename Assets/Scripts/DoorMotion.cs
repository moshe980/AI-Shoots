using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMotion : MonoBehaviour
{
    public Animator animator;
    public GameObject door_axis;
    public bool isOpen;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        animator = door_axis.GetComponent<Animator>();
        isOpen = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        
            animator.SetTrigger("Open");
            isOpen = true;
            audioSource.Play();
        
    }

    void OnTriggerExit(Collider other)
    {
       
            animator.SetTrigger("Close");
            isOpen = false;
            audioSource.PlayDelayed(0.5f);

        
    }
}