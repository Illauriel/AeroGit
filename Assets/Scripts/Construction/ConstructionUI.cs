using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class ConstructionUI : MonoBehaviour {
	public ItemCatalogue data;

	public Button[] control;
	public Button[] categories;
	public Button[] items;

	public GameObject[,] item_list;
	public GameObject[] cur_items;
	public int cur_cat;
	public int[] cat_sizes;

	public Animator ani;

	ConstructionMain main;
	// Use this for initialization
	void Start () {
		main = Camera.main.GetComponent<ConstructionMain>();
		item_list = new GameObject[6,8];
		cat_sizes = new int[6];
		for (int i = 0; i < data.categories.Length; i++) {
			int cat = data.categories[i];
			item_list[cat, cat_sizes [cat]] = data.const_items[i];
			cat_sizes [cat] ++; //increase the sizeof appropriate category
			Debug.Log("Nesting item "+ data.item_names[i]+" in category "+cat);

		}


	}
	
	// Update is called once per frame
	void Update () {
		if (main.current_obj != null){
			ani.SetBool("Hidden", true);
		}
		else{
			ani.SetBool("Hidden", false);
		}

	}

	void OnGUI(){
		if (main.delete_mode){
			GUI.Box(new Rect(-5,-5, Screen.width * 0.3f, Screen.height+10), "Drop to delete");
		}
		else if (!main.delete_mode && main.current_obj != null) {
			GUI.Box(new Rect(-5,-5, Screen.width * 0.05f, Screen.height+10), "d \ne \nl \ne \nt \ne ");
		}
	}

	public void ChangeIcons(int category){
		cur_cat = category;
		foreach(GameObject x in cur_items){
			Destroy(x);
		}
		cur_items = new GameObject[cat_sizes[cur_cat]];
		foreach (Button x in items){
			x.interactable = false;
		}
		for (int i = 0; i < cur_items.Length; i++) {
			cur_items[i] = (GameObject) Instantiate(item_list[cur_cat, i], Vector3.zero, Quaternion.identity);
			items[i].interactable = true;
			cur_items[i].transform.parent = items[i].transform;
			cur_items[i].transform.localPosition = Vector3.zero;
			ConstrItem this_item = cur_items[i].gameObject.GetComponent<ConstrItem>();
			Renderer renderer = this_item.renderers[0];
			Bounds bounds = renderer.bounds;
			float xtent = Mathf.Max(new float[]{bounds.extents.x,bounds.extents.y, bounds.extents.z});
			float height_coef = (items[i].GetComponent<RectTransform>().rect.height / 1.5f / 2 ) / xtent;
			Debug.Log(renderer.name+ " "+cur_items[i].transform.position+" "+ bounds + " " + height_coef );
			cur_items[i].transform.localPosition += (cur_items[i].transform.position - bounds.center) * height_coef;
			cur_items[i].transform.rotation = transform.rotation;
			cur_items[i].transform.localScale = Vector3.one * 2.5f / xtent;
			cur_items[i].layer = 0;
			Destroy(this_item);
			Destroy(cur_items[i].gameObject.GetComponentInChildren<Collider>());
			Destroy(cur_items[i].gameObject.GetComponent<ConstRope>());
		}
	}

	public void SelectItem(int index){
		GameObject go = (GameObject) Instantiate(item_list[cur_cat, index]);
		go.GetComponent<ConstrItem>().item_id = main.item_no;
		main.item_no ++;

		//StartCoroutine(SkipFrame());//WaitForSeconds (0.1f); 
		main.PickUpPart(go);

	}
	//IEnumerator SkipFrame(){
		//yield return new WaitForSeconds(0.5f);
	//}
	public void StartGame(){

	}
}
