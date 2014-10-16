using UnityEngine;
using System.Collections;

public class KeyComponent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update (){
		if (Input.GetKeyUp ("up")){
			this.SendMessage ("use", GameObject.FindGameObjectWithTag("Player"));
		}

	}
}
