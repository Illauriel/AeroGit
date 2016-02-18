using UnityEngine;
using System.Collections;

public class Propeller : Thruster {

	public Engine engine;
	//public float thrust;
	public float diameter;
	public float pitch;
	//public AerialPhysics phys;
	public float rpm;
	public GameObject[] normal;
	//public GameObject[] blurry;
	public GameObject[] broken;
	public GameObject discus;
	//bool disc;

	//AudioSource audi;

	public Rigidbody myBody;
	float velocity;
	// Use this for initialization
	void Start () {
		//myRenderer = gameObject.GetComponent<Renderer>();
		if (gameObject.GetComponentInParent<Rigidbody>() != null){
			myBody = gameObject.GetComponentInParent<Rigidbody>();
		//	audi = GetComponent<AudioSource> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (engine != null){
			rpm = engine.rpm;
			velocity = myBody.velocity.z;
		}
		else{
			if (rpm > 0){
				rpm -= 50 * Time.deltaTime;
			}
			else if (rpm < 0) {
				rpm = 0;
			}
		}
		if (rpm>0){
			transform.Rotate(Vector3.up, 360 * Time.deltaTime * (rpm/60));

			float in_diameter = diameter/2.54f;
			float in_pitch = diameter/2.54f;
			thrust = (4.3924e-8f * rpm * Mathf.Pow(in_diameter, 3.5f))/Mathf.Sqrt(in_pitch)*(4.23333e-4f * rpm * in_pitch - velocity); 

		}
		if (rpm > 350 && discus.activeSelf == false){
			for (int i = 0; i < normal.Length; i++) {
				if ( !broken[i].activeSelf ){
					normal[i].SetActive(false);
					if (discus == null){
						discus.SetActive(true);
					}
					else{
						discus.SetActive(true);
					}
				}
			}
			//blurry.enabled = true;
		//	myRenderer.enabled = false;
		}
		else if (rpm <= 350 && discus.activeSelf == true){
			//blurry.enabled = false;
			//myRenderer.enabled = true;
			for (int i = 0; i < normal.Length; i++) {
				if ( !broken[i].activeSelf ){
					normal[i].SetActive(true);
					if (discus == null){
						discus.SetActive(false);
					}
					else{
						discus.SetActive(false);
					}
				}
			}
		}

	}

	void LateUpdate(){
		//myBody.AddForceAtPosition(-transform.up * thrust, transform.localPosition);
		Debug.DrawLine(transform.localPosition, -transform.up * thrust);
		//Debug.Log(-transform.up * thrust);
	}
}
