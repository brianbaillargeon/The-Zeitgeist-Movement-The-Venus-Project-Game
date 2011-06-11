using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	GameObject fpc;
	
	// Use this for initialization
	void Start () {
		fpc=GameObject.Find("First Person Controller");
	}
	
	void OnGUI()
	{
		if (TrackScript.onTrain)
		{
			if (GUI.Button (new Rect (20,20,100,20),"Hop Off Train"))
			{
				TrackScript.onTrain=false;
				fpc.transform.Rotate(0,0,-1*fpc.transform.eulerAngles[2]);
			}
		}
		else
		{
			if (GUI.Button (new Rect (20,20,100,20),"Hop On Train"))
			{
				TrackScript.onTrain=true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
