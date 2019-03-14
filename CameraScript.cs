/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private GameObject player;
    private GameObject laurel;
    private GameObject hardy;

    private Vector3 center; //Position of camera

    //certain distances player can surpass without camera following:
    public float xLimit; //horizontal limit for left and right
    public float upLimit;
    public float downLimit;

    //limits setting at the level's borders, camera shouldn't move any further
    public float ultimateLeftLimit;
    public float ultimateRightLimit;
    public float ultimateUpperLimit;
    public float ultimateLowerLimit;


    void Start()
    {
        UpdatePlayer();
        CheckPlayer();
        center = transform.position;
    }

    void LateUpdate()
    {
        CheckPlayer();
        CheckCameraPosition();
    }
     
    void CheckPlayer()
    {
        if (Playermanager.isLaurel)
            player = laurel;
        else if (!Playermanager.isLaurel)
            player = hardy;
    }

    void CheckCameraPosition()
    {
        if (player.transform.position.x > transform.position.x + xLimit) //moving to the right
        {
            center = new Vector3(player.transform.position.x - xLimit, transform.position.y, transform.position.z);
            if (center.x < ultimateRightLimit) transform.position = center;
        }
        else if (player.transform.position.x < transform.position.x - xLimit) //moving to the left
        {
            center = new Vector3(player.transform.position.x + xLimit, transform.position.y, transform.position.z);
            if (center.x > ultimateLeftLimit) transform.position = center;
        }
        if (player.transform.position.y > transform.position.y + upLimit) //moving upwards
        {
            center = new Vector3(transform.position.x, player.transform.position.y - upLimit, transform.position.z);
            if (center.y < ultimateUpperLimit) transform.position = center;
        }
        else if (player.transform.position.y < transform.position.y - downLimit) //moving down
        {
            center = new Vector3(transform.position.x, player.transform.position.y + downLimit, transform.position.z);
            if (center.y > ultimateLowerLimit) transform.position = center;
        }
    }

    void UpdatePlayer()
    {
        laurel = GameObject.Find("PlayerLaurel");
        hardy = GameObject.Find("PlayerHardy");
        CheckPlayer();
    }
}