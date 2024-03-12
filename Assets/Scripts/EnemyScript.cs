using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyData data;

    SpriteRenderer sprite;

    public float currentMaxHp;
    public float currentDamage;

    public float currentHP;

    public int currentLevel;


    public bool initialized = false;


    public void Init()
    {
        sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = data.sprite;

        currentMaxHp = data.maxHealth + currentLevel * data.healthByLevel;
        currentDamage = data.damage + currentLevel * data.damageByLevel;

        currentHP = currentMaxHp;

        initialized = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
