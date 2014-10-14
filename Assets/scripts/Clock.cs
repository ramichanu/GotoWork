using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

	public int day = 1;
	public int hour = 0;
	public int minut = 0;
	protected Hashtable clockParams;

	// Use this for initialization
	void Start () {
		clockParams = new Hashtable();
		InvokeRepeating("updateClock", 0, 1.0F);
	}

	public void updateClock () {

		if (minut == 60) {
			minut = 0;
			hour++;
		}else{
			minut++;
		}
		if (hour == 23 && minut == 60) {
			minut = 0;
			hour = 0;
			day++;
		}

		clockParams.Add("day", day);
		clockParams.Add("hour", hour);
		clockParams.Add("minut", minut);

		NotificationCenter.DefaultCenter.PostNotification(this, "UpdateScript", clockParams);
		clockParams.Clear();

	
	}
}
