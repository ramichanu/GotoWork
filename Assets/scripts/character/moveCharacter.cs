using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class moveCharacter : MonoBehaviour {
	
	GameObject from;
	GameObject target;
	public bool follow = false;
	public Stack<GameObject> finalPath;
	public float velocity = 3;

	protected float pathFinderRefresh = 0.33f;
	protected float acumulatedTime = 0;
	protected GameObject gameManager;

	public void executeScript (ArrayList parameters) {
		target = (GameObject)parameters [1];
		from = (GameObject)parameters [0];
		gameManager = GameObject.FindWithTag ("gameManager");
		from.AddComponent ("PathFinder");

		from.GetComponent<PathFinder> ().enabled = true;
		from.GetComponent<PathFinder> ().target = target;
		from.GetComponent<PathFinder> ().from = from;
		from.GetComponent<PathFinder> ().SendMessage ("UpdatePath");

		finalPath = from.GetComponent<PathFinder> ().finalPath;
	}

	void Update(){

		/*if (follow) {
			acumulatedTime += Time.deltaTime;
			if (acumulatedTime >= pathFinderRefresh) {
				acumulatedTime = acumulatedTime - pathFinderRefresh;
				if(gameManager != null)
				{
					gameManager.GetComponent<PathFinder> ().SendMessage ("UpdatePath");		
					finalPath = gameManager.GetComponent<PathFinder> ().finalPath;
				}
				
			}
		}*/

		movingCharacter();

	}

	void movingCharacter(){
		if (from != null && target != null && finalPath.Count != 0) {
			GameObject targetObject = finalPath.Peek();
			bool sameRoom = from.transform.parent == target.transform.parent;
			float fromX = from.transform.position.x;
			float targetX = 0f;
			if(targetObject.transform.tag == "door"){
				targetX = targetObject.transform.GetComponent<Door>().connectedDoor.transform.position.x;
			}else if(targetObject.transform.tag == "stair"){
				targetX = targetObject.transform.GetComponent<Stair>().connectedStair.transform.position.x;
			}else{
				targetX = targetObject.transform.position.x;
			}

			if (targetObject != null) {

				if((Mathf.Round(fromX) > Mathf.Round(targetX)))
				{
					Vector3 pos = from.transform.position;
					pos.x -= velocity * Time.deltaTime;
					from.transform.position = pos;
					
					
				}else if((Mathf.Round(from.transform.position.x) < Mathf.Round(targetX))){
					Vector3 pos = from.transform.position;
					pos.x += velocity * Time.deltaTime;
					from.transform.position = pos;
					
				}else if((Mathf.Round(from.transform.position.x) == Mathf.Round(targetX))){
					if(targetObject.transform.tag == "door" || targetObject.transform.tag == "stair")
					{
						from.transform.position = targetObject.transform.position;
						GameObject parentWithRoom = Utils.getFirstParentWithComponent(targetObject, "Room");
						from.transform.parent = parentWithRoom.transform;
						from.GetComponent<characterValues>().currentRoom = parentWithRoom;
					}
					finalPath.Pop ();
					targetObject = null;

					if(Mathf.Round(from.transform.position.x) == Mathf.Round(target.transform.position.x))
					{
						Hashtable options = new Hashtable();
						options.Add("from", from);
						NotificationCenter.DefaultCenter.PostNotification(this, "scriptSuccess", options);
					}

				}
			}
		}
	}
}
