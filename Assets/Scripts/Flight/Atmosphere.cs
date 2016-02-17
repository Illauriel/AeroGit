using UnityEngine;
using System.Collections;

public class Atmosphere : MonoBehaviour {


	public float lapse = 0.0065f; //temperature lapse rate K/M
	public float id_gas = 8.31447f; //ideal gas constant
	public float p_o = 101.325f; //sealevel air pressure
	public float t_o = 288.15f; // kelvin sea level temperature
	public float mol = 0.0289644f; //molar mass of air
	public float g; //gravity constant
	//public float dens_o = 1.292f;

	public float pressure;
	public float temperature;
	public float density;

	bool updated; //is the atmosphere state updated?
	// Use this for initialization
	public GameObject measured_obj;
	float tick;
	void Start () {
		//g = 9.806f;
		Debug.Log(g);
		//measured_obj = GameObject.Find("Balloon");
	}
	
	// Update is called once per frame
	void Update () {
		if (tick <= 0){
			density = GetDensityAtAltitude(measured_obj.transform.position.y);
			tick = 1;
		}
		else{
			tick -= Time.deltaTime;
		}
	}

	public float GetTemperatureAtAltitude (float alt){
		float result = 0;
		result = t_o - lapse * alt;
		//Debug.Log("Temperature at altitude "+alt+" is "+result+" K/"+(result - 256)+"C");

		return result;
	}
	public float GetPressureAtAltitude (float alt){
		float result = 0;

		float temp_const = 5.25588f;
		float temp_fluct = 2.25577f*Mathf.Pow(10, -5) * alt;

		result = p_o * Mathf.Pow((1 - temp_fluct), temp_const);
		//Debug.Log ((1-temp_fluct)+ " pow " + temp_const + " = "+air_tmp);

		//Debug.Log("Atm. pressure at altitude "+alt+" is "+result+"kPa");
		return result;
	}

	public float GetDensityAtAltitude (float alt){
		float result = 0;


		pressure = GetPressureAtAltitude(alt);
		temperature = GetTemperatureAtAltitude(alt);


		result = (pressure * 1000)/(287.058f * temperature);

		//Debug.Log("Atm. density at altitude "+alt+" is "+result + "kg/m3");
		return result;
	}

	public float CalculateBouyancy(float alt, float p_gas, float gas_vol){
		float result = 0;
		//density = GetDensityAtAltitude(alt);
		
		result = (density - p_gas) * gas_vol * g;
		//Debug.LogWarning(result);
		return result;
	}


	public Vector3 GetAerodynamicDrag (Vector3 velocity, Vector3 coeff, Vector3 area, float alt ){
		Vector3 result = Vector3.zero;
		if (!updated){
			density = GetDensityAtAltitude(alt);
		}
		Vector3 n_vel = TrueNormal(velocity.normalized);

		float cur_c = Vector3.Dot(coeff, n_vel);
		float cur_a = Vector3.Dot(area, n_vel);
		float vel_mag = velocity.magnitude;
		if (n_vel != Vector3.zero){
			Debug.Log (n_vel + " "+ cur_a);
		}
		float drag = (density * Mathf.Pow(vel_mag, 2) * cur_c * cur_a) / 2;

		result = -n_vel * drag;

		return result;
	}

	/*public float LinearEquation(float dens, float vel, float area, float shape){
		//D = 1/2 * dens * u^2 * C_D * A
		return (dens * Mathf.Pow(vel,2) * shape * area) / 2;

	}*/

	public Vector3 TrueNormal(Vector3 normal){
		Vector3 result = Vector3.zero;
		float sum = Mathf.Abs(normal.x) + Mathf.Abs(normal.y) + Mathf.Abs(normal.z);
		if (sum != 0){
			result = new Vector3(normal.x / sum, normal.y / sum, normal.z / sum);
		}
		if (Mathf.Abs(sum) > 1){
			Debug.LogWarning (normal);
			//Debug.Break();
		}
		//Debug.Log(sum);
		return result;
	}
}
