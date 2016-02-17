using UnityEngine;
using UnityEditor;

public class SoundStoreAsset
{
	[MenuItem("Assets/Create/SoundStore")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<SoundStore> ();
	}
}