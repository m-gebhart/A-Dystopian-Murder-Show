/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyManagers
{
    /* This class is to transfer data into a next scene, for example to show a killcount of each enemy type in Game Over or Elevator Scene 
     * For every enemy type, the enemy manager (parent) of every enemy (child) counts how many instances are killed by the player in each scene*/

    public GameObject[] enemyManagers = {
        GameObject.Find("BananaManager"),
        GameObject.Find("BatManager"),
        GameObject.Find("CannonManager"),
        GameObject.Find("LobsterManager"),
        GameObject.Find("BombManager")
    };

    public int[] kills = {
        GameObject.Find("BananaManager").GetComponent<EnemyManager>().killed,
        GameObject.Find("BatManager").GetComponent<EnemyManager>().killed,
        GameObject.Find("CannonManager").GetComponent<EnemyManager>().killed,
        GameObject.Find("LobsterManager").GetComponent<EnemyManager>().killed,
        GameObject.Find("BombManager").GetComponent<EnemyManager>().killed};

    public string[] names = { "Bananas", "Bats", "Cannons", "Lobsters", "Bombs" };

    public void SaveKills()
    {
        PlayerPrefs.SetInt("BananasKilled", kills[0]);
        PlayerPrefs.SetInt("BatsKilled", kills[1]);
        PlayerPrefs.SetInt("CannonsKilled", kills[2]);
        PlayerPrefs.SetInt("LobstersKilled", kills[3]);
        PlayerPrefs.SetInt("BombsKilled", kills[4]); 
    }
}