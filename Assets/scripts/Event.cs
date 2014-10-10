using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

	public string startDateTime;
	public string scriptName;
	protected GameObject scriptObject;

	public void init(string dateTime, string script)
	{
		NotificationCenter.DefaultCenter.AddObserver(this, "UpdateEvent");

		scriptObject = new GameObject();
		startDateTime = dateTime;
		scriptName = script;
		scriptObject.AddComponent (scriptName);
	}

	// Update is called once per frame
	void UpdateEvent (Notification options) {

		Clock clock = (Clock)options.sender;
		string[] startTime = startDateTime.Split('_');

		if (startTime[0] == clock.day.ToString() && 
		    startTime[1] == clock.hour.ToString() &&
		    startTime[2] == clock.minut.ToString()) 
		{
			this.scriptObject.SendMessage ("executeScript");
			EventManager.eventList.Remove(this);
			NotificationCenter.DefaultCenter.PostNotification(this, "eventCallback");


		}

	}

}
