using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour{
    // Start is called before the first frame update
    public float moveSpeed;
    public bool inair;
    private Rigidbody cube;


    void Start(){
        moveSpeed = 1f;
        inair = false;
        cube = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        //simple movement
        transform.Translate(moveSpeed*Input.GetAxis("Horizontal")*
            Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        //jump
        if (!inair){
            if (Input.GetButtonDown("Jump")) {
                cube.velocity = new Vector3(0f, 8f, 0f);
                inair = true; 
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        collision.gameObject.CompareTag("Plane");
        collision.gameObject.CompareTag("SpeedPad");
        inair = false;
    }

}
