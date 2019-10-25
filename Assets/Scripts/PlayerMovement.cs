using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerController {
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        if (camera == null) {
            camera = Camera.current.GetComponent<FollowCamera>();
        }
    }

    public FollowCamera camera;

    public float jumpSpeed = 10f; // m/s
    public float moveSpeed = 3.5f; // m/s
    public float turnSpeed = 180.0f; // degrees / sec

    // TODO: use this for swinging logic
    public bool grabButtonPressed = false;
    public bool enableMovement = true;

    public AnimationCurve zoomAngleCurve;
    public float minZoomDist = 1.0f;
    public float maxZoomDist = 20.0f;
    public float zoomSpeed = 1.0f;
    public float zoomDist = 10.0f;

    // Update is called once per frame
    void Update() {
        if (enableMovement) {
            transform.Translate(
                (
                    Vector3.forward * moveDir.y +
                    Vector3.right * moveDir.x
                ) * moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, turnDir.x * turnSpeed * Time.deltaTime);
            zoomDist = Mathf.Clamp(
                zoomDist - turnDir.y * zoomSpeed * Time.deltaTime,
                minZoomDist,
                maxZoomDist
            );
        }

        if (camera) {
            var pitchAngle = zoomAngleCurve.Evaluate((zoomDist - minZoomDist) / (maxZoomDist - minZoomDist));
            var offset = Vector3.up * zoomDist * Mathf.Sin(pitchAngle)
                         + Vector3.back * zoomDist * Mathf.Cos(pitchAngle);

            var rotationAngle = transform.rotation.eulerAngles.y;
            var rotatedOffset = Quaternion.AngleAxis(rotationAngle, Vector3.up) * offset;
            camera.SetTargetPos(
                transform.position + rotatedOffset
            );
        }
    }

    private Vector2 moveDir = Vector2.zero;
    private Vector2 turnDir = Vector2.zero;
    private bool jumped = false;

    public void Jump() {
        if (!enableMovement) return;
//        if (!jumped) {
        jumped = true;
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
//        }
    }

    public void UpdatePlayerMovement(Vector2 dir) {
        moveDir = dir;
    }

    public void UpdateCameraMovement(Vector2 dir) {
        turnDir = dir;
    }

    public void OnJumpButtonPressed() {
        Jump();
    }

    public void OnInteractButtonPressed() {
        // TODO: replace this with interaction code (should open dialog)
        Debug.Log("Interact pressed!");
    }

    public void OnCancelButtonPressed() {
        // TODO: can use this for interaction code (could cancel dialog)
        Debug.Log("Cancel pressed!");
    }

    public void OnGrabButtonPressed() {
        grabButtonPressed = true;
        enableMovement = false;
    }

    public void OnGrabButtonReleased() {
        grabButtonPressed = false;
        enableMovement = true;
        if (GetComponent<HingeJoint>() != null) {
            HingeJoint hjOut = GetComponent<HingeJoint>();
            Destroy(hjOut);
/////////////////////////////////////           	/*Restore Normal Movement*/
        }
    }

    public void OnMenuButtonPressed() { }

    //Swinging trigger
    void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Hook")) {
            if (GetComponent<HingeJoint>() == null) {
                if (grabButtonPressed) {
/////////////////////////////////////        		 /*Disable Normal Movement*/
                    HingeJoint hj = gameObject.AddComponent<HingeJoint>() as HingeJoint;
                    hj.anchor = transform.InverseTransformPoint(other.gameObject.transform.position);
                    hj.axis = (other.gameObject.GetComponent<HookData>().hookAxis);
                    Debug.Log(other.gameObject.GetComponent<HookData>().hookAxis);
                }
            }
        }
    }

    // reset player rotation when it hits the ground
    void OnCollisionEnter(Collision collision) {
        transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
    }
}
