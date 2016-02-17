using UnityEngine;
using System.Collections;

public class ConstRope : MonoBehaviour {

	public bool reciever;
	public Transform target;
	public Material mat;
	LineRenderer lr;
	Vector3 hit;
	// Use this for initialization
	void Start () {
		if (! reciever){
			lr = gameObject.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			lr.material = mat;
			lr.SetPosition(0, transform.position);
			lr.SetWidth(0.02f, 0.02f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null && !reciever){
			lr.enabled = true;
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, target.transform.position);
		}

		else if (reciever && lr != null){
			lr.enabled = false;
		}
		else if (target == null){
			lr.enabled = false;
		}
		/*
		if (){
			Destroy lr
		}*/
	}
	void OnDestroy (){
		if (!reciever && target != null){
			Destroy(target);
		}
	}
	/*void OnMouseUp(){
		ConstConnect cc = gameObject.GetComponent<ConstConnect>();
		if (cc != null && target == null){

		}
	}*/
}
