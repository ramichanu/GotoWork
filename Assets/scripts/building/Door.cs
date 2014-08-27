using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour {

	List<GameObject> connectedDoors;
	Animator doorAnimator;

	// Los dos siguientes atributos no seran necesarios
	GameObject character;
	public string connectedDoorId = "door2";

	// Use this for initialization
	void Start () {
	

		doorAnimator = this.GetComponent<Animator> ();

		connectedDoors = new List<GameObject>();
		GameObject door = GameObject.Find(connectedDoorId) ;
		connectedDoors.Add(door);
	}
	
	// Update is called once per frame
	void Update () {
		AnimatorStateInfo stateInfo = doorAnimator.GetCurrentAnimatorStateInfo(0);
	
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.OpenDoor"))
		{

			GameObject connectedDoor = (GameObject)connectedDoors[0];
			Vector2 connectedDoorPosition = new Vector2(connectedDoor.transform.position.x, connectedDoor.transform.position.y-(float)0.286);

			character.transform.position = connectedDoorPosition;


		}
	}

	void OnTriggerStay2D(Collider2D other) {
		character = other.gameObject;
		if(character.tag == "character" && Input.GetKeyUp("up"))
		{  
			doorAnimator.SetBool ("openDoor", true);

		}
	}
}
