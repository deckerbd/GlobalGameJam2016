using UnityEngine;
using System.Collections;

public class PassthroughFloor : MonoBehaviour {

	private GameObject player, groundCheck;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		groundCheck = player.transform.FindChild ("Ground Check").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameObject.FindObjectOfType<WitchController> ().climbing) {
			if (groundCheck.transform.position.y > this.transform.position.y) {
				this.gameObject.GetComponent<Collider2D> ().enabled = true;
			} else {
				this.gameObject.GetComponent<Collider2D> ().enabled = false;
			}
		}
	}
}
