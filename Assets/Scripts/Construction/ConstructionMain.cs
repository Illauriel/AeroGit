using UnityEngine;
using System.Collections;

public class ConstructionMain : MonoBehaviour {

	private Plane plane;
	public GameObject grid;
	public GameObject current_obj;

	public GameObject hit_obj;
	//bool on_obj;
	//public bool connects;
	public ConstrItem current_item; //held obj renderer
//	public Collider currentCollider;
	public bool mirroring;
	public GameObject mirror_obj;
	public ConstrItem mirr_item;
	public int item_no;
	public Vector3 rot_offset;
	public GameObject ropeend;

	public bool delete_mode;
	// Use this for initialization
	void Start () {
		plane = new Plane(Vector3.forward, Vector3.zero);

	}

	// Update is called once per frame
	void Update () {
		//plane.SetNormalAndPosition(

		if (current_obj != null){
			/*if (Physics.Raycast(ray, out hit)){
				if (hit.transform.gameObject == grid){
					Debug.Log("Right On Spot captn!");
					current_obj.transform.position = hit.point;
				}
			}*/
			//RaycastHit hit = new RaycastHit();
			//if (ProbeUranus(out hit)){
				//current_obj.transform.position = hit.point;
			//}
			if (current_item.colliders[0].enabled){
				Debug.LogError("SOMETNING ACTIVATED THIS COLLIDER! WTF");
			}
			RaycastHit hit = new RaycastHit();
			if (ProbeUranus(out hit) && current_item.connectType != ConstrItem.Connection.NoConnection /*&& connects*/){
				current_obj.transform.position = hit.point;
				CorrectItemRotation(current_obj.transform, -hit.normal);

				if (mirroring){
					HandleMirroring();
					RaycastHit mirr_hit = new RaycastHit();
					Vector3 dir = hit.transform.position - mirror_obj.transform.position;
					Debug.DrawRay(mirror_obj.transform.position - dir, dir, Color.red);
					Physics.Raycast(mirror_obj.transform.position - dir, dir, out mirr_hit);
					CorrectItemRotation(mirror_obj.transform, -mirr_hit.normal); 

					mirr_item.tint = current_item.tint;
				}
				else if (!mirroring && mirror_obj != null){
					Destroy(mirror_obj);
				}
				current_item.tint = new Color(0, 0.5f, 0.2f, 0.3f);
			}
			else{
				current_item.tint = new Color(1, 1, 1, 0.5f);
				if (mirror_obj != null){
					Destroy(mirror_obj);
					item_no --;
				}

				PlaceOnPlane();
			}

			if (Input.GetMouseButtonDown(0)){
				ReleasePart();
			}

		}
		else {
			if (Input.GetMouseButtonDown(0)){
			
				RaycastHit hit = new RaycastHit();
				if (ProbeUranus(out hit)){
					PickUpPart(hit.transform.gameObject);
				}
			}
		}

		// Item deletion
		if (current_obj != null && !delete_mode && Input.mousePosition.x < Screen.width * 0.05f){
			delete_mode = true;
		}
		else if (delete_mode && Input.mousePosition.x > Screen.width * 0.3f){
			delete_mode = false;
		}
		else if (delete_mode && Input.GetMouseButtonDown(0)){
			DestroyPart();
		}
		/*else {
			delete_mode = false;
		}*/
	}

	public void PickUpPart (GameObject part){
		//Debug.Log("Found Object: "+ hit.transform.gameObject.name + "Captn!");
		current_obj = part;

		//Find root;
		if (current_obj.transform.parent != null){
			for (int i = 0; i < 10; i++) { //You won't make an item 10 children deep?
				current_obj = current_obj.transform.parent.gameObject;
				if (current_obj.transform.parent == null){
					break;
				}
			}
		}

		ConstConnect cc = current_obj.GetComponent<ConstConnect>();
		if (cc != null){
			cc.enabled = false;
			Destroy(cc);
		}

		Gondola gnd = current_obj.GetComponent<Gondola>();
		Balloon bal = current_obj.GetComponent<Balloon>();
		current_item = current_obj.GetComponent<ConstrItem>();
		current_item.EnableFadeMode();
		if (current_item.colliders.Length >0){
			foreach (Collider x in current_item.colliders){
				x.enabled = false;
				Debug.Log("ALL COLLIDERS DISABLED COMMANDER!");
			}
		}
		else {
			Debug.LogError("The collider array you refer to is empty!");
		}


		current_item.tint = new Color(1, 1, 1, 0.5f);
	}

