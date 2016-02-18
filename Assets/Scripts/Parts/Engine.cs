using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

	public float rpm;
	public float temperature;
	public float[] rpm_stages;
	public int cur_gear;
	public int prev_gear;
	public float engine_dynamic; //rpm change per second
	public int zero_gear;
	public bool stalling;
	public float stall_time;
	public bool accel; //are we on accel or decel?
	public float delta_temp;
	public float critical_temp;

	public GameObject explosion;
	public GameObject fire;
	public GameObject destroyed;

	public Atmosphere atm;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("rpm<trg:" + rpm < target_rpm + "rpm>trg:"+rpm > target_rpm +  "rpm>=low:" + rpm >= rpm_stages[cur_gear-1]);
		//MAKE IT ACCOUNT FOR REVERSE LATER
		if (accel){
			if (rpm < rpm_stages[cur_gear]){
				rpm += engine_dynamic * Time.deltaTime;
			}
			else if (rpm > rpm_stages[cur_gear]){
				rpm = rpm_stages[cur_gear];
			} 
		}
		else{
			if (rpm > rpm_stages[cur_gear]){
				rpm -= engine_dynamic * Time.deltaTime * 3;
			}
			else if (rpm < rpm_stages[cur_gear]){
				rpm = rpm_stages[cur_gear];
			} 
		}

		/*if (cur_gear != zero_gear && rpm > target_rpm && rpm > rpm_stages[cur_gear-1]){
			//target non zero rpm reached;
			Debug.Log ("RPM optimal!"+(rpm > target_rpm && rpm > rpm_stages[cur_gear-1]));
			rpm = target_rpm;


		}
		else if (rpm < target_rpm && rpm >= rpm_stages[cur_gear-1]){
			rpm += engine_dynamic * Time.deltaTime;
			//Debug.Log ("RPM increasing to "+rpm);
		}
		else if (rpm > target_rpm && rpm <= rpm_stages[cur_gear+1]){
			rpm -= engine_dynamic * Time.deltaTime;
			Debug.Log ("RPM Decreasing to "+rpm);
		}
		else if (rpm > target_rpm && rpm > rpm_stages[cur_gear+1]-engine_dynamic){
			cur_gear = zero_gear;
			target_rpm = rpm_stages[zero_gear];
			rpm = 0;
			stalling = true;
			stall_time = 3;
			Debug.LogWarning("ENGINE STALL!");
		}*/
		//| (cur_gear < rpm_stages.Length-1 && rpm < target_rpm && rpm < rpm_stages[cur_gear+1])


		if (stalling){
			stall_time -= Time.deltaTime;
			if (stall_time<0){
				stalling = false;
			}
		}

		if (rpm > 0){
			float rot_ratio = rpm / rpm_stages[rpm_stages.Length-1];
			float roll_x = Random.Range(-15 * rot_ratio, 15 * rot_ratio);
			float roll_y = Random.Range(-15 * rot_ratio, 15 * rot_ratio);
			float roll_z = Random.Range(-15 * rot_ratio, 15 * rot_ratio);

			transform.rotation = Quaternion.Euler(new Vector3(roll_x, roll_y, roll_z));
		}

		float rot_d_temp = (rpm/2 - temperature) * 0.004f;
		float atm_d_temp = ((atm.temperature - 273.15f) - temperature) * atm.density * 0.004f;
		delta_temp = rot_d_temp + atm_d_temp;
		temperature += delta_temp;

		if (temperature > critical_temp){
			ExplodeEngine();
		}
		else if (temperature < -10){
			stalling = true;
		}

		if (atm.density < 0.7f){
			stalling = true;
		}

	}

	public void GearUp(){
		if (!stalling && cur_gear<rpm_stages.Length-1){
			prev_gear = cur_gear;
			cur_gear++;

			if (rpm < rpm_stages[cur_gear]){
				accel = true;
			}
			else {
				accel = false;
			}
			Debug.Log("Gear Up to "+cur_gear);
		}
	}

	public void GearDown(){
		if (!stalling && cur_gear>0){
			prev_gear = cur_gear;
			cur_gear--;
			if (rpm < rpm_stages[cur_gear]){
				accel = true;
			}
			else {
				accel = false;
			}
			Debug.Log("Gear Down to "+cur_gear);
		}
	}

	public void ExplodeEngine(){
		GameObject carcass = (GameObject) Instantiate(destroyed, transform.position, transform.rotation);
		Instantiate(explosion, transform.position, Quaternion.identity);
		GameObject flame = (GameObject) Instantiate(fire, transform.position, Quaternion.identity);
		flame.transform.parent = carcass.transform;
		Destroy(gameObject);

	}
}
