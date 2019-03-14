/* Yannick Renz
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

    public float circleReward;
    private int circleCountdown = 0;
    private SpriteRenderer sprite;
    private GameObject manager;
    private Laurel laurelScript;

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
        manager = GameObject.Find("/Overlay/UIManager");
        laurelScript = GameObject.Find("/Playermanager/PlayerLaurel").GetComponent<Laurel>();
    }
	
	// Update is called once per frame
	void Update () {

        if (circleCountdown > 0)
        {
            circleCountdown--;
            manager.SendMessage("ChangeScore", circleReward*Time.deltaTime);
            if (laurelScript.facingRight)
                transform.Rotate(0f, 0f, 1f, Space.Self);
            else if (!laurelScript.facingRight)
                transform.Rotate(0f, 0f, -1f, Space.Self);
        }
        else
        {
            sprite.enabled = false;
        }

    }

    public void OnTriggerStay2D(Collider2D col)
    {
        int layer = col.gameObject.layer;
        if (layer <= 15 && layer >= 10 && layer != 12)
        {
            sprite.enabled = true;
            circleCountdown = 2;
        }
    }

}
