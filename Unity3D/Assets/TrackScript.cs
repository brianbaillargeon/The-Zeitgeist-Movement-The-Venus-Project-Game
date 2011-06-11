using UnityEngine;
using System.Collections;

public class TrackScript : MonoBehaviour {
	
	//just flip this to jump on and off the train
	public static bool onTrain=true;
	
	//the speed of the train
	public static float speed=80;
	
	//number of pieces of track
	int numTracks=100;
	
	//should be equal to track's transform.localScale[2] (ie. the size along z)
	int trackPieceLength=10;
	
	//the pieces of track
	GameObject[] track;
	
	//the train
	GameObject maglev;
	
	//the track piece that the train is currently on
	int trackNum=0;
	
	//First person controller
	GameObject fpc;
	
	//an invisible man in an invisible prison
	GameObject collisionFPC;
	
	// Use this for initialization
	void Start () {
		//Find things
		track=new GameObject[numTracks];
		track[0]=GameObject.Find("Track");
		maglev=GameObject.Find("Maglev");
		
		fpc=GameObject.Find("First Person Controller");
		collisionFPC=GameObject.Find("CollisionController");
		
		//Build the track
		RaycastHit rh;
		for (int i=1;i<40;i++)
		{
			track[i]=Instantiate(track[i-1]) as GameObject;
			track[i].transform.Translate(trackPieceLength*track[i].transform.forward,Space.World);
			//The meshes within track[i] should be set up so that their z's are half of their scales
			//This means when we call transform.Rotate on track[i], the track rotates around the back
			//of the track rather than the middle.
			track[i].transform.Rotate((float)(5*Random.value-2.5),5,(float)(5*Random.value-2.5));
			
			if(Physics.Raycast(track[i].transform.position,track[i].transform.forward,out rh,300))
			{
				//something's in the way, so let's go up!
				track[i].transform.Rotate(-20,0,0);
			}
		}
		/*can create forks in the track like this:
		the above created 40 pieces of track, but the for loop below overwrites the 
		track[] array so the train will take the path below*/
		for (int i=20;i<numTracks;i++)
		{
			track[i]=Instantiate(track[i-1]) as GameObject;
			track[i].transform.Translate(trackPieceLength*track[i].transform.forward,Space.World);
			//The meshes within track[i] should be set up so that their z's are half of their scales
			//This means when we call transform.Rotate on track[i], the track rotates around the back
			//of the track rather than the middle.
			if (i<60)
			{
				//left
				track[i].transform.Rotate((float)(5*Random.value-2.5),-5,(float)(5*Random.value-2.5));
			}
			else
			{
				//right
				track[i].transform.Rotate((float)(5*Random.value-2.5),5,(float)(5*Random.value-2.5));
			}
			
			if(Physics.Raycast(track[i].transform.position,track[i].transform.forward,out rh,300))
			{
				//something's in the way, so let's go up!
				track[i].transform.Rotate(-20,0,0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (trackNum==numTracks-1)
		{
			//We've reached the end of the tracks, go back to the start.
			//(I haven't tested this with circular tracks, it might skip a track piece)
			maglev.transform.Translate(-1*maglev.transform.position+track[0].transform.position,Space.World);
			trackNum=0;
			maglev.transform.LookAt(track[trackNum+1].transform.position+new Vector3(0,(float).5,0));
			
			//I suck at sketchup, and my train was facing the wrong axis. 
			//If we get something that faces the correct axis, we can delete this
			maglev.transform.Rotate(0,90,0);
		}
		else
		{
			//see if we've gone far enough to be on the next track piece
			Vector3 travelled=maglev.transform.position-track[trackNum].transform.position;
			if (travelled.magnitude>trackPieceLength)
			{
				trackNum++;
				
				//move to the next track piece
				maglev.transform.Translate(-1*maglev.transform.position+track[trackNum].transform.position+new Vector3(0,(float)0.5,0),Space.World);
				
				//Face the next track piece
				//throws an array out of bounds exception when it reaches the end. Shouldn't be a big deal, right?
				maglev.transform.LookAt(track[trackNum+1].transform.position+new Vector3(0,(float).5,0));
				maglev.transform.Rotate(0,90,0);
				
			}
			
			//the model's rotated 90 degrees, so we move forward as x goes down
			//if we get a new model, it'll be (0,0,speed*Time.deltaTime)
			maglev.transform.Translate(-1*speed*Time.deltaTime,0,0);
		}
		
		if (onTrain)
		{
			//Took me a long time to think of this, and I'm quite proud of it
			//This is why we have to keep an invisible man in an invisible prison.
			fpc.transform.Translate(-1*fpc.transform.position+maglev.transform.position,Space.World);
			Vector3 colPos=collisionFPC.transform.position;
			fpc.transform.Translate(colPos[0]*maglev.transform.right+colPos[1]*maglev.transform.up+colPos[2]*maglev.transform.forward,Space.World);
			fpc.transform.LookAt(fpc.transform.position+collisionFPC.transform.forward);
			fpc.transform.Rotate(maglev.transform.eulerAngles);
		}
	}
}
