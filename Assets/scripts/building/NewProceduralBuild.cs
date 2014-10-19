﻿using UnityEngine;
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
	const int TOTAL_WORKPLACES_PER_OFFICE = 8;

	public int totalProperties = 102;
	public int totalEmptyProperties = 6;
	public int totalHomelessNpcs = 5;

	public int totalEmptyWorkPlaces = 5;
	public int maxFloorsPerPropertyBuilding = 5;
	public int maxFloorsPerOfficeBuilding = 7;

	public int minAppleBuildings = 3;
	public int maxAppleBuildings = 7;
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
		int workPlacesLeft = this.totalWorkPlaces;

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

			int totalBuildingFloors = Random.Range(MIN_FLOORS_PER_PROPERTY_BUILDING, Mathf.Min((propertiesLeft / PROPERTIES_PER_FLOOR), this.maxFloorsPerPropertyBuilding));
			propertyBuildings.Add(totalBuildingFloors);
			propertiesLeft -= totalBuildingFloors * PROPERTIES_PER_FLOOR;
		}

		//We calculate the total shops
		totalShops = Mathf.Min(workPlacesLeft, Mathf.RoundToInt(propertyBuildings.Count * 0.75F));
		int extraShops = (this.totalWorkPlaces - totalShops) % (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE);
		totalShops += extraShops;
		workPlacesLeft -= totalShops;

		//We calculate the offices total buildings and floors per buildings
		while(workPlacesLeft > 0){
			int totalBuildingFloors = Random.Range(MIN_FLOORS_PER_OFFICE_BUILDING, Mathf.Min((workPlacesLeft / (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE)), this.maxFloorsPerOfficeBuilding));
			officeBuildings.Add(totalBuildingFloors);
			workPlacesLeft -= totalBuildingFloors * (OFFICES_PER_FLOOR * TOTAL_WORKPLACES_PER_OFFICE);
		}

		//We calculate the properties total apples and buildins per apple
		ArrayList propertyBuildingsApples = new ArrayList ();
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
		}

		//We calculate the offices total apples and buildins per apple
		ArrayList officeBuildingsApples = new ArrayList ();
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
		}

		//We calculate the shops total number per apple
		ArrayList shopsPerApples = new ArrayList ();
		int shopsLeft = totalShops;
		if (totalShops < propertyBuildingsApples.Count * this.minAppleShops || totalShops > propertyBuildings.Count) {
			Debug.LogError ("Error, less shops than the minimum per apple or more than property buildings");
			Application.Quit ();
		} else {
			for(int i = 0; i < propertyBuildingsApples.Count; i++){
				shopsPerApples.Add (this.minAppleShops);
			}
			shopsLeft -= propertyBuildingsApples.Count * this.minAppleShops;

			ArrayList applesWithSpaceForShops = (ArrayList)shopsPerApples.Clone();
			while(shopsLeft > 0 && applesWithSpaceForShops.Count - 1 >= 0){
				int randomApple = Random.Range(0, applesWithSpaceForShops.Count -1);
				//object apple = applesWithSpaceForShops[randomApple];
				if((int)applesWithSpaceForShops[randomApple] < this.maxAppleShops && (int)applesWithSpaceForShops[randomApple] < (int)propertyBuildingsApples[shopsPerApples.IndexOf(applesWithSpaceForShops[randomApple])]){
					applesWithSpaceForShops[randomApple] = (int)applesWithSpaceForShops[randomApple] + 1;
					shopsLeft --;
				}
				Debug.Log ("apple: " + shopsPerApples.IndexOf(applesWithSpaceForShops[randomApple]));
				if((int)applesWithSpaceForShops[randomApple] == this.maxAppleShops || (int)applesWithSpaceForShops[randomApple] == (int)propertyBuildingsApples[shopsPerApples.IndexOf(applesWithSpaceForShops[randomApple])]){
					applesWithSpaceForShops.Remove(applesWithSpaceForShops[randomApple]);
				}
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
}
