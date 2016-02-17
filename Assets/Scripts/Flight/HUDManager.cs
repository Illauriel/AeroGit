using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HUDManager : MonoBehaviour {

	 Atmosphere atm;
	 Wind wind;

	public Image compass;
	//Atmosphere
	public Image atm_arrow;
	public Image wind_dir; 
	public Image wind_st;
	public Image atm_temp;

	public Text[] alt;
	public Text[] speed;

	public Image ball_ico;
	public Image[] winds;

	void Awake () {
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = 45;
	}

	// Use this for initialization
	void Start () {
		if (atm == null){
			atm = GameObject.Find("GameController").GetComponent<Atmosphere>();
		}
		if (wind == null){
			wind = atm.gameObject.GetComponent<Wind>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		float atm_rot = ArrowRotation(atm.pressure, 33, 101, -90, 90);
		atm_arrow.rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, atm_rot));

		atm_temp.rectTransform.anchoredPosition = new Vector2(0, FluidLvl(atm.temperature-273.15f, -40,40,40));
		SetCounter(atm.measured_obj.transform.position.y, alt);
		Vector3 vel = atm.measured_obj.GetComponent<Rigidbody>().velocity;
		Vector2 spd = new Vector2(vel.x, vel.z); 
		//Debug.Log(spd);
		SetCounter(spd.magnitude * 3.6f, speed);
		if (wind.enabled){
			ball_ico.enabled = true;
			wind_dir.enabled = true;
			float minimap_alt = (431-19) * (atm.measured_obj.transform.position.y/7000);
			ball_ico.rectTransform.anchoredPosition = new Vector2(-19, 19 + minimap_alt);
			wind.cur_wind = wind.GetWindAtAltitude(atm.measured_obj.transform.position.y);
			wind_dir.rectTransform.localRotation = CompassRotation(wind.cur_wind);
			for (int i = 0; i < winds.Length; i++) {
				winds[i].enabled = true;
				winds[i].color = wind.cols[i];
				winds[i].rectTransform.rotation = CompassRotation(wind.streams[i]);
			}
		}
		else{
			ball_ico.enabled = false;
			wind_dir.enabled = false;
			for (int i = 0; i < winds.Length; i++) {
				winds[i].enabled = false;
			}
		}
	}

	float ArrowRotation(float value, float min_val, float max_val, float min_angle, float max_angle){
		float result = 0;
		float degrees = max_angle - min_angle; // get arrow total amplitude
		float unit = degrees/(max_val - min_val); //amount of units in one degree
		result = (value - min_val) * unit;
		result += min_angle;
		return result;
	}
	Quaternion CompassRotation(Vector3 direction){
		Quaternion stream_dir = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		Quaternion result = Quaternion.Euler(new Vector3(0, 0, -stream_dir.eulerAngles.y));
		return result;
	}

	float FluidLvl(float value, float min_val, float max_val, float vert_size){
		float result = 0;
		float unit = (max_val-min_val)/vert_size;
		result = (value - max_val) / unit;
		return result;
	}
	void SetCounter(float value, Text[] txt){
		string str = Mathf.RoundToInt(value) + "";
		int digits = txt.Length-1;
		for (int i = digits; i >= 0; i--) {
			if (digits-i < str.Length){
				txt[i].text = ""+str[str.Length-1-(digits-i)];
			}
			else {
				txt[i].text = 0+"";
			}
		}
	}

}
