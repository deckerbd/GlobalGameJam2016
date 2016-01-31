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
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<WitchController> ().inSpawn = true;
			col.gameObject.GetComponent<WitchController> ().currentMaxSeeds = maxSeeds;
			col.gameObject.GetComponent<WitchController> ().spawnType = "Bean";
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if (col.gameObject.GetComponent<WitchController> ().spawnType == "Bean") {
				col.gameObject.GetComponent<WitchController> ().spawnType = string.Empty;
			}
			col.gameObject.GetComponent<WitchController> ().inSpawn = false;
			col.gameObject.GetComponent<WitchController> ().currentMaxSeeds = 0;
		}
	}
}
