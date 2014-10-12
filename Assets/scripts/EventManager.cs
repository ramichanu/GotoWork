using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public static ArrayList eventList;
	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter.AddObserver(this, "eventCallback");
		eventList = new ArrayList();

		GameObject player = GameObject.FindWithTag ("Player");
		GameObject npc = GameObject.FindWithTag ("npc");

		addEvent("1_0_5", "moveCharacter", npc, player);


	}

	public static void addEvent (string dateTime, string scriptName, GameObject from = null, GameObject target = null) {
		GameObject eventObject = new GameObject ();

		eventObject.AddComponent<Event>();
		((Event)eventObject.GetComponent<Event>()).init(dateTime, scriptName, from, target);

	}

	public void eventCallback(Notification options){
		Debug.Log ("callback!!");
		Event eventCallback = options.sender.GetComponent<Event>();
		NotificationCenter.DefaultCenter.RemoveObserver (eventCallback, "UpdateEvent");
	}

}
