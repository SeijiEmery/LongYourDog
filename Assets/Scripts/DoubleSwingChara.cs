using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DoubleSwingChara : MonoBehaviour
{
	public float speed = 2.5f;
	public float jumpSpeed = 3.5f;
	public float gravity = 8.0F;
	public GameObject otherPlayer;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    Rigidbody rb;
    Transform tf;
    private float speedVertical = 0.0f;
    private int count;
    private bool swinging = false;
    DoubleSwingChara otherScript;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        //rb.freezeRotation = true;
        rb.isKinematic = true;
        otherScript = otherPlayer.GetComponent<DoubleSwingChara>();
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

        //movement
        if (!swinging)
            controller.Move(moveDirection * Time.deltaTime * speed);

        // if (Input.GetKey(KeyCode.W))
        //     rb.AddForce(new Vector3(5.0f,0.0f,0.0f));
        // if (Input.GetKey(KeyCode.A))
        //     rb.AddForce(new Vector3(0.0f,0.0f,5.0f));
        // if (Input.GetKey(KeyCode.S))
        //     rb.AddForce(new Vector3(-5.0f,0.0f,0.0f));
        // if (Input.GetKey(KeyCode.D))
        //     rb.AddForce(new Vector3(0.0f,0.0f,-5.0f));
        if (Input.GetKeyUp(KeyCode.K))
        {
           if (otherPlayer.GetComponent<HingeJoint>()!= null){
           		HingeJoint hjOut = otherPlayer.GetComponent<HingeJoint>();
           		Destroy(hjOut);
           }
        }
    }
    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.CompareTag ("Hook"))
        {
        	if (otherPlayer.GetComponent<HingeJoint>()== null){
        		//.isKinematic = false;
        		if(Input.GetKey(KeyCode.K)/*&& gameObject.CompareTag("Player")*/){
        			//rb.isKinematic = true;
        			otherPlayer.GetComponent<Rigidbody>().isKinematic = false;
	            	swinging = true;
	            	otherScript.swingingOn();
	            	controller.enabled = false;
	            	otherPlayer.GetComponent<CharacterController>().enabled = false;
	            	HingeJoint hj = otherPlayer.AddComponent<HingeJoint>() as HingeJoint;
	            	hj.anchor = otherPlayer.GetComponent<Transform>().InverseTransformPoint(transform.position);
	            	hj.axis = (Vector3.forward);
	            	//rb.freezeRotation = false;
            	}
            }
        }
    }
    void OnCollisionEnter(Collision collision) 
    {
    	if (collision.gameObject.CompareTag("Ground"))
        {
        	swinging = false;
        	tf.rotation = Quaternion.Euler(new Vector3(0,0,0));
        	rb.isKinematic = true;
           	controller.enabled = true;
        }
    }
    void swingingOn()
    {
    	swinging = true;
    }
}
