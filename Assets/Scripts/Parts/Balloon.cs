using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	public float p_gas; //balloon gas density
	public float gas_vol; //balloon gas volume
	public float max_vol; //balloon volume

	public float lift;

	public GameObject envelope;

	public float size = 2;
	public float min_size;
	public float max_size;

	Rigidbody myBody;

	public Atmosphere atm;
	ConfigurableJoint cj_a;
	//MassObject myMass;

	void Start (){
		myBody = gameObject.GetComponent<Rigidbody>();
		Debug.Log(myBody.gameObject.name);


		//ConfigurableJoint cj_a = null;
		//ConfigurableJoint cj_b = null;

		/*if (envelope != null){
			/*GameObject hinge = new GameObject("GasCellHinge");
			hinge.transform.position = transform.position;
			Rigidbody h_body = hinge.AddComponent<Rigidbody>();*/

		/*	//cj_a = hinge.AddComponent<ConfigurableJoint>();
			cj_a = gameObject.AddComponent<ConfigurableJoint>();
			//cj_b = envelope.AddComponent<ConfigurableJoint>();

			cj_a.xMotion = ConfigurableJointMotion.Limited;
			cj_a.yMotion = ConfigurableJointMotion.Limited;
			cj_a.zMotion = ConfigurableJointMotion.Limited;

			/*cj_b.xMotion = ConfigurableJointMotion.Locked;
			cj_b.yMotion = ConfigurableJointMotion.Locked;
			cj_b.zMotion = ConfigurableJointMotion.Locked;*/

		/*	cj_a.connectedBody = envelope.GetComponent<Rigidbody>();
			//cj_a.connectedBody = myBody;
			//cj_b.connectedBody = h_body;
		} */
		if (max_vol <= 0){
			max_vol = 0.01f;
		}
	}

	void Update (){
		if (atm != null){
			lift = atm.CalculateBouyancy(gameObject.transform.position.y, p_gas, gas_vol);
		}

		//Debug.Log(lift/myBody.mass);

		size = max_size * (gas_vol/max_vol);
		if (size > max_size){
			size = max_size;
		}
		else if (size < min_size){
			size = min_size;
		}
		//gameObject.transform.localScale = new Vector3(size, size, size);
		/*SoftJointLimit sjl = new SoftJointLimit();
		//SoftJointLimitSpring spr = new SoftJointLimitSpring();

		sjl.limit = (max_size - size)*0.52f;
		cj_a.linearLimit = sjl;
		//spr.damper = 1;
		//cj_a.linearLimitSpring = spr; */

	}
	void FixedUpdate(){
		LiftBalloon(lift);
	}
	void LiftBalloon(float acceleration){
		myBody.AddForce(new Vector3(0,acceleration,0), ForceMode.Force);
		//myBody.velocity += Physics.gravity+ new Vector3(0, acceleration, 0);
	}
}
