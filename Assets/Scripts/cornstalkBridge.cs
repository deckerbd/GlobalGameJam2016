using UnityEngine;
using System.Collections;

public class cornstalkBridge : MonoBehaviour 
{
	private Rigidbody2D rbCornstalk; 
	private Animator anim; 
	public float force; 
	private bool isFalling = true; 
	public bool fallRight;
	public bool fallLeft; 

	// Use this for initialization
	void Start () 
	{
		rbCornstalk = GetComponent<Rigidbody2D> (); 
		anim = GetComponent<Animator>(); 

		if (this.gameObject.name == "cornstalkLargeRight(Clone)"||this.gameObject.name == "cornstalkLargeLeft(Clone)")
		{
			anim.SetBool ("isLarge", true); 
		}

		if (this.gameObject.name == "cornstalkMediumRight(Clone)" || this.gameObject.name == "cornstalkMediumLeft(Clone)") 
		{
			anim.SetBool ("isMedium", true); 
		}

		if (this.gameObject.name == "cornstalkSmallRight(Clone)" || this.gameObject.name == "cornstalkSmallLeft(Clone)") 
		{
			anim.SetBool ("isSmall", true); 
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Walkable") 
		{
			rbCornstalk.isKinematic = true; 
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(isFalling == true)
		{
			Invoke ("falling", .5f);
		}
	}

	void falling()
	{
		isFalling = false;
		if (fallRight == true) 
		{
			rbCornstalk.AddForce (new Vector2 (.5f, -.5f) * force);
		}

		 if (fallLeft == true)
		{
			rbCornstalk.AddForce (new Vector2 (-.5f, -.5f) * force);
		}
	}
}