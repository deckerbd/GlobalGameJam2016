using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpeningScript : MonoBehaviour {
	public GameObject fader;

	public GameObject teamSad;
	public GameObject title;

	public float fadeTime = 1.0f;
	public float hangTime = 2.0f;

	public Color fadeTo;
	public Color current;
	public Color fadeFrom;

	public bool fadeIn, fadeOut, teamSadDone;

	public float counter;

	// Use this for initialization
	void Start () {
		fadeIn = true;
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if(fadeIn){
			if (counter <= fadeTime) {
				current.a = Mathf.Lerp (fadeFrom.a, fadeTo.a, counter / fadeTime);
			} else if (counter >= fadeTime && !teamSadDone)
				StartCoroutine("RunTeamSad");
		}else if(fadeOut){
			current.a = Mathf.Lerp(fadeTo.a, fadeFrom.a, counter/fadeTime);
		}

		fader.GetComponent<Image> ().color = current;

	}

	IEnumerator RunTeamSad(){
		fadeIn = false;
		yield return new WaitForSeconds (hangTime);
		counter = 0;
		teamSadDone = true;
		fadeOut = true;
		yield return new WaitForSeconds (fadeTime);
		counter = 0;
		teamSad.SetActive (false);
		title.SetActive (true);
		fadeIn = true;
		yield return new WaitForSeconds (fadeTime);
		GameObject.Destroy (this.gameObject);
	}
}
