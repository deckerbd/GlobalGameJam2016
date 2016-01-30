using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public Transform farLeft;
    public Transform farRight;
    public Transform farTop;
    public Transform farBottom;
    private const float X_TOLERANCE = 2f;
    private const float Y_TOLERANCE = 5.5f;
    private const float CHARACTER_Y_OFFSET = 4.2f;

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
        Vector3 newPosition = this.transform.position;

        // if player to the right of the camera
        if (player.transform.position.x - this.transform.position.x > X_TOLERANCE)
        {
            newPosition.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x - (X_TOLERANCE + 0.1f), 0.5f);
        }
        // if player to the left of the camera
        else if (player.transform.position.x - this.transform.position.x < -1f*X_TOLERANCE)
        {
            newPosition.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x + (X_TOLERANCE + 0.1f), 0.5f);
        }

        // if player goes above the camera
        if ((player.transform.position.y + CHARACTER_Y_OFFSET) - this.transform.position.y > Y_TOLERANCE)
        {
            newPosition.y = Mathf.Lerp(this.transform.position.y, (player.transform.position.y + CHARACTER_Y_OFFSET) - (Y_TOLERANCE + 0.1f), 0.5f);
        }
        // if player goes below the camera
        else if ((player.transform.position.y+CHARACTER_Y_OFFSET ) - this.transform.position.y < 0f)
        {
            newPosition.y = Mathf.Lerp(this.transform.position.y, (player.transform.position.y + CHARACTER_Y_OFFSET) + 0.1f, 0.5f);
        }


        //newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
        newPosition.y = Mathf.Clamp(newPosition.y, farBottom.position.y, farTop.position.y);
        this.transform.position = newPosition;
    }

}