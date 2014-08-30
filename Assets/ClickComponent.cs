using UnityEngine;
using System.Collections;

public class ClickComponent : MonoBehaviour {

	void OnMouseUp() {
		this.SendMessage ("use", GameObject.FindGameObjectWithTag("Player"));
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
