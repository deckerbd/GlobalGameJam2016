using UnityEngine;
using System.Collections;

public class VineGrow : MonoBehaviour {

	private GameObject platform;

	public GameObject vineTile;

	public float difference = 1.58f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GrowVine(GameObject bgWall){
		platform = bgWall.transform.GetChild (0).gameObject;
		Vector3 newPos = platform.transform.position;
		newPos.x = this.transform.position.x;
		this.transform.position = newPos;
		for (int i = 1; i < 25; i++) {
			Vector3 tilePos = this.transform.position + new Vector3 (0, -1 * difference * i, 0);
			if (!bgWall.GetComponent<BoxCollider2D> ().bounds.Contains (tilePos)) {
				break;
			}
			GameObject VineTile = Instantiate (vineTile, tilePos, this.transform.rotation) as GameObject;
			VineTile.transform.parent = this.transform;
		}
	}
}
