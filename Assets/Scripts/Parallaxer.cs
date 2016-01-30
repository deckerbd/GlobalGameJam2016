using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parallaxer : MonoBehaviour {

    public float parallaxHorizontalMultiplier;
    public float parallaxVerticalMultiplier;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Parallaxer() { }

    public void SetParallaxLocation(bool shouldModifyX, float oldX, bool shouldModifyY, float oldY)
    {
        if(shouldModifyX)
        {
            float deltaX = ((Camera.main.transform.position.x - oldX) * parallaxHorizontalMultiplier);
            Vector3 changedPosition = new Vector3(gameObject.transform.position.x + deltaX,
                                                    gameObject.transform.position.y,
                                                    gameObject.transform.position.z);
            gameObject.transform.position = changedPosition;
        }
        if(shouldModifyY)
        {
            float deltaY = ((Camera.main.transform.position.y - oldY) * parallaxVerticalMultiplier);
            Vector3 changedPosition = new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.position.y + deltaY,
                                                    gameObject.transform.position.z);
            gameObject.transform.position = changedPosition;
        }
    }
}
