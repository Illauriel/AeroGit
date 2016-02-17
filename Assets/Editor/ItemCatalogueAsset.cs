using UnityEngine;
using UnityEditor;

public class ItemCatalogueAsset
{
	[MenuItem("Assets/Create/ItemCatalogue")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ItemCatalogue> ();
	}
}