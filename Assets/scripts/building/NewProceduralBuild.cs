using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NewProceduralBuild : MonoBehaviour {

	const int RESIDENTIAL_OR_SHOP = 1;
	const int COMPANY = 2;
	const int RESIDENTIAL = 1;
	const int SHOP = 2;
	const int PROPERTIES_PER_FLOOR = 2;
	const int MIN_FLOORS_PER_PROPERTY_BUILDING = 1;
	const int MIN_FLOORS_PER_OFFICE_BUILDING = 1;
	const int OFFICES_PER_FLOOR = 1;
	const int TOTAL_WORKPLACES_PER_OFFICE = 6;
	const float PERCENTAGE_OF_PROPERTY_BUILDINGS_WITH_SHOPS = 0.5f;

	public int totalProperties = 102;
	public int totalEmptyProperties = 6;
	public int totalHomelessNpcs = 5;

	public int totalEmptyWorkPlaces = 5;
	public int maxFloorsPerPropertyBuilding = 5;
	public int maxFloorsPerOfficeBuilding = 5;

	public int minAppleBuildings = 3;
	public int maxAppleBuildings = 5;
	public int maxAppleSolar = 2;
	public int maxAppleAlley = 2;

	public int minAppleShops = 1;
	public int maxAppleShops = 3;

	private int totalNpcs = 0;
	private int totalWorkPlaces = 0;
	
		//Scenary - city1
			// apple - apple1
				// residential - residential1
					// floor - hall
					// floor - floor1
						// property - property1
							// room - livingRoom
								// room - bathRoom
					// floor - floor2
				// shop - shop1
					// shopRoom - shopRoom1
				// solar - solar1
				// residential - residential2
				// alley - alley1
				// residential - residential3
				
			
			// apple - apple2
				// company - company1
					// lo mismo que residential
	void Start () { 
		precalculateProcedural ();
	}

	void precalculateProcedural () {

		this.totalNpcs = this.totalProperties - 1 - this.totalEmptyProperties + this.totalHomelessNpcs;
		this.totalWorkPlaces = this.totalNpcs + 1;
		ArrayList propertyBuildings = new ArrayList ();
		int propertiesLeft = this.totalProperties;
		ArrayList officeBuildings = new ArrayList ();
		int totalShops = 0;

		if(this.totalProperties % PROPERTIES_PER_FLOOR != 0)
		{
			Debug.LogError("Property number is not divisible by PROPERTIES_PER_FLOOR: " + PROPERTIES_PER_FLOOR);
			Application.Quit();
		}

		/*if(this.totalWorkPlaces % (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE) != 0)
		{
			Debug.LogError("WorkPlaces number is not divisible by TOTAL_WORKPLACES_PER_OFFICE:" + TOTAL_WORKPLACES_PER_OFFICE);
			Application.Quit();
		}*/

		//We calculate the properties total buildings and floors per buildings
		while(propertiesLeft > 0){
			int totalBuildingFloors = Random.Range(MIN_FLOORS_PER_PROPERTY_BUILDING, Mathf.Min((propertiesLeft / PROPERTIES_PER_FLOOR), this.maxFloorsPerPropertyBuilding) + 1);
			propertyBuildings.Add(totalBuildingFloors);
			propertiesLeft -= totalBuildingFloors * PROPERTIES_PER_FLOOR;
		}

		//We calculate the total shops
		totalShops = Mathf.Min(this.totalWorkPlaces, Mathf.RoundToInt(propertyBuildings.Count * PERCENTAGE_OF_PROPERTY_BUILDINGS_WITH_SHOPS));
		int extraWorkPlaces = (this.totalWorkPlaces - totalShops) % (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE);
		this.totalWorkPlaces += extraWorkPlaces;
		int workPlacesLeft = this.totalWorkPlaces;
		workPlacesLeft -= totalShops;

		//We calculate the offices total buildings and floors per buildings

		while(workPlacesLeft > 0){
			int totalBuildingFloors = Random.Range(MIN_FLOORS_PER_OFFICE_BUILDING, Mathf.Min((workPlacesLeft / (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE)), this.maxFloorsPerOfficeBuilding) + 1);
			officeBuildings.Add(totalBuildingFloors);
			workPlacesLeft -= totalBuildingFloors * (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE);
		}

		//We calculate the properties total apples and buildins per apple
		ArrayList propertyBuildingsApples = calculateApples (propertyBuildings.Count);
		/*ArrayList propertyBuildingsApples = new ArrayList ();
		int propertyBuildingsLeft = propertyBuildings.Count;
		while (propertyBuildingsLeft > 0) {
			int maxAppleBuildings = Mathf.Min(propertyBuildingsLeft , this.maxAppleBuildings);
			if(maxAppleBuildings >= this.minAppleBuildings){
				int totalBuildingsPerApple = Random.Range(this.minAppleBuildings, maxAppleBuildings);
				propertyBuildingsApples.Add(totalBuildingsPerApple);
				propertyBuildingsLeft -= totalBuildingsPerApple;
			}else{
				if(propertyBuildingsApples.Count > 0){
					int propertyBuildingsApplesIterator = 0;
					while (propertyBuildingsLeft > 0 && propertyBuildingsApplesIterator < propertyBuildingsApples.Count) {
						int appleBuildingsCapacity = this.maxAppleBuildings - (int)propertyBuildingsApples[propertyBuildingsApplesIterator];
						if(appleBuildingsCapacity > 0 ){
							int extraBuildings = Mathf.Min(appleBuildingsCapacity, propertyBuildingsLeft);
							propertyBuildingsApples[propertyBuildingsApplesIterator] = (int)propertyBuildingsApples[propertyBuildingsApplesIterator] + extraBuildings;
							propertyBuildingsLeft -= extraBuildings;
						}
						propertyBuildingsApplesIterator ++;
					}
					if(propertyBuildingsApplesIterator == propertyBuildingsApples.Count && propertyBuildingsLeft > 0){
						Debug.LogError("Error in calculating the number of property buildings apples");
						Application.Quit();
					}
				}else{
					Debug.LogError("Error in calculating the number of property buildings apples");
					Application.Quit();
				}
			}
		}*/

		//We calculate the offices total apples and buildins per apple
		ArrayList officeBuildingsApples = calculateApples (officeBuildings.Count);
		/*ArrayList officeBuildingsApples = new ArrayList ();
		int officeBuildingsLeft = officeBuildings.Count;

		while (officeBuildingsLeft > 0) {
			int maxAppleBuildings = Mathf.Min(officeBuildingsLeft , this.maxAppleBuildings);
			if(maxAppleBuildings >= this.minAppleBuildings){
				int totalBuildingsPerApple = Random.Range(this.minAppleBuildings, maxAppleBuildings);
				officeBuildingsApples.Add(totalBuildingsPerApple);
				officeBuildingsLeft -= totalBuildingsPerApple;
			}else{
				if(officeBuildingsApples.Count > 0){
					int officeBuildingsApplesIterator = 0;
					while (officeBuildingsLeft > 0 && officeBuildingsApplesIterator < officeBuildingsApples.Count) {
						int appleBuildingsCapacity = this.maxAppleBuildings - (int)officeBuildingsApples[officeBuildingsApplesIterator];
						if(appleBuildingsCapacity > 0 ){
							int extraBuildings = Mathf.Min(appleBuildingsCapacity, officeBuildingsLeft);
							officeBuildingsApples[officeBuildingsApplesIterator] = (int)officeBuildingsApples[officeBuildingsApplesIterator] + extraBuildings;
							officeBuildingsLeft -= extraBuildings;
						}
						officeBuildingsApplesIterator ++;
					}
					if(officeBuildingsApplesIterator == officeBuildingsApples.Count && officeBuildingsLeft > 0){
						Debug.LogError("Error in calculating the number of office buildings apples");
						Application.Quit();
					}
				}else{
					Debug.LogError("Error in calculating the number of office buildings apples");
					Application.Quit();
				}
			}
		}*/

		//We calculate the shops total number per apple
		if (totalShops < propertyBuildingsApples.Count * this.minAppleShops) {
			Debug.LogError ("Error, less shops than the minimum per apple");
			Application.Quit ();
		}

		int maxShopsAllowed = 0;
		for(int i = 0; i < propertyBuildingsApples.Count; i++){
			maxShopsAllowed += Mathf.Min((int)propertyBuildingsApples[i], this.maxAppleShops);
		}
		if (totalShops > maxShopsAllowed || totalShops > propertyBuildings.Count) {
			Debug.LogError ("Error, more shops than maximum allowed");
			Application.Quit ();
		}

		ArrayList shopsPerApples = new ArrayList ();
		int shopsLeft = totalShops;
		for(int i = 0; i < propertyBuildingsApples.Count; i++){
			shopsPerApples.Add (this.minAppleShops);
		}
		shopsLeft -= propertyBuildingsApples.Count * this.minAppleShops;

		ArrayList applesWithSpaceForShops = new ArrayList();
		for(int i = 0; i < propertyBuildingsApples.Count; i++){
			applesWithSpaceForShops.Add (i);
		}

		while(shopsLeft > 0 && applesWithSpaceForShops.Count - 1 >= 0){
			int randomApple = Random.Range(0, applesWithSpaceForShops.Count);
			//object apple = applesWithSpaceForShops[randomApple];
			if((int)shopsPerApples[(int)applesWithSpaceForShops[randomApple]] < this.maxAppleShops && 
			   (int)shopsPerApples[(int)applesWithSpaceForShops[randomApple]] < (int)propertyBuildingsApples[(int)applesWithSpaceForShops[randomApple]]){
				shopsPerApples[(int)applesWithSpaceForShops[randomApple]] = (int)shopsPerApples[(int)applesWithSpaceForShops[randomApple]] + 1;
				shopsLeft --;
			}
			//Debug.Log ("apple: " + shopsPerApples.IndexOf(applesWithSpaceForShops[randomApple]));
			if((int)shopsPerApples[(int)applesWithSpaceForShops[randomApple]] == this.maxAppleShops || 
			   (int)shopsPerApples[(int)applesWithSpaceForShops[randomApple]] == (int)propertyBuildingsApples[(int)applesWithSpaceForShops[randomApple]]){
				applesWithSpaceForShops.Remove(applesWithSpaceForShops[randomApple]);
			}
		}

		string floorsString = "";
		foreach(int floors in propertyBuildings){
			floorsString += ", " + floors;
		}
		Debug.Log ("Floors per propertyBuilding: " + floorsString);
		Debug.Log ("Total shops: " + totalShops);

		floorsString = "";
		foreach(int floors in officeBuildings){
			floorsString += ", " + floors;
		}
		Debug.Log ("Floors per officeBuilding: " + floorsString);

		string applesString = "";
		foreach(int buildings in propertyBuildingsApples){
			applesString += ", " + buildings;
		}
		Debug.Log ("Buildings per Properties Apple: " + applesString);

		applesString = "";
		foreach(int shops in shopsPerApples){
			applesString += ", " + shops;
		}
		Debug.Log ("Shops per Properties Apple: " + applesString);

		applesString = "";
		foreach(int buildings in officeBuildingsApples){
			applesString += ", " + buildings;
		}
		Debug.Log ("Buildings per Office Apple: " + applesString);

	}

	private ArrayList calculateApples(int buildingsNumber){
		ArrayList apples = new ArrayList ();
		int minApples = Mathf.CeilToInt((float)buildingsNumber / (float)this.maxAppleBuildings);
		if (!(buildingsNumber >= this.minAppleBuildings * minApples && buildingsNumber <= this.maxAppleBuildings * minApples)) {
			Debug.LogError ("Error: Cannot create a minimum of " + minApples + " apples with " + buildingsNumber + 
			                " buildings and a minimun of " + this.minAppleBuildings + " and a maximum of " + this.maxAppleBuildings);
			Application.Quit ();
		}

		int maxApples = Mathf.FloorToInt((float)buildingsNumber / (float)this.minAppleBuildings);
		if (maxApples * (this.maxAppleBuildings - this.minAppleBuildings) < buildingsNumber%3) {
			Debug.LogError ("Error: Cannot create a maximum of " + maxApples + " apples with " + buildingsNumber + 
			                " buildings and a minimun of " + this.minAppleBuildings + " and a maximum of " + this.maxAppleBuildings);
			Application.Quit ();
		}

		int totalApples = Random.Range (minApples, maxApples + 1);

		for(int i = 0; i < totalApples - 1; i++){
			int minRandom = Mathf.Max (this.minAppleBuildings, buildingsNumber - ((totalApples - (i + 1)) * this.maxAppleBuildings));
			int maxRandom = Mathf.Min (this.maxAppleBuildings, buildingsNumber - ((totalApples - (i + 1)) * this.minAppleBuildings));
			int totalBuildings = Random.Range (minRandom, maxRandom + 1);
			apples.Add (totalBuildings);
			buildingsNumber -= totalBuildings;
		}
		apples.Add (buildingsNumber);
		shuffleList (apples);
		return apples;
	}

	public static void shuffleList(ArrayList list)
	{
		if (list.Count > 1)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				object tmp = list[i];
				int randomIndex = Random.Range(0, i + 1);
				
				//Swap elements
				list[i] = list[randomIndex];
				list[randomIndex] = tmp;
			}
		}
	}
}
