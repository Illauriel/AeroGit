using UnityEngine;
using System.Collections;

public class Ballast : MonoBehaviour {


	//public FixedJoint[] sandbags;
	//public ConfigurableJoint[] sandbags;

	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	public void Drop(){
		//if (cur_bag < sandbags.Length){
			Rigidbody rb = GetComponent<Rigidbody>();
			Debug.Log(rb);
			Destroy(GetComponent<FixedJoint>());
			rb.AddForce(-rb.transform.forward*10, ForceMode.Impulse);
			//cur_bag++;
		//}
	}
}
