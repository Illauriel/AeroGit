using UnityEngine;
using System.Collections;
using System;

public class ItemCatalogue : ScriptableObject {


	public GameObject[] play_items; //itms for playmode
	public string[] item_names; //ids of items
	public int[] categories; // category that item belongs to
	public GameObject[] const_items; // construction mode items

}
