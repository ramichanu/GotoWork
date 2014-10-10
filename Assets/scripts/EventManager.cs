using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public static ArrayList eventList;
	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter.AddObserver(this, "eventCallback");
		eventList = new ArrayList();
		addEvent("1_2_10", "showMessage");


	}

	public static void addEvent (string dateTime, string scriptName) {
		GameObject eventObject = new GameObject ();

		eventObject.AddComponent<Event>();
		((Event)eventObject.GetComponent<Event>()).init(dateTime, scriptName);

	}

	public void eventCallback(Notification options){
		Debug.Log ("callback!!");
		Event eventCallback = options.sender.GetComponent<Event>();
		NotificationCenter.DefaultCenter.RemoveObserver (eventCallback, "UpdateEvent");
	}

}
