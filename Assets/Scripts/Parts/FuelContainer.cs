using UnityEngine;
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
		if (audio != null){
			audio.loop = true;
		}
	}

	public void SpendFuel (float amount){
		Debug.Log("Spending "+ amount + " fuel");
		volume -= amount;
		my_body.mass -= amount * unit_weight;
		if (resType == ResType.Hydrogen && !audio.isPlaying) {
			audio.Play ();
		}
		if (volume <0 && audio != null){
			volume = 0;
			audio.Stop();
		}

	}
}
