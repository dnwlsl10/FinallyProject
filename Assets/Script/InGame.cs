using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    //Enemy
    public Enemy enemyPrefabs;
    private Vector3 spawnPos;
    private Enemy enemy;

    //Player
    public Warrior warrior;


    private void Start()
    {
        spawnPos = new Vector3(0, 0, 10);
        OnCreateEnemy();
    }

    public void Update()
    {
        this.warrior.onCreateEnemy = () =>
        {
            OnCreateEnemy();
        };

    }

    private void OnCreateEnemy()
    {
        this.enemy = Instantiate<Enemy>(this.enemyPrefabs);
        this.enemy.transform.position = spawnPos;
    }
}
