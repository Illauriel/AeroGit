using UnityEngine;
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

	InputManager inp;

	// Use this for initialization
	void Start () {
		GameObject crutch = GameObject.Find("Savecrutch");
		if (crutch != null){
			savetext = crutch.GetComponent<Savecrutch>().savetext;
		}
		//lines = save_file.text.Split('\n');
		lines = savetext.Split('\n');

		indices = new int[3];
		inp = GameObject.Find("GameController").GetComponent<InputManager>();

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

			if (tmp_ball != null){
				indices[0]++;
			}
			else if (tmp_last != null){
				indices[1]++;
			}
			else if (tmp_engn != null){
				indices[2]++;
			}
		}
		balloons = new Balloon[indices[0]];
		//Smth about ballast
		engines = new Engine[indices[2]];
		indices[0] = 0;
		indices[2] = 0;

		//establish Connections
		for (int i = 0; i < lines.Length-1; i++) {
			string[] substrings = lines[i].Split('|');
			int place = int.Parse(substrings[0]);

			if (substrings.Length > 4){
				FixedJoint fj = hierarchy[place].AddComponent<FixedJoint>();
				int connect_id = int.Parse(substrings[4]);
				fj.connectedBody = hierarchy[connect_id].GetComponent<Rigidbody>();
				fj.breakForce = 1200f;
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
			
			if (tmp_ball != null){
				balloons[indices[0]] = tmp_ball;
				indices[0]++;
			}
			else if (tmp_last != null){
				indices[1]++;
			}
			else if (tmp_engn != null){
				engines[indices[2]] = tmp_engn;
				indices[2]++;
			}


		}

		inp.balloons = balloons;
		inp.engines = engines;
	}

	int FindEntry(string id){
		int result = -1;
		for (int i = 0; i < data.item_names.Length; i++) {
			if (data.item_names[i] == id){
				result = i;
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
