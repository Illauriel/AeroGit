﻿using UnityEngine;
using System.Collections;

public class LevelInitiator : MonoBehaviour {

	public TextAsset save_file;
	public string savetext;
	public ItemCatalogue data;
	public string[] lines;
	public GameObject[] hierarchy;

	int[] indices;
	Balloon[] balloons;
	Ballast[] ballasts;
	Engine[] engines;
	FuelContainer[] gas_cont;
	FuelContainer[] hydro_cont;
	FuelContainer[] water_cont;

	Propeller[] props;

	public CapsuleCollider[] floor;

	InputManager inp;
	Atmosphere atm;

	// Use this for initialization
	void Start () {
		GameObject crutch = GameObject.Find("Savecrutch");
		if (crutch != null){
			savetext = crutch.GetComponent<Savecrutch>().savetext;
		}
		//lines = save_file.text.Split('\n');
		lines = savetext.Split('\n');

		indices = new int[7];
		inp = GameObject.Find("GameController").GetComponent<InputManager>();
		atm = GameObject.Find("GameController").GetComponent<Atmosphere>();

		CreateBuild();
		//save_file = Resources.Load("Saves/testsave.txt") as TextAsset;


	}

	void CreateBuild(){
		hierarchy = new GameObject[lines.Length-1];
		for (int i = 0; i < lines.Length-1; i++) {
			string[] substrings = lines[i].Split('|');
			int place = int.Parse(substrings[0]);
			GameObject temp_obj = data.play_items[FindEntry(substrings[1])];
			Vector3 pos = transform.position + ReadVector(substrings[2]);
			Quaternion rot = Quaternion.Euler(ReadVector(substrings[3]));
			Debug.Log(i+ ") adds to place " + place);
			hierarchy[place] = (GameObject) Instantiate(temp_obj, pos, rot);

			Balloon tmp_ball = hierarchy[place].GetComponent<Balloon>();
			Ballast tmp_last = hierarchy[place].GetComponent<Ballast>();
			Engine  tmp_engn = hierarchy[place].GetComponent<Engine>();
			FuelContainer tmp_fuel = hierarchy[place].GetComponent<FuelContainer>();
			Propeller tmp_prop = hierarchy[place].GetComponent<Propeller>();

			if (tmp_ball != null){
				indices[0]++;
				tmp_ball.GetComponentInChildren<Cloth>().capsuleColliders = floor;
			}
			else if (tmp_last != null){
				indices[1]++;
			}
			else if (tmp_engn != null){
				indices[2]++;
			}
			else if (tmp_fuel != null){
				switch (tmp_fuel.resType){
				case FuelContainer.ResType.Gasoline: indices[3]++; break;
				case FuelContainer.ResType.Hydrogen: indices[4]++; break;
				case FuelContainer.ResType.Water: indices[5]++; break;
				}
			}
			else if (tmp_prop != null){
				indices[6]++;
			}
		}
		balloons = new Balloon[indices[0]];
		ballasts = new Ballast[indices[1]];
		engines = new Engine[indices[2]];
		gas_cont = new FuelContainer[indices[3]];
		hydro_cont = new FuelContainer[indices[4]];
		water_cont = new FuelContainer[indices[5]];
		props = new Propeller[indices[6]];
		for (int i = 0; i < indices.Length; i++) {
			
		
			indices[i] = 0;
		}



		//establish Connections
		for (int i = 0; i < lines.Length-1; i++) {
			string[] substrings = lines[i].Split('|');
			int place = int.Parse(substrings[0]);

			if (substrings.Length > 4){
				FixedJoint fj = hierarchy[place].AddComponent<FixedJoint>();
				int connect_id = int.Parse(substrings[4]);
				Debug.Log ("Connecting " + hierarchy[place].name +" to "+hierarchy[connect_id]);
				fj.connectedBody = hierarchy[connect_id].GetComponent<Rigidbody>();
				//fj.breakForce = 5100f;
				if (substrings.Length > 5){
					RopeGen rope = hierarchy[place].GetComponent<RopeGen>();
					int anchor_id = int.Parse(substrings[5]);
					Debug.Log(hierarchy[place].name + " " + rope);
					Debug.Log(hierarchy[anchor_id]);
					if (rope != null){
					rope.target = hierarchy[anchor_id].transform;
					}
					else {
						Debug.Log("No rope attached to " + hierarchy[place]);
					}

				}
			}
			Balloon tmp_ball = hierarchy[place].GetComponent<Balloon>();
			Ballast tmp_last = hierarchy[place].GetComponent<Ballast>();
			Engine  tmp_engn = hierarchy[place].GetComponent<Engine>();
			FuelContainer tmp_fuel = hierarchy[place].GetComponent<FuelContainer>();
			Propeller tmp_prop = hierarchy[place].GetComponent<Propeller>();

			if (tmp_ball != null){
				balloons[indices[0]] = tmp_ball;
				balloons[indices[0]].atm = atm;
				indices[0]++;
			}
			else if (tmp_last != null){
				ballasts[indices[1]] = tmp_last;
				indices[1]++;
			}
			else if (tmp_engn != null){
				engines[indices[2]] = tmp_engn;
				engines[indices[2]].atm = atm;
				indices[2]++;
			}
			else if (tmp_fuel != null){
				switch (tmp_fuel.resType){
				case FuelContainer.ResType.Gasoline: gas_cont[indices[3]] = tmp_fuel; indices[3]++; break;
				case FuelContainer.ResType.Hydrogen: hydro_cont[indices[4]] = tmp_fuel; indices[4]++; break;
				case FuelContainer.ResType.Water: water_cont[indices[5]] = tmp_fuel; indices[5]++; break;
				}
			}
			else if (tmp_prop != null){
				props[indices[6]] = tmp_prop;
				Destroy(GetComponent<FixedJoint>());
				//Destroy(GetComponent<Rigidbody>());
				//GetComponent<Rigidbody>().isKinematic = true;


				indices[6]++;
			}

		}

		inp.balloons = balloons;
		inp.last = ballasts;
		inp.engines = engines;
		inp.gas_cont = gas_cont;
		inp.hydro_cont = hydro_cont;
		inp.water_cont = water_cont;

		foreach (Propeller x in props){
			if (engines.Length > 0){
				x.engine = engines[0];
				//x.transform.parent = engines[0].transform;
				HingeJoint hj = x.gameObject.AddComponent<HingeJoint>();
				hj.axis = Vector3.up;
			}
			else{
				x.transform.parent = hierarchy[0].transform;
			}
		}

		Camera.main.gameObject.GetComponent<CameraController>().target = hierarchy[0].transform;
		atm.measured_obj = hierarchy[0];
	}

	int FindEntry(string id){
		int result = -1;
		for (int i = 0; i < data.item_names.Length; i++) {
			if (data.item_names[i] == id){
				result = i;
				Debug.Log("Entry named \""+id+"\" found" );
				break;
			}
		}
		if (result == -1){
			Debug.LogError("Entry named \""+id+"\" not found" );
		}
		return result;
	}

	Vector3 ReadVector(string str){
		string[] numbers = str.Split(',');
		//Debug.Log(numbers[0] + "," + numbers[1] + "," + numbers[2]);
		return new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2]));
	}
}
