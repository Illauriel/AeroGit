using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.IO;

public class SaveModel : MonoBehaviour {

	string savepath;

	// Use this for initialization
	void Start () {
		savepath = Application.dataPath+"/Saves";
		if ( !Directory.Exists(savepath)){
			Debug.LogWarning("Creating new save directory");
			Directory.CreateDirectory(savepath);
		}
		if (!File.Exists(savepath+"/"+"testsave"+".txt")){
			StreamWriter sw = new StreamWriter(savepath+"/"+"testsave"+".txt");
			sw.WriteLine("This file is empty");
			sw.Close();
		}
	}


	void Save(string filename){
		string save_text = "";
		GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();//UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		foreach (GameObject x in allObjects){
			//Debug.Log(x.name);
			if (x.activeInHierarchy){ //if it's active in hierarchy
				ConstrItem item = x.GetComponent<ConstrItem>();
				ConstConnect connector = x.GetComponent<ConstConnect>();
				ConstRope rope = x.GetComponent<ConstRope>();
				if (item != null){
					save_text += item.item_id + "|" + item.spawnedItem; // name of the item to spawn
					//Saving position and rotation;
					Vector3 pos = x.transform.position;
					Vector3 rot = x.transform.rotation.eulerAngles;
					save_text += "|" + pos.x + "," + pos.y + "," + pos.z;
					save_text += "|" + rot.x + "," + rot.y + "," + rot.z;

					if (connector != null){
						ConstrItem connectedItem = connector.connectedTransform.gameObject.GetComponent<ConstrItem>();
						if (connectedItem == null){
							Debug.Log(connector.gameObject.name + " is missing a connected item!");
						}
						save_text += "|" + connectedItem.item_id;
					}
					if (rope != null){
						if (rope.target != null){
							save_text += "|" + rope.target.GetComponent<ConstrItem>().item_id;
							Debug.Log("Connected rope anchor");
						}
						else {
							Debug.Log("RopeGen "+rope.gameObject.name + " " + x.name+ " has no linked items");
						}
					}
					save_text += "\n";
				}

			}
		}

		File.WriteAllText(savepath+"/"+filename+".txt", save_text);
		Debug.Log("Savegame "+filename + " saved!");
		//AssetDatabase.ImportAsset(savepath+"/"+filename+".txt");
		GameObject.Find("Savecrutch").GetComponent<Savecrutch>().savetext = save_text;
	}

	public void StartGame(){
		Save ("testsave");
		Application.LoadLevel("FlightDayNEW");
	}
	/*void OnGUI(){
		if (GUI.Button(new Rect(10,10,100, 25), "Save")){
			Save ("testsave");
		}
	}*/
}
