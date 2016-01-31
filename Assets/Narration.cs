using UnityEngine;
using System.Collections;

public class Narration : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake(){
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
}
