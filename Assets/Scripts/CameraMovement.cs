using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Transform farLeft;
    public Transform farRight;
    public Transform farTop;
    public Transform farBottom;
    private const float X_TOLERANCE = 2f;
    private const float Y_TOLERANCE = 5.5f;
    private const float CHARACTER_Y_OFFSET = 4.3f;

    void Start()
    {
        farLeft = GameObject.Find("LeftBound").transform;
        farRight = GameObject.Find("RightBound").transform;
        farTop = GameObject.Find("UpperBound").transform;
        farBottom = GameObject.Find("LowerBound").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        List<Pair<bool, float>> cameraResults = UpdateCameraPanning();
        GameObject parallaxParent = GameObject.Find("ParallaxParent");
        Debug.Log(parallaxParent.ToString());
        
        foreach (Transform t in parallaxParent.transform.GetComponent<Transform>())
        {
            // camresults[0/1] = x/y, .first = camera has movement on axis, .second is the old value
            Debug.Log("t: " + t.GetComponent<Parallaxer>().ToString());
            t.GetComponent<Parallaxer>().SetParallaxLocation(cameraResults[0].first, cameraResults[0].second,
                                                             cameraResults[1].first, cameraResults[1].second);
        }
    }

    /// <summary>
    /// Updates the camera pan. Returns an abomination list[0/1] = x/y, .first = hasChanged, .second = old float value
    /// 
    /// </summary>
    /// <returns>List[0] = hasCameraMovedX, List[1] = hasCameraMovedY</returns>
    private List<Pair<bool,float>> UpdateCameraPanning()
    {
        Vector3 oldPosition = this.transform.position;
        Vector3 newPosition = this.transform.position;
        bool hasCameraMovedX = false, hasCameraMovedY = false;
        // if player to the right of the camera
        if (player.transform.position.x - this.transform.position.x > X_TOLERANCE)
        {
            hasCameraMovedX = true;
            newPosition.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x - (X_TOLERANCE), 0.5f);
        }
        // if player to the left of the camera
        else if (player.transform.position.x - this.transform.position.x < -1f * X_TOLERANCE)
        {
            hasCameraMovedX = true;
            newPosition.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x + (X_TOLERANCE), 0.5f);
        }

        // if player goes above the camera
        if ((player.transform.position.y + CHARACTER_Y_OFFSET) - this.transform.position.y > Y_TOLERANCE)
        {
            hasCameraMovedY = true;
            newPosition.y = Mathf.Lerp(this.transform.position.y, (player.transform.position.y + CHARACTER_Y_OFFSET) - (Y_TOLERANCE), 0.5f);
        }
        // if player goes below the camera
        else if ((player.transform.position.y + CHARACTER_Y_OFFSET) - this.transform.position.y < 0f)
        {
            hasCameraMovedY = true;
            newPosition.y = Mathf.Lerp(this.transform.position.y, (player.transform.position.y + CHARACTER_Y_OFFSET), 0.5f);
        }


        //newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        newPosition.y = Mathf.Clamp(newPosition.y, farBottom.position.y, farTop.position.y);
        this.transform.position = newPosition;

        Pair<bool, float> pair1 = new Pair<bool, float>(hasCameraMovedX, oldPosition.x);

        Pair<bool, float> pair2 = new Pair<bool, float>(hasCameraMovedY, oldPosition.y);

        return new List<Pair<bool, float>>() { pair1, pair2 };

    }

    /// <summary>
    ///     Pass in a Vector3, returned values are clamped to the edges of the camera.
    ///     This method does not set any camera positions, it just does the calculations.
    /// </summary>
    /// <param name="vec">The vector to clamp </param>
    public Vector3 GetClampedPosition(Vector3 vec)
    {
        try
        { 
            return new Vector3(Mathf.Clamp(vec.x, farLeft.position.x, farRight.position.x),
                                Mathf.Clamp(vec.y, farBottom.position.y, farTop.position.y), vec.z);
        }
        catch (UnassignedReferenceException)
        {
            farLeft = GameObject.Find("LeftBound").transform;
            farRight = GameObject.Find("RightBound").transform;
            farTop = GameObject.Find("UpperBound").transform;
            farBottom = GameObject.Find("LowerBound").transform;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            return new Vector3(Mathf.Clamp(vec.x, farLeft.position.x, farRight.position.x),
                                Mathf.Clamp(vec.y, farBottom.position.y, farTop.position.y), vec.z);
        }
    }

}