using UnityEngine;
using System.Collections;

public class ProceduralBuild : MonoBehaviour {

	//Public
	public int buildingCountMax = 10;
	public int floorCountMax = 5;
	public int spaceBetweetBuildings = 20;
	public int verticalRoomPadding = 10;
	public GameObject floor;
	public GameObject hall;
	public GameObject property = null;
	//Private
	GameObject lastFloorStairUp = null;
	GameObject newBuilding;
	GameObject newHall;
	GameObject newFloor;
	GameObject newRoom;
	GameObject newProperty;
	GameObject newBathRoom;

	float positionY;
	int buildingCount;
	int floorCount;
	
	void Start () { 
		generateBuldings ();
	}

	void generateBuldings(){
		buildingCount = Random.Range (1, buildingCountMax);
		for (int i=1; i<=buildingCount; i++) {
			positionY = 0;
			generateOneBuilding(i);
		}
	}

	void generateOneBuilding(int i){
		int buildingType= Random.Range (1, 4);
		UnityEngine.Object buildingObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "building" + buildingType);
		newBuilding = Instantiate(buildingObject, transform.position= new Vector2(spaceBetweetBuildings*i, positionY), transform.rotation) as GameObject;
		GameObject buildingMainDoor = Utils.getChildWithTag(newBuilding, "door");
		generateOneHall(buildingMainDoor);
		generateFloors();
	}

	void generateOneHall(GameObject buildingDoor){
		positionY += verticalRoomPadding;
		newHall = Instantiate(hall, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newHall.GetComponent<Room>().parentRoom = newBuilding;
		foreach (GameObject hallDoor in Utils.getChildrenWithTag(newHall, "door")){
			if(hallDoor.name == "entrance")
			{
				hallDoor.GetComponent<Door>().connectedDoor = buildingDoor;
				buildingDoor.GetComponent<Door>().connectedDoor = hallDoor;
			}
		}
		lastFloorStairUp = Utils.getChildWithTag (newHall, "stair");
	}

	void generateFloors(){
		floorCount = Random.Range (1, floorCountMax);
		for (int i=1; i<=floorCount; i++) {
			positionY += verticalRoomPadding;
			generateOneFloor(i);
		}
	}

	void generateOneFloor(int i){
		newFloor = Instantiate(floor, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newFloor.GetComponent<Room>().parentRoom = newHall;
		GameObject tempLastFloorStairUp = null;
		foreach (GameObject floorStair in Utils.getChildrenWithTag(newFloor, "stair")){
			if(floorStair.name == "stairDown"){
				floorStair.GetComponent<Stair>().connectedStair = lastFloorStairUp;
				lastFloorStairUp.GetComponent<Stair>().connectedStair = floorStair;
			}else if(floorStair.name == "stairUp"){
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
			generateOneRoom(floorDoor);
		}
	}



	void generateOneRoom(GameObject floorDoor){

		int roomType = Random.Range (1, 3);

		UnityEngine.Object roomObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "livingRoom" + roomType);
		newRoom = Instantiate(roomObject, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newRoom.GetComponent<Room>().parentRoom = newFloor;

		UnityEngine.Object propertyObject = Resources.Load (Utils.PREFAB_BUILDING_FOLDER + "property");
		property = Instantiate(propertyObject, transform.position= new Vector2(newBuilding.transform.position.x, positionY), transform.rotation) as GameObject;
		newRoom.transform.parent = property.transform;

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
		Transform property = roomDoor.transform.parent.transform.parent;
		newBathRoom.transform.parent = property;

		foreach (GameObject bathDoor in Utils.getChildrenWithTag(newBathRoom, "door")){
			bathDoor.GetComponent<Door>().connectedDoor = roomDoor;
			roomDoor.GetComponent<Door>().connectedDoor = bathDoor;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
