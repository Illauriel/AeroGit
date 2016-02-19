﻿using UnityEngine;
using System.Collections;

public class FuelContainer : MonoBehaviour {

	public enum ResType{Gasoline, Hydrogen, Water};
	public ResType resType;
	public float volume;
	float unit_weight;
	public AudioSource audio;
	Rigidbody my_body;

	void Start(){
		float[] wts = new float[]{0.75f, 0.071f, 1f};
		unit_weight = wts[(int) resType];
		my_body = GetComponent<Rigidbody>();
		my_body.mass += volume * unit_weight;
	}

	public void SpendFuel (float amount){
		volume -= amount;
		my_body.mass -= amount * unit_weight;
	}
}