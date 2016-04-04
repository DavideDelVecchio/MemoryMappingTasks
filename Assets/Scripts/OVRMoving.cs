using UnityEngine;
using System.Collections;

public class OVRMoving : MonoBehaviour {

    public GameObject cameraRig;
    public float speed = 0.7f;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("LeftJoystickX") > 0)
        {
            Vector3 moveDirection = Camera.main.transform.forward;
            moveDirection *= speed * Time.deltaTime;
            moveDirection.y = 0.0f;
            transform.position += moveDirection;
        }
	}
}
