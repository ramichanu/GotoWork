using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

	public List<GameObject> connectedDoors;
	public Animator doorAnimator;
	public AnimatorStateInfo stateInfo;

	// Los dos siguientes atributos no seran necesarios
	public GameObject connectedDoor;
	GameObject character;
	bool charFrontDoor = false;

	// Use this for initialization
	void Start () {
	

		doorAnimator = this.GetComponent<Animator> ();
		//stateInfo = doorAnimator.GetCurrentAnimatorStateInfo (0);

		connectedDoors = new List<GameObject>();
		connectedDoors.Add(connectedDoor);
	}

	// Update is called once per frame
	void Update () {	
		stateInfo = doorAnimator.GetCurrentAnimatorStateInfo (0);
		moveCharacter ();
	}

	void use (GameObject character) {
		
		if (isColliding(character)) {
			this.character = character;
			doorAnimator.SetTrigger("openDoor");
		} 
	}

	void moveCharacter(){
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.OpenDoor"))
		{
			
			GameObject doorToConnect = (GameObject)connectedDoors[0];
			Vector2 connectedDoorPosition = new Vector2(doorToConnect.transform.position.x, doorToConnect.transform.position.y-(float)0.286);
			
			character.transform.position = connectedDoorPosition;
			character.gameObject.GetComponent<characterValues>().currentRoom = doorToConnect.transform.parent.gameObject;
			
			
		}
	}

	bool isColliding(GameObject character){
		return renderer.bounds.Intersects(character.renderer.bounds);
	}
	/*void OnTriggerExit2D(Collider2D other) {
		character = other.gameObject;
		if (character.tag == Utils.PLAYER_TAG) { 
			charFrontDoor = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		character = other.gameObject;
		if (character.tag == Utils.PLAYER_TAG) {  
			charFrontDoor = true;

		} 
	}*/
}
