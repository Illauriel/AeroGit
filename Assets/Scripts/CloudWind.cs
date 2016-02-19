using UnityEngine;
using System.Collections;

public class CloudWind : MonoBehaviour {

	Wind wind;
	ParticleSystem shuriken;
	bool set;
	// Use this for initialization
	void Start () {
		wind = GameObject.Find("GameController").GetComponent<Wind>();
		shuriken = gameObject.GetComponent<ParticleSystem>();

		//transform.rotation = Quaternion.Euler(0, 0, myRotation.eulerAngles.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (!set){
			Vector3 my_wind = wind.GetWindAtAltitude(transform.position.y);
			Quaternion myRotation = Quaternion.LookRotation(my_wind);
			//transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, myRotation.y);
			transform.rotation = myRotation;
			//Debug.Log (myRotation.eulerAngles);
			set = true;
			//Debug.Log(transform.rotation.eulerAngles);
		}
	}
}
