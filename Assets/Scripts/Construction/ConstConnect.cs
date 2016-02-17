using UnityEngine;
using System.Collections;

public class ConstConnect : MonoBehaviour {

	public Transform connectedTransform;
	public Vector3 anchor;
	public Collider myColl;
	public Collider connectedColl;

	void Start () {
		//myColl = gameObject.GetComponent<Collider>();
		if (connectedTransform != null){
			anchor = transform.position-connectedTransform.position;
			//public connectedColl = connectedTransform.gameObject.GetComponent<Collider>();
		}
	}
	// Update is called once per frame
	void LateUpdate () {
		if (connectedTransform != null){
			//GameObject parent = connectedTransform
			transform.position = connectedTransform.position + anchor;
			if (myColl.enabled != connectedColl.enabled){
				Debug.Log("Restoring item to state of "+connectedTransform.name);
			}
			myColl.enabled = connectedColl.enabled;
		}
	}


	bool FindRootParent(GameObject parent){
		if (parent.GetComponent<ConstConnect>() == null){
			return true;
		}
		else {
			return false;
		}
	}
}
