using UnityEngine;
using System.Collections;

public class BeanSpawn : MonoBehaviour {

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
				if (player.GetComponent<WitchController> ().beanSeeds.Count < maxSeeds) {
					player.GetComponent<WitchController> ().AddFollower ("Bean");
				}else if (player.GetComponent<WitchController> ().beanSeeds.Count >= maxSeeds) {
					player.GetComponent<WitchController> ().RemoveFollower ("Bean");
					player.GetComponent<WitchController> ().AddFollower ("Bean");
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			playerInside = true;
			col.gameObject.GetComponent<WitchController> ().inSpawn = true;
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			playerInside = false;
			col.gameObject.GetComponent<WitchController> ().inSpawn = false;
		}
	}
}
