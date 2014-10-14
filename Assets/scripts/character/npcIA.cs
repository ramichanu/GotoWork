using UnityEngine;
using System.Collections;

public class npcIA : MonoBehaviour {

	public GameObject home;
	public GameObject work;

	public string npcState;
	public string taskState;
	public string startTimeWork;
	public string endTimeWork;


	// Use this for initialization
	void Start () {
		npcState = "Idle";
		startTimeWork = "1_0_10";
		endTimeWork = "1_0_30";
		NotificationCenter.DefaultCenter.AddObserver(this, "scriptSuccess");
	}
	
	// Update is called once per frame
	void Update () {
	
		Clock clock = GameObject.FindWithTag("gameManager").GetComponent<Clock>();

		string[] startTimeToWork = startTimeWork.Split('_');
		string[] endTimeToWork = startTimeWork.Split('_');
		
		if (isTimeTo(startTimeToWork, clock))
		{
			changeNpcStateAndExecuteTasks("goToWork");
		}
		else if(isTimeTo(endTimeToWork, clock)) 

		{
			changeNpcStateAndExecuteTasks("goToHome");
		}
		else if(npcState != "idle")
		{
			changeNpcStateAndExecuteTasks("idle");
		}
	}
	bool isTimeTo(string[] timeTo, Clock clock)
	{
		return (timeTo [0] == clock.day.ToString () && 
				timeTo [1] == clock.hour.ToString () &&
				timeTo [2] == clock.minut.ToString ());
		
	}
	void changeNpcStateAndExecuteTasks(string npcState){
		switch(npcState)
		{
		case "goToWork":
			if(this.taskState != "running")
			{
				this.taskState = "running";
				moveCharacterToWork();
				this.npcState = "idle";
			}

			break;
		}
	}

	void moveCharacterToWork(){
		GameObject target = GameObject.FindWithTag("Player");
		GameObject from = gameObject;
		ArrayList fromAndTarget = new ArrayList();
		fromAndTarget.Add(from);
		fromAndTarget.Add (target);

		from.AddComponent<moveCharacter> ();
		from.GetComponent<moveCharacter>().executeScript(fromAndTarget);
	}

	void scriptSuccess(Notification options){
		Debug.Log ("SUCCESS");
		this.taskState = "success";
	}
}
