﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PathFinder : MonoBehaviour {

	List<CandidatePath> openList;
	List<CandidatePath> closedList;
	public GameObject target;
	public GameObject from;
	public GameObject currentRoom;
	public GameObject fromBuilding;
	public GameObject targetEntity;
	public GameObject targetRoom;
	List<GameObject> finalPath;

	// Use this for initialization
	void Start () {
		bool finish = false;
		bool sum = true;
		openList = new List<CandidatePath>();
		closedList = new List<CandidatePath>();

		currentRoom = from.GetComponent<characterValues>().currentRoom;
		fromBuilding = currentRoom.GetComponent<Room>().building;
		targetRoom = Utils.getFirstParentWithComponent(target, "Room");
		targetEntity = targetRoom.GetComponent<Room>().building;


		//Primer cop, es l'inicial
		GameObject parentEntity = currentRoom.GetComponent<Room>().building;
		bool sameEntity = (parentEntity == targetEntity);
		int weightRoom;
		if(sameEntity){
			weightRoom = Mathf.Abs(currentRoom.GetComponent<Room>().getRoomDepth() - targetRoom.GetComponent<Room>().getRoomDepth());
		}else{
			weightRoom = currentRoom.GetComponent<Room>().getRoomDepth() + targetRoom.GetComponent<Room>().getRoomDepth();
		}
		GameObject candidateObject = new GameObject ();
		CandidatePath candidate = candidateObject.AddComponent<CandidatePath>();
		candidate.weight = weightRoom;
		candidate.currentWeight = 0;
		int weight = candidate.weight + candidate.currentWeight;
		candidate.keyWeight = weight;
		candidate.previousCandidatePath = null;
		candidate.checkPoint = from;
		List<CandidatePath> initialCandidates = new List<CandidatePath> ();
		initialCandidates.Add(candidate);
		putCandidatesInOpenList (initialCandidates);

		while (true) {
			sortOpenList();
			CandidatePath bestCandidate = getBestCandidateFromOpenList();
			putCandidateInClosedList(bestCandidate);
			if(Utils.getFirstParentWithComponent(bestCandidate.checkPoint, "Room") == targetRoom) break;

			List<CandidatePath> candidates = getCandidatesInPlaceByObject(bestCandidate);
			putCandidatesInOpenList(candidates);
		}

		finalPath = retrievePath ();
	}

	public List<GameObject> retrievePath(){
		List<GameObject> finalPath = new List<GameObject> ();
		CandidatePath path = closedList[closedList.Count-1];
		do {
			if(path.checkPoint.tag != "Player")
				Debug.Log (path.checkPoint.transform.parent.name);
			finalPath.Add(path.checkPoint);
			path = path.previousCandidatePath;
		} while (path != null);
		return finalPath;
	}
	
	public List<CandidatePath> getCandidatesInPlaceByObject(CandidatePath obj){
		GameObject room;
		if (obj.checkPoint.tag == "Player") {
			room = obj.checkPoint.GetComponent<characterValues>().currentRoom;
		} else {
			room = Utils.getFirstParentWithComponent (obj.checkPoint, "Room"); 
		}
		List<CandidatePath> candidates = new List<CandidatePath> ();
		foreach (GameObject childDoor in Utils.getChildrenWithTag(room, "door")) {
			GameObject connectedDoor = childDoor.GetComponent<Door>().connectedDoor;
			GameObject parent = Utils.getFirstParentWithComponent(connectedDoor, "Room");
			GameObject parentEntity = parent.GetComponent<Room>().building;
			bool sameEntity = (parentEntity == targetEntity);

			int weightRoom;
			if(sameEntity){
				weightRoom = Mathf.Abs(parent.GetComponent<Room>().getRoomDepth() - targetRoom.GetComponent<Room>().getRoomDepth());
			}else{
				weightRoom = parent.GetComponent<Room>().getRoomDepth() + targetRoom.GetComponent<Room>().getRoomDepth();
			}

			GameObject candidateObject = new GameObject ();
			CandidatePath candidate = candidateObject.AddComponent<CandidatePath>();
			candidate.weight = weightRoom;
			candidate.currentWeight = obj.currentWeight + 1;
			int weight = candidate.weight + candidate.currentWeight;
			candidate.keyWeight = weight;
			candidate.checkPoint = connectedDoor;
			candidate.previousCandidatePath = obj;

			candidates.Add(candidate);
		}
		foreach (GameObject childStair in Utils.getChildrenWithTag(room, "stair")) {
			GameObject connectedStair = childStair.GetComponent<Stair>().connectedStair;
			GameObject parent = Utils.getFirstParentWithComponent(connectedStair, "Room");
			GameObject parentEntity = parent.GetComponent<Room>().building;
			bool sameEntity = (parentEntity == targetEntity);

			int weightRoom;
			if(sameEntity){
				weightRoom = Mathf.Abs(parent.GetComponent<Room>().getRoomDepth() - targetRoom.GetComponent<Room>().getRoomDepth());
			}else{
				weightRoom = parent.GetComponent<Room>().getRoomDepth() + targetRoom.GetComponent<Room>().getRoomDepth();
			}

			GameObject candidateObject = new GameObject ();
			CandidatePath candidate = candidateObject.AddComponent<CandidatePath>();
			candidate.weight = weightRoom;
			candidate.currentWeight = obj.currentWeight + 1;
			int weight = candidate.weight + candidate.currentWeight;
			candidate.keyWeight = weight;
			candidate.checkPoint = connectedStair;
			candidate.previousCandidatePath = obj;
			candidates.Add(candidate);
		}

		return candidates;
	}

	public void putCandidatesInOpenList(List<CandidatePath> candidates){
		foreach(CandidatePath candidate in candidates){
			if((openList.Any(obj => obj.checkPoint == candidate.checkPoint) == false))
				openList.Add(candidate);
		}
	}

	public CandidatePath getBestCandidateFromOpenList(){
		CandidatePath bestCandidate = (CandidatePath)openList[0];
		return bestCandidate;
	}

	public void putCandidateInClosedList(CandidatePath candidate){
		closedList.Add (candidate);
		openList.Remove (candidate);
	}

	public void sortOpenList(){
		openList = openList.OrderBy(x => x.keyWeight).ToList();
	}

	/*public void setCurrentRoom(CandidatePath bestCandidate){
		string isDoorOrStair = bestCandidate.lastDoorStair.transform.tag;
		currentWeight = bestCandidate.currentWeight;
		if(isDoorOrStair == "door")
		{
			currentRoom = Utils.getFirstParentWithComponent(bestCandidate.lastDoorStair.GetComponent<Door>().connectedDoor,
			                                                "Room");
		}else if(isDoorOrStair == "stair"){
			currentRoom = Utils.getFirstParentWithComponent(bestCandidate.lastDoorStair.GetComponent<Stair>().connectedStair,
			                                                "Room");
		}
	}*/
	
}