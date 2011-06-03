using UnityEngine;
using System.Collections;

public class ElevatorScript : MonoBehaviour {

	float top=(float)20.5;
	float bottom=(float)0.5;
	
	//number of seconds elevator should wait at top and bottom
	float time=5;
	
	//start false, it'll switch to true after 'time' has elapsed
	bool goingUp=false;
	bool resting=true;
	float restingTime;
	
	GameObject player;
	
	
	// Use this for initialization
	void Start () {
		restingTime=Time.time;
		player=GameObject.Find("First Person Controller");
	}
	
	// Update is called once per frame
	void Update () {
		if (resting)
		{
			if (Time.time-restingTime>=time)
			{
				resting=false;
				goingUp=!goingUp;
			}
		}
		else
		{
			if (goingUp)
			{
				//bl and tr are opposite corners of the elevator.
				Vector3 bl=transform.position-transform.localScale;
				Vector3 tr=transform.position+transform.localScale;
				Vector3 ppos=player.transform.position-player.transform.localScale/2;
				
				//check if the player is on the platform, then move the platform, and then move the player
				
				bool onPlatform=false;
				if (bl[0]<ppos[0]&&ppos[0]<tr[0])
				{
					//we add 2 to the top just to make the ride more smooth
					if (bl[1]<ppos[1]&&ppos[1]<tr[1]+2)
					{
						if (bl[2]<ppos[2]&&ppos[2]<tr[2])
						{
							onPlatform=true;
						}
					}
				}
				
				/*we can do any translation here really. 
				Especially if the fpc's Moving Platform -> Movement Tranfer is set to PermaLocked.
				Just try not to kill the player with G forces, okay?*/
				transform.Translate(0,(float)(7.5*Time.deltaTime),0);
				
				//transform.Translate(10*(new Vector3(Random.value,Random.value,Random.value))); <- That's a ride!
				
				if (onPlatform)
				{
					//move the player precicely the height it needs to be
					tr=transform.position+transform.localScale;
					player.transform.Translate(0,tr[1]-ppos[1],0,Space.World);
				}
				
				
				if (transform.position[1]>=top)
				{
					resting=true;
					restingTime=Time.time;
					
				}
			}
			else
			{
				transform.Translate(0,(float)(-7.5*Time.deltaTime),0);
				if (transform.position[1]<=bottom)
				{
					resting=true;
					restingTime=Time.time;
				}
			}
		}
	}
}
