using UnityEngine;
using System.Collections;

public class cornstalkBridge : MonoBehaviour 
{
	private Rigidbody2D rbCornstalk; 
	private Animator anim; 
	private bool isFalling = false; 

	// Use this for initialization
	void Start () 
	{
		rbCornstalk = GetComponent<Rigidbody2D> (); 
		anim = GetComponent<Animator>(); 

		if (this.gameObject.name == "cornstalkLarge")
		{
			anim.SetBool ("isLarge", true); 
		}

		if (this.gameObject.name == "cornstalkMedium") 
		{
			anim.SetBool ("isMedium", true); 
		}

		if (this.gameObject.name == "cornstalkSmall") 
		{
			anim.SetBool ("isSmall", true); 
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isFalling == true) 
		{
			
		}
	}
}
