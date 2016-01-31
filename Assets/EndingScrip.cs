using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingScrip : MonoBehaviour {

	public GameObject MorePopulatedSprite;

	public AudioClip[] lines;

	public GameObject[] cameraLocations;

	public float fadeTime = 49.0f;

	private float counter;

	public Color fadeTo;
	public Color current;
	public Color fadeFrom;

	public AudioSource crowd;

	public AudioClip rowdy, calm;

	// Use this for initialization
	void Start () {
		StartCoroutine ("PlayScript");

	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		current.a = Mathf.Lerp (fadeFrom.a, fadeTo.a, counter / fadeTime);
		MorePopulatedSprite.GetComponent<SpriteRenderer> ().color = current;

		crowd.volume = counter / fadeTime;

	}

	IEnumerator PlayScript(){
		int linecount = 0;

		foreach (AudioClip line in lines) {
			this.GetComponent<AudioSource> ().clip = line;
			this.GetComponent<AudioSource> ().Play ();
			Camera.main.transform.position = cameraLocations [linecount].transform.position;
			yield return new WaitForSeconds (this.GetComponent<AudioSource> ().clip.length);
			linecount++;
		}

		crowd.clip = rowdy;
		crowd.Play ();

		yield return new WaitForSeconds (5.0f);
		SceneManager.LoadScene ("Credits");
	}
}
