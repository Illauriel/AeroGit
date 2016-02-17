using UnityEngine;
using System.Collections;

public class DrawRope : MonoBehaviour {


	public Transform[] links;
	LineRenderer line;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer>();
		line.SetVertexCount(links.Length);
		if (links.Length == 0){
			Debug.LogWarning("Achtung No joints linked");
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (links.Length > 0){
			for (int i = 0; i < links.Length; i++) {
				line.SetPosition(i, links[i].position);

			}
		}
		else{
			Debug.Log(gameObject.name);
		}

	}
}
