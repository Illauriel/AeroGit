using UnityEngine;
using System.Collections;

public class LevelInitiator : MonoBehaviour {

	public TextAsset save_file;
	public string savetext;
	public ItemCatalogue data;
	public string[] lines;
	public GameObject[] hierarchy;

	// Use this for initialization
	void Start () {
		GameObject crutch = GameObject.Find("Savecrutch");
		if (crutch != null){
			savetext = crutch.GetComponent<Savecrutch>().savetext;
		}
		//lines = save_file.text.Split('\n');
		lines = savetext.Split('\n');
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
		}
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

		}
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
