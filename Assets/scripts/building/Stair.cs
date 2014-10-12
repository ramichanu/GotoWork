using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stair : MonoBehaviour {
	
	public List<GameObject> connectedStairs;
	
	// Los dos siguientes atributos no seran necesarios
	GameObject character;
	public GameObject connectedStair;
	bool charFrontStair= false;
	
	// Use this for initialization
	void Start () {

		connectedStairs = new List<GameObject>();
		connectedStairs.Add(connectedStair);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (charFrontStair == true) {
			GameObject stairToConnect = (GameObject)connectedStairs[0];
			Vector3 connectedStairPosition = new Vector3(stairToConnect.transform.position.x, stairToConnect.transform.position.y-(float)0.286, 0);

			GameObject stairToConnectParent = stairToConnect.transform.parent.gameObject;
			character.transform.position = connectedStairPosition;
			character.gameObject.GetComponent<characterValues>().currentRoom = stairToConnect.transform.parent.gameObject;

			if(stairToConnect.transform.parent.tag == "building")
			{
				stairToConnect = stairToConnect.transform.parent.transform.parent.gameObject;
			}
			
			character.gameObject.GetComponent<characterValues>().currentRoom = stairToConnectParent;
			character.transform.parent = stairToConnectParent.transform;
			charFrontStair = false;
		} 

		
		
		
	}

	void use (GameObject character) {
		
		if (isColliding(character)) {
			if(character.transform.tag == "Player")
			{
				this.character = character;
				charFrontStair = true;
			}

		} 
	}

	bool isColliding(GameObject character){
		return renderer.bounds.Intersects(character.renderer.bounds);
	}


}
