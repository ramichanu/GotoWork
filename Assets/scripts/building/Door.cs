using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

	public List<GameObject> connectedDoors;
	Animator doorAnimator;

	// Los dos siguientes atributos no seran necesarios
	GameObject character;
	public GameObject connectedDoor;
	bool charFrontDoor = false;

	// Use this for initialization
	void Start () {
	

		doorAnimator = this.GetComponent<Animator> ();

		connectedDoors = new List<GameObject>();
		connectedDoors.Add(connectedDoor);
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp ("up") && charFrontDoor) {
			doorAnimator.SetTrigger("openDoor");
		} 


		AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
	
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.OpenDoor"))
		{

			GameObject doorToConnect = (GameObject)connectedDoors[0];
			Vector2 connectedDoorPosition = new Vector2(doorToConnect.transform.position.x, doorToConnect.transform.position.y-(float)0.286);

			character.transform.position = connectedDoorPosition;


		}


			
	}
	void OnTriggerExit2D(Collider2D other) {
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
	}
}
