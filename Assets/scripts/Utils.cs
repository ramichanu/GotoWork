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
		if (parent != null && parent.transform != null) {
			foreach (Transform child in parent.transform) {
				if (child.CompareTag (tag)) {
					childrenByTag.Add (child.gameObject);
				}
				ArrayList childChildren = Utils.getChildrenWithTag (child.gameObject, tag);
				if (childChildren != null)
					childrenByTag.AddRange (childChildren);
			}
		}
		return childrenByTag;
	}

	public static GameObject getFirstParentWithComponent(GameObject gameObject, string componentType){
		GameObject parent = null;
		Transform parentTransform = gameObject.transform.parent;
		if(parentTransform != null)
			parent = gameObject.transform.parent.gameObject;
		if (parent != null) {
			Component component = parent.transform.GetComponent(componentType);
			if(component != null){
				return parent;
			}else{
				parent = Utils.getFirstParentWithComponent(parent, componentType);
			}
		}
		return parent;
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
