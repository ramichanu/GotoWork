using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public GameObject parentRoom;

	public int getRoomDepth(){
		if(parentRoom == null){
			return 0;
		}else{
			return 1 + parentRoom.GetComponent<Room>().getRoomDepth();
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
