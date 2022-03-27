using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    private float speed;
    //private CharacterController characterController;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 180f;



    // Use this for initialization
    void Start()
    {
        speed = 0f;
     //   characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            speed += 0.05f;
        else if (Input.GetKey(KeyCode.S))
            speed -= 0.05f;

        yaw += speedH * Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);

        Vector3 direction = transform.TransformDirection(Vector3.forward * Time.deltaTime * speed);
      //  characterController.Move(direction);
    }
}
