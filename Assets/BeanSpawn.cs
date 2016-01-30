using UnityEngine;
using System.Collections;

public class BeanSpawn : MonoBehaviour {

	public bool playerInside;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if(Input.GetButtonDown("Seed Action")){
				col.gameObject.GetComponent<WitchController> ().AddFollower ("Bean");
			}
		}
	}
}
