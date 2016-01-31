using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

public class goNextLevel : MonoBehaviour 
{
	private int nextScene; 
	private Scene thisScene; 
	// Use this for initialization
	void Start () 
	{
		thisScene = SceneManager.GetActiveScene();
		nextScene = thisScene.buildIndex + 1; 

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") 
		{ 
			SceneManager.LoadScene (nextScene);
		}
	}
}
