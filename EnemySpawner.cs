/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    /*This class is about enemy spawners
     *These are visual boxes the designers can put anywhere into the scene and a certain number of enemies of one enemy types spawn from*/

    public GameObject enemyPrefab;
    public float frequencyTime;
    public int maxNumber;
    public int number = 0;
    private float originFrequency;
    public float minimumFrequency;
    public float frequencyChange;
    public float offset;
    public float growthSpeedOfNewEnemy;
    private float timer;
    private bool playerBlocking = false;

	void Start () {
        gameObject.GetComponent<SpriteRenderer>().enabled = false; //box should only be visible in game mode
        originFrequency = frequencyTime;
	}

    void Update() {
        if (number < maxNumber && !playerBlocking) {
            if (offset > 0)
                offset -= Time.deltaTime;
            else CheckFrequency(); }
    }

    //cooldown until next enemy spawn
    void CheckFrequency()
    {
        frequencyTime -= Time.deltaTime;
        if (frequencyTime < 0f)
        {
            GameObject newEnemy = GameObject.Instantiate(enemyPrefab);
            //setting data for new enemy
            newEnemy.transform.position = this.transform.position;
            Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
            newEnemyScript.growable = true;
            newEnemyScript.growthSpeed = growthSpeedOfNewEnemy;
            newEnemyScript.enemySpawner = this.gameObject;
            newEnemyScript.spawner = true;
            //frequency changes with time
            originFrequency = Mathf.Max(minimumFrequency, (originFrequency - frequencyChange));
            frequencyTime = originFrequency;
            number++;
        }
    }

    //to control max number of instantiated enemies by this spawner
    void InstanceDied()
    {
        number--;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!playerBlocking && col.gameObject.layer == 10)
            playerBlocking = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (playerBlocking && col.gameObject.layer == 10)
            playerBlocking = false;
    }
}