	void SelectPart (GameObject part){

	}

	void ReleasePart(){
		ConstRope rope = current_obj.GetComponent<ConstRope>();
		if (hit_obj != null && current_item.connectType != ConstrItem.Connection.NoConnection /*connects*/){


			ConnectPart(current_obj);
			if (mirror_obj != null){
				ConnectPart(mirror_obj);
				HandleItemRelease(mirror_obj);

				mirror_obj = null;
				mirr_item = null;
			}
			hit_obj = null;
		
			foreach (Collider x in current_item.colliders){
				x.enabled = true;
			}


		}
		HandleItemRelease(current_obj);
		current_obj = null;
		current_item = null;
		
		if (rope != null && !rope.reciever && rope.gameObject.GetComponent<ConstConnect>() != null ){

			GameObject second = (GameObject) Instantiate(ropeend, rope.transform.position, rope.transform.rotation);



			PickUpPart(second);
			rope.target = current_obj.transform;
			second.GetComponent<ConstrItem>().item_id = item_no;
			item_no ++;

		}



	}


	void HandleItemRelease(GameObject obj){
		ConstrItem temp_item = obj.GetComponent<ConstrItem>();
		temp_item.DisableFadeMode();
		foreach (Collider x in temp_item.colliders){
			x.enabled = true;
		}
		temp_item.tint = Color.white;
	}

	void DestroyPart(){

	}

	void ConnectPart(GameObject part){
		ConstrItem item = part.GetComponent<ConstrItem>();
		ConstConnect cc = part.AddComponent<ConstConnect>();

		cc.connectedTransform = hit_obj.transform.root;
		cc.myColl = item.colliders[0];
		cc.connectedColl = hit_obj.GetComponent<Collider>();
	}

	bool ProbeUranus(out RaycastHit hit){
		hit = new RaycastHit();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int mask = 1 << 10;

		if (Physics.Raycast(ray, out hit, mask)){


			hit_obj = hit.transform.gameObject;

			return true;
		}
		else {
			hit_obj = null;

			return false;
		}
	}

	void PlaceOnPlane(){
		float enter = 0;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(plane.Raycast(ray, out enter)){
			Vector3 intersect = ray.GetPoint(enter);
			if (intersect.magnitude > 100){
				intersect = intersect.normalized * 100;
			}
			current_obj.transform.position = intersect;
			current_obj.transform.rotation = Quaternion.Euler(rot_offset);
		}
	}
	void CorrectItemRotation(Transform item, Vector3 dir){
		Quaternion new_rot = Quaternion.identity;
		Vector3 old_rot = item.rotation.eulerAngles;
		Vector3 look = Quaternion.LookRotation(dir).eulerAngles;
		new_rot = Quaternion.Euler(look+rot_offset);// + Quaternion.Euler(rot_offset);
		item.rotation = new_rot;
	}



	void HandleMirroring(){
		Vector3 mirr_pos = Vector3.Reflect(hit_obj.transform.position - current_obj.transform.position, hit_obj.transform.up);
		Debug.DrawRay(hit_obj.transform.position, hit_obj.transform.up, Color.blue);
		if (mirror_obj == null){


			mirror_obj = (GameObject) Instantiate(current_obj, mirr_pos, Quaternion.identity);
			mirr_item = mirror_obj.GetComponent<ConstrItem>();

			item_no++;
			mirr_item.item_id = item_no;
		}
		else{
			mirror_obj.transform.position = hit_obj.transform.position + mirr_pos;
		}
	}
}
