using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	public const string PREFAB_BUILDING_FOLDER = "prefabs/building/";
	public const string PLAYER_TAG = "Player";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static ArrayList getChildrenWithTag(GameObject parent, string tag){
		ArrayList childrenByTag = new ArrayList ();
		foreach(Transform child in parent.transform){
			if(child.CompareTag(tag)){
				childrenByTag.Add(child.gameObject);
			}
		}
		return childrenByTag;
	}

	public static GameObject getChildWithTag(GameObject parent, string tag){
		GameObject childWithTag = null;
		foreach(Transform child in parent.transform){
			if(child.CompareTag(tag)){
				childWithTag = child.gameObject;
				break;
			}
		}
		return childWithTag;
	}
}
