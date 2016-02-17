using UnityEngine;
using System.Collections;

public class Propeller : Thruster {

	public Engine engine;
	//public float thrust;
	public float diameter;
	public float pitch;
	//public AerialPhysics phys;
	public GameObject[] normal;
	public GameObject[] blurry;
	public GameObject[] broken;

	public Rigidbody myBody;
	// Use this for initialization
	void Start () {
		//myRenderer = gameObject.GetComponent<Renderer>();
		if (gameObject.GetComponentInParent<Rigidbody>() != null){
			myBody = gameObject.GetComponentInParent<Rigidbody>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (engine.rpm>0){
			transform.Rotate(Vector3.up, 360 * Time.deltaTime * (engine.rpm/60));

			float in_diameter = diameter/2.54f;
			float in_pitch = diameter/2.54f;
			thrust = (4.3924e-8f * engine.rpm * Mathf.Pow(in_diameter, 3.5f))/Mathf.Sqrt(in_pitch)*(4.23333e-4f * engine.rpm * in_pitch - myBody.velocity.z); 

		}
		if (engine.rpm > 350 && blurry[0].activeSelf == false){
			for (int i = 0; i < normal.Length; i++) {
				if ( !broken[i].activeSelf ){
					normal[i].SetActive(false);
					blurry[i].SetActive(true);
				}
			}
			//blurry.enabled = true;
		//	myRenderer.enabled = false;
		}
		else if (engine.rpm <= 350 && blurry[0].activeSelf == true){
			//blurry.enabled = false;
			//myRenderer.enabled = true;
			for (int i = 0; i < normal.Length; i++) {
				if ( !broken[i].activeSelf ){
					normal[i].SetActive(true);
					blurry[i].SetActive(false);
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
