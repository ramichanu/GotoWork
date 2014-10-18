using UnityEngine;
using System.Collections;

public class npcIA : MonoBehaviour {
	
	public string npcState;
	public string taskState;
	public string startTimeWork;
	public string endTimeWork;


	// Use this for initialization
	void Start () {
		npcState = "Idle";
		startTimeWork = "1_0_10";
		endTimeWork = "1_1_50";
		NotificationCenter.DefaultCenter.AddObserver(this, "scriptSuccess");
	}
	
	// Update is called once per frame
	void Update () {
	
		Clock clock = GameObject.FindWithTag("gameManager").GetComponent<Clock>();

		string[] startTimeToWork = startTimeWork.Split('_');
		string[] endTimeToWork = endTimeWork.Split('_');

		if (isTimeTo(startTimeToWork, clock) && this.taskState != "running")
		{

			this.npcState = "goToWork";
			executeNpcTasksByNpcState();
		}
		else if(isTimeTo(endTimeToWork, clock) && this.taskState != "running") 
		{
			this.npcState = "goToHome";
			executeNpcTasksByNpcState();
		}
		else if(npcState != "idle" && this.taskState != "running")
		{
			//changeNpcStateAndExecuteTasks("idle");
		}
	}
	bool isTimeTo(string[] timeTo, Clock clock)
	{
		return (timeTo [0] == clock.day.ToString () && 
				timeTo [1] == clock.hour.ToString () &&
				timeTo [2] == clock.minut.ToString ());
		
	}
	void executeNpcTasksByNpcState(){
		Debug.Log ("npcState: " + this.npcState + "  -  taskState: " + this.taskState);
		switch(this.npcState)
		{
		case "goToWork":
			if(this.taskState != "running")
			{
				this.taskState = "running";
				moveCharacterToWork();
			}

			break;
		case "goToHome":
			if(this.taskState != "running")
			{
				this.taskState = "running";
				moveCharacterToHome();
				this.npcState = "idle";
			}
		break;

		}
	}

	void moveCharacterToWork(){
		GameObject target = GameObject.FindWithTag("Player");
		ArrayList fromAndTarget = new ArrayList();

		fromAndTarget.Add(gameObject);
		fromAndTarget.Add (gameObject.GetComponent<characterValues>().workCheckPoint);

		gameObject.GetComponent<moveCharacter>().executeScript(fromAndTarget);
	}

	void moveCharacterToHome(){

		ArrayList fromAndTarget = new ArrayList();
		fromAndTarget.Add(gameObject);
		fromAndTarget.Add(gameObject.GetComponent<characterValues>().homeCheckPoint);

		gameObject.GetComponent<moveCharacter>().executeScript(fromAndTarget);
	}

	void scriptSuccess(Notification options){
		moveCharacter moveCharacter = (moveCharacter)options.sender;
		if(moveCharacter.from == gameObject)
		{
			this.taskState = "success";
		}

	}
}
