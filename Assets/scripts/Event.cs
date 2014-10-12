using UnityEngine;
using System.Collections;

public class Event : MonoBehaviour {

	public string startDateTime;
	public string scriptName;
	public GameObject from;
	public GameObject target;
	protected GameObject scriptObject;

	public void init(string dateTime, string script, GameObject fromObject, GameObject targetObject)
	{
		NotificationCenter.DefaultCenter.AddObserver(this, "UpdateEvent");

		scriptObject = new GameObject();
		startDateTime = dateTime;
		scriptName = script;
		scriptObject.AddComponent (scriptName);

		if( fromObject != null && targetObject != null) {
			from = fromObject;
			target = targetObject;
		}
	}

	// Update is called once per frame
	void UpdateEvent (Notification options) {

		Clock clock = (Clock)options.sender;
		string[] startTime = startDateTime.Split('_');

		if (startTime[0] == clock.day.ToString() && 
		    startTime[1] == clock.hour.ToString() &&
		    startTime[2] == clock.minut.ToString()) 
		{

			ArrayList parameters = new ArrayList();
			if (from != null && from != null) {
				parameters.Add(from);
				parameters.Add(target);
			}
			this.scriptObject.SendMessage ("executeScript", parameters);
			EventManager.eventList.Remove(this);
			NotificationCenter.DefaultCenter.PostNotification(this, "eventCallback");


		}

	}

}
