using UnityEngine;
using System.Collections;
using System;

public class Object : MonoBehaviour {

	public string characterAnimationUse = null;
	public string characterAnimationUnuse = null;
	public string objectAnimation = null;
	bool used = false;
	GameObject usedBy = null;

	bool isColliding(GameObject character){
		return renderer.bounds.Intersects(character.renderer.bounds);
	}

	void use(GameObject character){
		if(used && usedBy != character) return;
		if(!isColliding(character)) return;

		if(!used){
			if(characterAnimationUse != null && characterAnimationUse != ""){
				character.GetComponent<Animator> ().SetTrigger(characterAnimationUse);
			}
			if(objectAnimation != null && objectAnimation != ""){
				this.GetComponent<Animator> ().SetTrigger(objectAnimation);
			}
			used = true;
			usedBy = character;

			this.SendMessage ("executeEffect", character, SendMessageOptions.DontRequireReceiver);
		}else{
			if(characterAnimationUnuse != null && characterAnimationUnuse != ""){
				character.GetComponent<Animator> ().SetTrigger(characterAnimationUnuse);
			}
			used = false;
			usedBy = null;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
