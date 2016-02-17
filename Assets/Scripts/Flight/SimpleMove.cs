using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

	public Rigidbody myBody;
	// Use this for initialization
	void Start () {
		myBody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
			transform.Translate(Vector3.left * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D)){
			transform.Translate(Vector3.right * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.W)){
			transform.Translate(Vector3.up * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S)){
			transform.Translate(Vector3.down * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.Q)){
			myBody.AddForce(Vector3.forward*10000 * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.E)){
			myBody.AddForce(Vector3.back*10000 * Time.deltaTime);
		}
	}
}
