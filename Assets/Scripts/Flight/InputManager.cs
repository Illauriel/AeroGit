using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	public Ballast last;
	//public ForcesManager forc;
	CameraController cam;
	public Balloon balloon;
	public Engine eng;
	public bool const_mode;
	ConstructionMain constr;
	// Use this for initialization
	void Start () {
		if (cam == null){
			cam = Camera.main.gameObject.GetComponent<CameraController>();
		
		}

		constr = cam.gameObject.GetComponent<ConstructionMain>();
		if (constr != null){
			const_mode = true;
		}
		if (balloon == null){
			GameObject ball_obj = GameObject.Find("Balloon");
			if(ball_obj != null){
				balloon = ball_obj.GetComponent<Balloon>();
			}

		}
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

			if (!const_mode){
			//Gas Pressure Control
			if (Input.GetKey(KeyCode.X)){
				balloon.gas_vol -= Time.deltaTime*30;
				if (balloon.gas_vol < 0f){
					balloon.gas_vol = 0f;
				}
			}
			if (Input.GetKey(KeyCode.C)){
				balloon.gas_vol += Time.deltaTime*30;
				if (balloon.gas_vol > balloon.max_vol){
					balloon.gas_vol = balloon.max_vol;
				}
			}
			//Ballast
			if (Input.GetKeyDown(KeyCode.Space)){
				last.Drop();
			}

			//RPM Control
			if (Input.GetKeyDown(KeyCode.LeftShift)){

				/*Debug.Log("Shift");
				foreach(Engine x in forc.engines){
					x.GearUp();
				}*/
				eng.GearUp();
			}
			
			if (Input.GetKeyDown(KeyCode.LeftControl)){
				/*foreach(Engine x in forc.engines){
					x.GearDown();
				}*/
				eng.GearDown();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1)){
				//forc.engines[0].GearUp();
				eng.GearUp();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)){
				//forc.engines[1].GearUp();
				eng.GearDown();
			}
			//Burner pshhh
			if (Input.GetKeyDown(KeyCode.B)){

			} 
		}
		else {
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
			if (Input.GetKeyDown(KeyCode.Space)){
				constr.mirroring = ! constr.mirroring;
			}
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
}
