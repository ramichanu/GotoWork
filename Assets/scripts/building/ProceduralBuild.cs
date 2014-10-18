using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProceduralBuild : MonoBehaviour {

	//Public
	public int buildingCountMax = 0;
	public int floorCountMax = 0;
	public int spaceBetweetBuildings = 20;
	public int verticalRoomPadding = 10;
	public int npcCountMax = 1;
	public string buildingName = "";
	public GameObject property = null;
	public int npcCountPlaceOffice = 0;

	//Private
	GameObject lastFloorStairUp = null;
	GameObject lastFloorStair = null;
	GameObject newBuilding;
	GameObject newHall;
	GameObject newFloor;
	GameObject newRoom;
	GameObject newProperty;
	GameObject newBathRoom;
	GameObject city;
	GameObject newNpc;
	GameObject player;

	float positionY;
	int buildingCount;
	int floorCount;
	int npcCount = 1;
	int officeChairCount = 0;

	
	void Start () { 
		generateBuldings ();
		assignWorkToPlayerAndNpcs ();
	}

	void generateBuldings(){

		city = new GameObject ();
		city.AddComponent ("Room");
		city.transform.tag = "scenary";
		city.transform.name = "city1";


		UnityEngine.Object playerObject = Resources.Load (Utils.PREFAB_CHARACTER_FOLDER + "player");
		player = Instantiate(playerObject, transform.position= new Vector2(-7.4F, -5.9F), transform.rotation) as GameObject;

		GameObject camera = GameObject.FindWithTag ("MainCamera");
		camera.GetComponent<Camera> ().target = player.transform;


		buildingCount = Random.Range (20, buildingCountMax);
		bool needCompaniesToNpcs = false;
		int buildingType = 0;

		for (int i=0; i<buildingCount; i++) {

			positionY = 0;
			buildingType = 0;

			if(needCompaniesToNpcs == false)
			{
				int randomCompanyOrResidence = Random.Range(0,2);
				if((randomCompanyOrResidence == 1) && (i != 0)) 
				{
					buildingType = 1;
				}
			}else{
				buildingType = 1;
			}

			if(this.npcCountPlaceOffice <= this.npcCount)
			{
				generateOneBuilding(i, buildingType);

			}

			if(i+1 == buildingCount && this.npcCountPlaceOffice <= this.npcCount){
				buildingCount++;
				needCompaniesToNpcs = true;
			}

		}
	}

	void generateOneBuilding(int i, int buildingType = 0){

		if(buildingType == 0){
			buildingType= Random.Range (1, 4);
			this.buildingName = "building" + buildingType;
		}else{
			this.buildingName = "company" + buildingType;
		}

		UnityEngine.Object buildingObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + this.buildingName);
		newBuilding = Instantiate(buildingObject, transform.position= new Vector2(spaceBetweetBuildings*i, positionY), transform.rotation) as GameObject;
		GameObject buildingMainDoor = Utils.getChildWithTag(newBuilding, "door");
		generateOneHall(buildingMainDoor);
		generateFloors();
		newBuilding.transform.parent = city.transform;

	}

	void generateOneHall(GameObject buildingDoor){

		string hallName = "";

		if(this.buildingName == "company1"){
			hallName = "hallCompany1";
		}else{
			hallName = "hall";
		}

		positionY += verticalRoomPadding;
		UnityEngine.Object hall = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + hallName);
		newHall = Instantiate(hall, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newHall.GetComponent<Room>().parentRoom = newBuilding;
		newHall.GetComponent<Room>().building = newBuilding;

		foreach (GameObject hallDoor in Utils.getChildrenWithTag(newHall, "door")){
			if(hallDoor.name == "entrance")
			{
				hallDoor.GetComponent<Door>().connectedDoor = buildingDoor;
				buildingDoor.GetComponent<Door>().connectedDoor = hallDoor;
			}
		}
		lastFloorStairUp = Utils.getChildWithTag (newHall, "stair");
		player.transform.position = lastFloorStairUp.transform.position;
		player.GetComponent<characterValues>().currentRoom = newHall;
		player.transform.parent = newHall.transform;
	}

	void generateFloors(){
		floorCount = Random.Range (1, floorCountMax);
		for (int i=0; i<=floorCount; i++) {
			positionY += verticalRoomPadding;
			if(this.npcCountPlaceOffice <= this.npcCount)
			{
				generateOneFloor(i);
			}else{
				Destroy(lastFloorStair);
			}
		}
	}

	void generateOneFloor(int i){

		UnityEngine.Object floor = null;
		if(this.buildingName == "company1"){
			floor = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "floorCompany1");
		}else{
			floor = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "floor1");
		}


		newFloor = Instantiate(floor, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newFloor.GetComponent<Room>().parentRoom = newHall;
		newFloor.GetComponent<Room>().building = newBuilding;
		GameObject tempLastFloorStairUp = null;

		foreach (GameObject floorStair in Utils.getChildrenWithTag(newFloor, "stair")){
			if(floorStair.name == "stairDown"){
				floorStair.GetComponent<Stair>().connectedStair = lastFloorStairUp;
				lastFloorStairUp.GetComponent<Stair>().connectedStair = floorStair;
			}else if(floorStair.name == "stairUp"){
				lastFloorStair = floorStair;
				if(i == floorCount){
					Destroy(floorStair);
				}else{
					tempLastFloorStairUp = floorStair;
				}
			}
		}
		lastFloorStairUp = tempLastFloorStairUp;

		foreach (GameObject floorDoor in Utils.getChildrenWithTag(newFloor, "door")){
			positionY += verticalRoomPadding;
			generateOneRoom(newFloor, floorDoor);
		}
	}



	void generateOneRoom(GameObject newFloor, GameObject floorDoor){

		int roomType = Random.Range (1, 3);
		UnityEngine.Object roomObject = null;

		if(this.buildingName == "company1")
		{
			roomObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "office");
			this.officeChairCount = 0;
			foreach (GameObject roomItemIn in Utils.getChildrenWithTag((GameObject)roomObject, "officeChair")){
				officeChairCount++;
				
			}
			this.npcCountPlaceOffice += officeChairCount;

		}else{
			roomObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "livingRoom" + roomType);
		}

		newRoom = Instantiate(roomObject, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;

		newRoom.GetComponent<Room>().parentRoom = newFloor;
		newRoom.GetComponent<Room>().building = newBuilding;

		UnityEngine.Object propertyObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "property");
		property = Instantiate(propertyObject, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newRoom.transform.parent = property.transform;

		if (this.buildingName != "company1") {
			addNpcAndAssignHome (newRoom);
		}

		
		foreach (GameObject roomDoor in Utils.getChildrenWithTag(newRoom, "door")){
			if(roomDoor.name == "entrance")
			{
				floorDoor.GetComponent<Door>().connectedDoor = roomDoor;
				roomDoor.GetComponent<Door>().connectedDoor = floorDoor;
			}else if(roomDoor.name == "bath"){
				positionY += verticalRoomPadding;
				generateOneBath(roomDoor);
			}
		}
	}

	void generateOneBath(GameObject roomDoor){
		int bathType = Random.Range (1, 3);
		UnityEngine.Object bathRoomObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "bath" + bathType);
		newBathRoom = Instantiate(bathRoomObject, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;

		newBathRoom.GetComponent<Room>().parentRoom = roomDoor.transform.parent.gameObject;
		newBathRoom.GetComponent<Room>().building = newBuilding;
		Transform property = roomDoor.transform.parent.transform.parent;
		newBathRoom.transform.parent = property;

		foreach (GameObject bathDoor in Utils.getChildrenWithTag(newBathRoom, "door")){
			bathDoor.GetComponent<Door>().connectedDoor = roomDoor;
			roomDoor.GetComponent<Door>().connectedDoor = bathDoor;
		}
	}

	void addNpcAndAssignHome(GameObject room){

		if(this.npcCount <= this.npcCountMax){
			UnityEngine.Object npcObject = Resources.Load (Utils.PREFAB_CHARACTER_FOLDER + "npc");
			GameObject sofa = null;
			foreach (GameObject roomItemIn in Utils.getChildrenWithTag(room, "object")){
				
				if(roomItemIn.name == "sofa")
				{
					sofa = roomItemIn;
				}
				
			}

			if(sofa != null)
			{
				newNpc = Instantiate(npcObject, transform.position= sofa.transform.position, transform.rotation) as GameObject;
				newNpc.name = "npc" + this.npcCount;
				newNpc.GetComponent<characterValues>().currentRoom = room;
				newNpc.transform.parent = room.transform;
				newNpc.renderer.sortingOrder = 1;
				newNpc.GetComponent<characterValues>().home = room.transform.parent.gameObject;
				newNpc.GetComponent<characterValues>().homeCheckPoint = sofa;
				this.npcCount++;

			}

		}
	}

	void generateOneCompany(int i)
	{
		UnityEngine.Object buildingObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "company1");
		newBuilding = Instantiate(buildingObject, transform.position= new Vector2(spaceBetweetBuildings*i, positionY), transform.rotation) as GameObject;
		GameObject buildingMainDoor = Utils.getChildWithTag(newBuilding, "door");
		generateOneHall(buildingMainDoor);
		generateFloors();
		newBuilding.transform.parent = city.transform;
	}
	void assignWorkToPlayerAndNpcs(){

		List<GameObject> npcs = new List<GameObject>(GameObject.FindGameObjectsWithTag("npc"));
		List<GameObject> officeChairs = new List<GameObject>(GameObject.FindGameObjectsWithTag("officeChair"));

		int randomOfficeChairs = Random.Range(0, officeChairs.Count-1);
		player.GetComponent<characterValues> ().workCheckPoint = officeChairs.ElementAt(randomOfficeChairs);
		officeChairs.RemoveAt (randomOfficeChairs);


		foreach (GameObject npc in npcs) {
			randomOfficeChairs = Random.Range(0, officeChairs.Count-1);
			npc.GetComponent<characterValues> ().workCheckPoint = officeChairs.ElementAt(randomOfficeChairs);
			officeChairs.RemoveAt (randomOfficeChairs);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
