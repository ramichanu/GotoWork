using UnityEngine;
using System.Collections;

public class character2dMovement : MonoBehaviour {

	Vector2 movementSpeed = new Vector2 (5, 0);
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.RightArrow)) 
		{

			anim.SetBool("rightWalk", true);
			rigidbody2D.MovePosition(rigidbody2D.position + movementSpeed * Time.deltaTime);
			
		}else{
			anim.SetBool("rightWalk", false);
		}
		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			rigidbody2D.MovePosition(rigidbody2D.position - movementSpeed * Time.deltaTime);
			
		}
	}

}
