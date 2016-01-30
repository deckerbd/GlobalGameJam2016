using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour {

	public float movementModifier = 2.5f;

	public GameObject groundCheck;
	public float jumpModifier = 200f;

	public bool grounded = true;

	public Queue beanSeeds;
	public Queue cornSeeds;

	public GameObject beanPrefab;
	public GameObject cornPrefab;

	public ArrayList seedFollowers;

	// Use this for initialization
	void Start () {
		beanSeeds = new Queue ();
		cornSeeds = new Queue ();
	}
	
	// Update is called once per frame
	void Update () {
		InputManager ();
		JumpManager ();
	}

	void InputManager(){
		if (Input.GetAxis ("Horizontal") != 0) {
			this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * movementModifier, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
			if (this.gameObject.GetComponent<Rigidbody2D> ().velocity.x > 0) {
				if (this.gameObject.transform.localScale.x < 0) {
					this.gameObject.transform.localScale = new Vector3 (this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
				}
			} else if (this.gameObject.GetComponent<Rigidbody2D> ().velocity.x < 0){
				if (this.gameObject.transform.localScale.x > 0) {
					this.gameObject.transform.localScale = new Vector3 (this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
				}
			}
		}
	}

	void JumpManager(){
		JumpChecker ();
		if(grounded){
			if (Input.GetButtonDown ("Jump")) {
				this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
				this.gameObject.GetComponent<Rigidbody2D> ().AddForce (Vector3.up * jumpModifier);
			}
		}
	}

	void JumpChecker(){
		grounded = Physics2D.Linecast (transform.position, groundCheck.transform.position, 1 << LayerMask.NameToLayer ("Walkable"));
	}

	public void AddFollower(string followerType){
		if (followerType == "Bean") {
			GameObject beanToAdd = Instantiate (beanPrefab, this.transform.position, this.transform.rotation) as GameObject;
			beanToAdd.GetComponent<BeanFollow>().positionInQueue = beanSeeds.Count;
			beanSeeds.Enqueue(beanToAdd);
		}
	}

}
