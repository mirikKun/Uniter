using UnityEngine;
using System.Collections;

public class TimedObjectDestructor : MonoBehaviour {

	public float timeOut = 3.0f;
	
	void Awake () 
	{
		Invoke ("DestroyNow", timeOut);
	}
	
	void DestroyNow ()
	{
		Destroy(gameObject);
	}
}
