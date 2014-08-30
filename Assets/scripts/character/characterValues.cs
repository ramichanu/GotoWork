using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class characterValues : MonoBehaviour {

	public GameObject door;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("up")) {
			Debug.Log ("Player width: " + this.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x);

			GameObject mainDoorRoom = door.transform.parent.gameObject;
			Debug.Log ("Profundidad habitacion pueta jugador: " + mainDoorRoom.GetComponent<Room>().getRoomDepth());
			Debug.Log (mainDoorRoom.GetComponent<SpriteRenderer>().bounds.extents.x);

			List<GameObject> doorsArray = door.GetComponent<Door>().connectedDoors;
			foreach(GameObject connectedDoor in doorsArray){
				GameObject room = connectedDoor.transform.parent.gameObject;
				Debug.Log ("Profundidad habitacion puertas enlazadas: " + room.GetComponent<Room>().getRoomDepth());
				Debug.Log (room.GetComponent<SpriteRenderer>().bounds.extents.x);
				
				foreach (Transform child in room.transform){
					GameObject childDoor = child.gameObject;
					if(child.tag == "door" && childDoor != connectedDoor)
					{
						Debug.Log (childDoor.name);
					}
				}
			}
		} 
	}
}
