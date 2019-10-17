using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
	public float speed = 2.5f;
	public float jumpSpeed = 2.5f;
	public float gravity = 10.0F;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    //Rigidbody rb;
    private float speedVertical = 0.0f;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    	// moveDirection = transform.TransformDirection(moveDirection);
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (controller.isGrounded) {
             speedVertical = 0;
             if (Input.GetButtonDown ("Jump")) {
                 speedVertical = jumpSpeed;
             }
         }
        speedVertical -= gravity * Time.deltaTime;
        moveDirection.y = speedVertical;
        controller.Move(moveDirection * Time.deltaTime * speed);

        // if (Input.GetKey(KeyCode.W))
        //     rb.AddForce(new Vector3(5.0f,0.0f,0.0f));
        // if (Input.GetKey(KeyCode.A))
        //     rb.AddForce(new Vector3(0.0f,0.0f,5.0f));
        // if (Input.GetKey(KeyCode.S))
        //     rb.AddForce(new Vector3(-5.0f,0.0f,0.0f));
        // if (Input.GetKey(KeyCode.D))
        //     rb.AddForce(new Vector3(0.0f,0.0f,-5.0f));
    }
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag ("Pickup"))
        {
            Destroy(other.gameObject);
        }
    }
}
