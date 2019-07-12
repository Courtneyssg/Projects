using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    // References to game objects
    public GameObject Player;
    public GameObject Thruster;
    public GameObject RightEngine;
    public GameObject LeftEngine;
    public GameObject Enemy;
    public GameObject EnemyExplosion;
    public GameObject Explosion;
    public GameObject laser;
    public GameObject shieldPowerUp;
    public GameObject shield;
    public GameObject speedPowerUp;
    public GameObject tripleShotPowerUp;
    public GameObject background;
    public GameObject thrusterLeft;
    public GameObject thrusterRight;

    private void Start()
    {
        setRenderer(Player, 1);
        setRenderer(Thruster, 1);
        setRenderer(RightEngine, 3);
        setRenderer(LeftEngine, 3);
        setRenderer(Enemy, 1);
        setRenderer(EnemyExplosion, 2);
        setRenderer(Explosion, 1);
        setRenderer(laser, 1);
        setRenderer(shieldPowerUp, 1);
        setRenderer(speedPowerUp, 1);
        setRenderer(tripleShotPowerUp, 1);
        setRenderer(shield, 2);
        setRenderer(background, -1);
        setRenderer(thrusterLeft, 1);
        setRenderer(thrusterRight, 1);
    }

    // Gets access to sprite renderer on each game object and sets the order number
    public void setRenderer(GameObject prefab, int order)
    {
        SpriteRenderer render = prefab.GetComponent<SpriteRenderer>();
        render.sortingOrder = order;
    }
}