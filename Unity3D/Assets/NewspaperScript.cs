using UnityEngine;
using System.Collections;

public class NewspaperScript: MonoBehaviour {

	int numPapers=20;
	
	GameObject[] newspaper;
	GameObject fpc;
	
	//direction in degrees
	Vector3 wind=new Vector3(0,(float)(-0.2),1);
	Vector3[] windDirection;
	Vector3[] rotation;
	
	// Use this for initialization
	void Start () {
		newspaper=new GameObject[numPapers];
		windDirection=new Vector3[numPapers];
		rotation=new Vector3[numPapers];
		newspaper[0]=GameObject.Find("Newspaper");
		for (int i=1;i<numPapers;i++)
		{
			newspaper[i]=Instantiate(newspaper[0]) as GameObject;
		}
		
		fpc=GameObject.Find("First Person Controller");
		for (int i=0;i<numPapers;i++)
		{
			windDirection[i]=(new Vector3((float)(Random.value-0.5),(float)(Random.value-0.5),(float)(Random.value-0.5))+wind).normalized/10;
			newspaper[i].transform.Translate(fpc.transform.position+new Vector3((float)(Random.value*100-50),(float)(Random.value*20-10),(float)(Random.value*100-50)),Space.World);
			rotation[i]=new Vector3(Random.value,Random.value,Random.value);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0;i<numPapers;i++)
		{
			//weigh the components of the wind's direction so it has 5/10 is its original direction, 1/10 is the wind's direction, and 4/10 is a random direction
			windDirection[i]=5*windDirection[i]+wind+4*(new Vector3((float)(Random.value-0.5),(float)(Random.value-0.5),(float)(Random.value-0.5)));
			windDirection[i]=windDirection[i].normalized;
			
			RaycastHit rh;
			if(Physics.Raycast(newspaper[i].transform.position,windDirection[i],out rh,windDirection[i].magnitude))
			{
				newspaper[i].transform.LookAt(newspaper[i].transform.position+rh.normal);
				newspaper[i].transform.Rotate(90,0,0);
			}
			else
			{
				newspaper[i].transform.Translate(windDirection[i]*Time.deltaTime,Space.World);
				//newspaper[i].transform.Rotate((float)(Time.deltaTime*500*(Random.value-0.5)),(float)(Time.deltaTime*500*(Random.value-0.5)),(float)(Time.deltaTime*500*(Random.value-0.5)));
				rotation[i].x+=(float)(Random.value-.5);
				rotation[i].y+=(float)(Random.value-.5);
				rotation[i].z+=(float)(Random.value-.5);
				newspaper[i].transform.Rotate(rotation[i]*Time.deltaTime);
			}
			
			if(Vector3.Distance(newspaper[i].transform.position,fpc.transform.position)>100)
			{
				newspaper[i].transform.Translate(-1*newspaper[i].transform.position+fpc.transform.position+50*(new Vector3(2*Random.value-1,2*Random.value-1,2*Random.value-1)),Space.World);
			}
		}
	}
}
