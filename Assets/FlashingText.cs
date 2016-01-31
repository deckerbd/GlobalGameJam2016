using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FlashingText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InvokeRepeating ("ToggleText", 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			SceneManager.LoadScene (1);
		}
	}

	void ToggleText(){
		if (this.GetComponent<Text> ().enabled == false) {
			this.GetComponent<Text> ().enabled = true;
		} else {
			this.GetComponent<Text> ().enabled = false;
		}
	}
}
