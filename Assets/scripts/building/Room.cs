﻿using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public GameObject parentRoom;
	public GameObject actualRoom;
	public GameObject building;

	public int getRoomDepth(){
	if(parentRoom == null){
		return 0;
	}else if(parentRoom.transform.tag == "building"){
			return 1;
	}else{
			return 1 + parentRoom.GetComponent<Room>().getRoomDepth();
		}
	}

	public GameObject getActualRoom(){
		return this.gameObject;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
