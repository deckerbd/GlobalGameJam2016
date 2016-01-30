using UnityEngine;
using System.Collections;

public class moundScript : MonoBehaviour 
{
	public GameObject cornstalk;


	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && Input.GetKeyDown ("f"))
		{
			Instantiate (cornstalk, transform.position, Quaternion.identity);
		}
			
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
