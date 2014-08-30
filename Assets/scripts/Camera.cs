using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	public Transform target;
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {

		this.transform.position = new Vector3(target.rigidbody2D.transform.position.x-(this.transform.localScale.x/2),target.rigidbody2D.transform.position.y+2, -10);

	
	}
}
