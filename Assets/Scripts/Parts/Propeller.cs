using UnityEngine;
using System.Collections;

public class Propeller : Thruster {

	public Engine engine;
	//public float thrust;
	public float diameter;
	public float pitch;
	//public AerialPhysics phys;
	public Renderer blurry;
	public Renderer myRenderer;
	public Rigidbody myBody;
	// Use this for initialization
	void Start () {
		myRenderer = gameObject.GetComponent<Renderer>();
		myBody = gameObject.GetComponentInParent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (engine.rpm>0){
			transform.Rotate(Vector3.up, 360 * Time.deltaTime * (engine.rpm/60));

			float in_diameter = diameter/2.54f;
			float in_pitch = diameter/2.54f;
			thrust = (4.3924e-8f * engine.rpm * Mathf.Pow(in_diameter, 3.5f))/Mathf.Sqrt(in_pitch)*(4.23333e-4f * engine.rpm * in_pitch - myBody.velocity.z); 

		}
		if (engine.rpm > 350){
			blurry.enabled = true;
			myRenderer.enabled = false;
		}
		else{
			blurry.enabled = false;
			myRenderer.enabled = true;
		}
	}

	void LateUpdate(){
		myBody.AddForceAtPosition(-transform.up * thrust, transform.localPosition);
		Debug.DrawLine(transform.localPosition, -transform.up * thrust);
		Debug.Log(-transform.up * thrust);
	}
}
