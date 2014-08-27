using UnityEngine;
using System.Collections;

public class character2dMovement : MonoBehaviour {

	Vector2 movementSpeed = new Vector2 (5, 0);
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			rigidbody2D.MovePosition(rigidbody2D.position + movementSpeed * Time.deltaTime);
			
		}
		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			rigidbody2D.MovePosition(rigidbody2D.position - movementSpeed * Time.deltaTime);
			
		}
	}

}
