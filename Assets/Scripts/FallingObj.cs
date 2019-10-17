using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObj : MonoBehaviour{

    private Rigidbody ball;
    private bool steppedon = false;

    // Start is called before the first frame update
    void Start(){
        ball = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
      
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            steppedon = true;
            ball.useGravity = true;
            ball.isKinematic = false;
        }
    }
}
