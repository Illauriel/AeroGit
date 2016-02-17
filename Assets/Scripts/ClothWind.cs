using UnityEngine;
using System.Collections;

public class ClothWind : MonoBehaviour {

	Wind wind;
	Cloth cloth;
	// Use this for initialization
	void Start () {
		wind = GameObject.Find("GameController").GetComponent<Wind>();
		cloth = gameObject.GetComponent<Cloth>();
	}
	
	// Update is called once per frame
	void Update () {
		cloth.externalAcceleration = wind.cur_wind;
		cloth.randomAcceleration = wind.cur_wind.normalized;
	}
}
