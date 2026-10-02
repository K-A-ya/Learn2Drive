using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("Settings")]
	// antenna cam type style
    public Transform car;
	public float distance = 6.4f;
	public float height = 1.4f;

	[Header("Damping")] // camera "suspension"
	public float Rotation_Damping = 3.0f;
	public float Height_Damping = 2.0f;

	[Header("Zoom Settings")]
	public float Zoom_Ratio = 0.5f;
	public float FOV = 60f;
	// 60<FOV<80
	// dont do more than 120, will break and 90 needs debug 45 is no better than telescope and 70 breaks 1080p
	private Vector3 rotationVector;

	void LateUpdate(){
		float wantedAngle = rotationVector.y; //maf
		float wantedHeight = car.position.y + height;
		float myAngle = transform.eulerAngles.y;
		float myHeight = transform.position.y;

		myAngle = Mathf.LerpAngle(myAngle, wantedAngle, Rotation_Damping*Time.deltaTime); //more maf
		myHeight = Mathf.Lerp(myHeight, wantedHeight, Height_Damping*Time.deltaTime);

		Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);
		transform.position = car.position;
		transform.position -= currentRotation * Vector3.forward*distance;

		Vector3 temp = transform.position; //temporary variable so Unity doesn't complain
		temp.y = myHeight;
		transform.position = temp;
		transform.LookAt(car);
	}

	void FixedUpdate(){
		Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);
		if (localVelocity.z < -0.1f){
			Vector3 temp = rotationVector; // temporary variables seem to be removed after a closing bracket "}" we can use the same variable name multiple times!!!!!!!!!!!!!!!!!!!!!
			temp.y = car.eulerAngles.y + 180;
			rotationVector = temp;
		}

		else{
			Vector3 temp = rotationVector;
			temp.y = car.eulerAngles.y;
			rotationVector = temp;
		}

		//Setting the field of view of the camera again(ig):
		float acc = car.GetComponent<Rigidbody>().velocity.magnitude;
		GetComponent<Camera>().fieldOfView = FOV + acc * Zoom_Ratio * Time.deltaTime;  //h removd * Time.deltaTime but it works better if you leave it like this ig bc unity complains
	}
}
