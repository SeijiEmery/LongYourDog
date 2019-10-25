using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookData : MonoBehaviour
{
	public float radius = 3.0f;
	public enum RotationAxis { X_axis, Z_axis };
	public RotationAxis rotationAxis = RotationAxis.X_axis;
	public Vector3 hookAxis;
    void Start()
    {
//    	GetComponent<Transform>().localScale = new Vector3(radius,radius,radius);

        var capsule = GetComponent<CapsuleCollider>();
        if (capsule != null) {
	        // cylinder shape: use cylinder's axis for rotation!

	        var axis = transform.rotation * Vector3.up;
	        radius = capsule.radius * Mathf.Max(transform.localScale.x, transform.localScale.z);
	        hookAxis = axis;
        }
        else {
	        switch (rotationAxis)
	        {
		        case RotationAxis.X_axis:
			        hookAxis = Vector3.right;
			        break;
		        case RotationAxis.Z_axis:
			        hookAxis = Vector3.forward;
			        break;
	        }
        }
    }
}
