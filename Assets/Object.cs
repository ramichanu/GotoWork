using UnityEngine;
using System.Collections;

public class Object : MonoBehaviour {

	public string characterAnimationUse = null;
	public string characterAnimationUnuse = null;
	public string objectAnimation = null;
	//bool charFront = false;
	bool used = false;
	//GameObject character = null;

	bool isColliding(GameObject character){
		return renderer.bounds.Intersects(character.renderer.bounds);
	}

	void use(GameObject character){
		if(used) return;
		if(characterAnimationUse != null && characterAnimationUse != ""){
			character.GetComponent<Animator> ().SetTrigger(characterAnimationUse);
		}
		if(objectAnimation != null && objectAnimation != ""){
			this.GetComponent<Animator> ().SetTrigger(objectAnimation);
		}
		used = true;
	}

	void unuse(GameObject character){
		if(characterAnimationUnuse != null && characterAnimationUnuse != ""){
			character.GetComponent<Animator> ().SetTrigger(characterAnimationUnuse);
		}
		used = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		/*if (Input.GetKeyUp ("up") && charFront) {
			doorAnimator.SetBool ("openDoor", true);
		} */
	}

	/*void OnTriggerExit2D(Collider2D other) {
		character = other.gameObject;
		if (character.tag == "character") { 
			charFront = false;
			character = null;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		
		character = other.gameObject;
		if (character.tag == "character") {  
			charFront = true;
			
		} 
	}*/
}
