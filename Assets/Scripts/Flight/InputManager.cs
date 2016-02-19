using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	//public ForcesManager forc;
	CameraController cam;
	public Balloon[] balloons;
	public Ballast[] last;
	public Engine[] engines;
	public FuelContainer[] gas_cont;
	public FuelContainer[] hydro_cont;
	public FuelContainer[] water_cont;
	public bool const_mode;
	ConstructionMain constr;
	int ballast_index;
	// Use this for initialization
	void Start () {
		if (cam == null){
			cam = Camera.main.gameObject.GetComponent<CameraController>();
		
		}

		constr = cam.gameObject.GetComponent<ConstructionMain>();
		if (constr != null){
			const_mode = true;
		}
		/* if (balloons.Length == null){
			GameObject ball_obj = GameObject.Find("Balloon");
			if(ball_obj != null){
				balloon = ball_obj.GetComponent<Balloon>();
			}

		} */
	}
	
	// Update is called once per frame
	void Update () {
		//Camera Control
		if (Input.GetKey(KeyCode.LeftArrow)){
			cam.long_angle -= Time.deltaTime * 60;
		}
		if (Input.GetKey(KeyCode.RightArrow)){
			cam.long_angle += Time.deltaTime * 60;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			cam.lat_angle += Time.deltaTime * 60;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			cam.lat_angle -= Time.deltaTime * 60;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0){
			//Debug.Log("Scroll");
			cam.distance -= Input.GetAxis("Mouse ScrollWheel");
		}

		//if (!const_mode){
		if (balloons.Length > 0){
			//Gas Pressure Control
			if (Input.GetKey(KeyCode.X)){
				foreach(Balloon x in balloons){
					x.gas_vol -= Time.deltaTime*30;
					if (x.gas_vol < 0f){
						x.gas_vol = 0f;
					}
				}
			}
			if (Input.GetKey(KeyCode.C)){
				foreach(Balloon x in balloons){

					//Debug.Log("Pshhhhh");
					if (ChoseContainer(1, Time.deltaTime*30)){
						x.gas_vol += Time.deltaTime*30;
					}
					if (x.gas_vol > x.max_vol){
						x.gas_vol = x.max_vol;
					}
				}
			}
			if (Input.GetKeyUp (KeyCode.C)) {
				foreach (var x in hydro_cont) {
					x.audio.Stop ();
				}
			}
		}
		//Ballast
		if (Input.GetKeyDown(KeyCode.Space)){
			if (ballast_index < last.Length){
				last[ballast_index].Drop();
				ballast_index ++;
			}
			else{
				ChoseContainer(2, Time.deltaTime * 5);
			}
		}
		if (engines.Length > 0){
		//RPM Control
			if (Input.GetKeyDown(KeyCode.LeftShift)){
				foreach(Engine x in engines){
					x.GearUp();
				}
			}
			
			if (Input.GetKeyDown(KeyCode.LeftControl)){
				foreach(Engine x in engines){
					x.GearDown();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha1)){

				engines[0].GearUp();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)){
				//forc.engines[1].GearUp();
				engines[0].GearDown();
			}
		}
			
			//Burner pshhh
			if (Input.GetKeyDown(KeyCode.B)){

			} 
		//}
		if (const_mode){
			if (Input.GetKeyDown(KeyCode.A)){
				constr.rot_offset += Vector3.forward * 90;
			}
			if (Input.GetKeyDown(KeyCode.D)){
				constr.rot_offset += -Vector3.forward * 90;
			} 
			if (Input.GetKeyDown(KeyCode.W)){
				constr.rot_offset += Vector3.right * 90;
			} 
			if (Input.GetKeyDown(KeyCode.S)){
				constr.rot_offset += -Vector3.forward * 90;
			} 
			if (Input.GetKeyDown(KeyCode.Space)){
				constr.rot_offset = Vector3.zero;
			}
			/*if (Input.GetKeyDown(KeyCode.Space)){
				constr.mirroring = ! constr.mirroring;
			}*/
		}
	}
	float ConstrainAngle(float input){
		if (input>=360){
			input -= 360;
		}
		if (input<0){
			input = 360 + input;
		}
		return input;
	}

	bool ChoseContainer(int id, float amount){
		//Debug.Log("Chosing");
		FuelContainer[] containers = new FuelContainer[0];
		switch (id){
		case 0: containers = gas_cont; break;
		case 1: containers = hydro_cont; break;
		case 2: containers = water_cont; break;
		}
		if (containers.Length > 0){
			//Debug.Log(containers[0]);
			for (int i = 0; i < containers.Length; i++) {
				if (containers[i].volume >0){
					//Debug.Log("containers " + containers[i].name + " vol = "+ containers[i].volume);
					containers[i].SpendFuel(amount);
					return true;
					break;
				}
				else if (i == containers.Length-1 && containers[i].volume <= 0){
					Debug.LogWarning("DasWas " +id);
					return false;
				}
				/*else{

				}*/

			}

		}
		/*else{
			return false;
		}*/
		return false;
	}
}
