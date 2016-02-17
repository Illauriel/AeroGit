using UnityEngine;
using System.Collections;

public class ConstrItem : MonoBehaviour {

	public int item_id;
	public string spawnedItem;
	public enum Connection {SingleConnection, DoubleConnection, NoConnection, UniversalConnection};
	public Connection connectType;
	//public enum Connectible {Universal, Rotary, None};
	public MeshRenderer[] renderers;
	public Color tint;
	public Material[] materials;
	public Collider[] colliders;
	public Color [] old_tints;
	public GameObject mirror;
	// Use this for initialization
	void Awake () {
		colliders = gameObject.GetComponentsInChildren<Collider>(true);
		materials = new Material[renderers.Length];
		old_tints = new Color[renderers.Length];
		tint = Color.white;
		for (int i = 0; i < renderers.Length; i++) {

			materials[i] = renderers[i].material;//(Material) Instantiate(x.material);
			materials[i].name += "Edited";
			//materials[i].SetFloat("_Mode", 2.0f);
			old_tints[i] = materials[i].color;
			renderers[i].material = materials[i];
		}
		//WriteRenMode();
		//EnableFadeMode( 0f);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < renderers.Length; i++) {
			//Debug.Log ("I " + i+ " equals shit");
			materials[i].color = new Color(tint.r, tint.g, tint.b, tint.a);
			renderers[i].material = materials[i];
		}
	}


	public void EnableFadeMode(){
		for (int i = 0; i < materials.Length; i++) {
			materials[i].SetFloat("_Mode", 2);
			materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			materials[i].SetInt("_ZWrite", 0);
			materials[i].DisableKeyword("_ALPHATEST_ON");
			materials[i].EnableKeyword("_ALPHABLEND_ON");
			materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
			materials[i].renderQueue = 3000;
		}
	}

	public void DisableFadeMode(){
		for (int i = 0; i < materials.Length; i++) {
			materials[i].SetFloat("_Mode", 0);
			materials[i].SetInt("_SrcBlend", 1);
			materials[i].SetInt("_DstBlend", 0);
			materials[i].SetInt("_ZWrite", 1);
			materials[i].EnableKeyword("_ALPHATEST_ON");
			materials[i].DisableKeyword("_ALPHABLEND_ON");
			materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
			materials[i].renderQueue = 2000;
		}
	}
	/*public void WriteRenMode(){
		//for (int i = 0; i < materials.Length; i++) {
		string result = "";
		result += "_Mode " + materials[0].GetFloat("_Mode") + "\n";
		result += "_SrcBlend" + materials[0].GetInt("_SrcBlend") + "\n";
		result += "_DstBlend" + materials[0].GetInt("_DstBlend") + "\n";
		result += "_ZWrite" + materials[0].GetInt("_ZWrite") + "\n";
		//materials[0].DisableKeyword("_ALPHATEST_ON");
		//materials[0].EnableKeyword("_ALPHABLEND_ON");
		//materials[0].DisableKeyword("_ALPHAPREMULTIPLY_ON");
		result += materials[0].renderQueue;// = 3000;
		//}
		Debug.Log(result);
	}*/
}
