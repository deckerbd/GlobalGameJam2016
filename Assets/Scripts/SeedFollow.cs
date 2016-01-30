using UnityEngine;
using System.Collections;

public class SeedFollow : MonoBehaviour {
	
	public int positionInQueue;
	public float yDifferential;

	private GameObject player;
	private float positionDifference = 0.65f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		FollowManager ();
	}

	void FollowManager(){		
		float followDifference = (positionInQueue + 1) * positionDifference;

		if (player.transform.localScale.x > 0) {
			Vector3 newPos = Vector3.Lerp (this.transform.position, player.transform.position - new Vector3 (followDifference, 0, 0), 0.25f);
			newPos.y = player.GetComponent<WitchController>().groundCheck.transform.position.y + yDifferential;
			newPos.z = this.transform.position.z;
			this.transform.position = newPos;
			if (this.transform.localScale.x < 0) {
				Vector3 newScale = this.transform.localScale * -1;
				newScale.y = this.transform.localScale.y;
				newScale.z = this.transform.localScale.z;
				this.transform.localScale = newScale;
			}
		}
		if (player.transform.localScale.x < 0) {
			Vector3 newPos = Vector3.Lerp (this.transform.position, player.transform.position + new Vector3 (followDifference, 0, 0), 0.25f);
			newPos.y = player.GetComponent<WitchController>().groundCheck.transform.position.y + yDifferential;
			newPos.z = this.transform.position.z;
			this.transform.position = newPos;
			if (this.transform.localScale.x > 0) {
				Vector3 newScale = this.transform.localScale * -1;
				newScale.y = this.transform.localScale.y;
				newScale.z = this.transform.localScale.z;
				this.transform.localScale = newScale;
			}
		}
	}
}