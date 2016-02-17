using UnityEngine;
using System.Collections;

public class Ballast : MonoBehaviour {


	public FixedJoint[] sandbags;
	//public ConfigurableJoint[] sandbags;
	public int cur_bag;
	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	public void Drop(){
		if (cur_bag < sandbags.Length){
			Rigidbody rb = sandbags[sandbags.Length-1-cur_bag].GetComponent<Rigidbody>();
			Debug.Log(rb);
			Destroy(sandbags[sandbags.Length-1-cur_bag]);
			rb.AddForce(-rb.transform.forward*10, ForceMode.Impulse);
			cur_bag++;
		}
	}
}
