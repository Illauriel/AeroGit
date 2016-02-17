using UnityEngine;
using System.Collections;

public class AdminInput : MonoBehaviour {

	public GameObject[] engine;
	public Wind wind;

	public AudioClip mus1;
	public AudioClip mus2;
	public AudioSource sauce;
	// Use this for initialization
	void Start () {
		sauce = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)){
			Application.Quit();
		}
		if (Input.GetKeyUp(KeyCode.Backspace)){
			Application.LoadLevel("1488");
		}
		if (Input.GetKeyUp(KeyCode.Alpha0)){
			for (int i = 0; i < engine.Length; i++) {
				engine[i].SetActive(!engine[i].activeSelf);
			}
		}
		if (Input.GetKeyUp(KeyCode.Alpha9)){
			wind.enabled = !wind.enabled;
		}
		if (Input.GetKeyUp(KeyCode.RightBracket)){
			sauce.clip = mus2;
			sauce.Play();
		}
		if (Input.GetKeyUp(KeyCode.LeftBracket)){
			sauce.clip = mus1;
			sauce.Play();
		}
	}
}
