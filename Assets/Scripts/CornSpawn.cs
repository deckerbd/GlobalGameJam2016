using UnityEngine;
using System.Collections;

public class CornSpawn : MonoBehaviour {

	public int maxSeeds;

	public bool playerInside;

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");	
	}
	
	// Update is called once per frame
	void Update () {
		if (playerInside) {
			if (Input.GetButtonDown ("Seed Action")) {
				if (player.GetComponent<WitchController> ().cornSeeds.Count < maxSeeds) {
					player.GetComponent<WitchController> ().AddFollower ("Corn");
				} else if (player.GetComponent<WitchController> ().cornSeeds.Count >= maxSeeds) {
					player.GetComponent<WitchController> ().RemoveFollower ("Corn");
					player.GetComponent<WitchController> ().AddFollower ("Corn");
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			playerInside = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			playerInside = false;
		}
	}
}
