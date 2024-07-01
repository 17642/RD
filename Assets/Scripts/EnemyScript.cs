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

    public int[] directionX = { -1, 0, 1, 1, 1, 0, -1, -1 };
    public int[] directionY = { 1, 1, 1, 0, -1, -1, -1, 0 };

    public float moveRange = 1.00f;
    public int usableturn = 1;

    public int movespeed = 3;

    public bool moving = false;


    public void Init(int lev)
    {
        sprite = GetComponent<SpriteRenderer>();
        

        currentLevel = lev;

        sprite.sprite = data.sprite;
        sprite.color = data.color;

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

    void MoveToLocation(int direction)
    {
        if (thereIsNoObject(direction))
        {
            StartCoroutine(Move(new Vector2(transform.position.x + directionX[direction], transform.position.y + directionY[direction])));
        }

        usableturn--;
    }
    bool thereIsNoObject(int direction)
    {
        float rng = 1f;
        LayerMask lm = LayerMask.GetMask("User");

        Ray2D ray = new Ray2D((Vector2)transform.position, new Vector2(directionX[direction], directionY[direction]));

        RaycastHit2D hit2 = Physics2D.Raycast(ray.origin, ray.direction, rng, lm);

        if (hit2.collider == null)
        {
            return true;
        }

        if (hit2.collider.CompareTag("Item"))//아이템: 이동가능
        {
            GrabItem(hit2);
            return true;
        }
        else if (hit2.collider.CompareTag("Player"))//플레이어: 이동불가능
        {
            moveToEnemy(hit2);
        }
        else if (hit2.collider.CompareTag("Ground"))//땅: 이동가능
        {
            return true;
        }
        else if (hit2.collider.CompareTag("Trap"))//함정: 이동가능
        {
            handleTrap(hit2);
            return true;
        }


        return false;
    }

    bool CanAttack()
    {


        return false;
    }

    bool TryUseSkill()
    {
        return false;
    }
    void GrabItem(RaycastHit2D hit)//자동 줍기
    {
        //무시하고 지나가기
    }

    void moveToEnemy(RaycastHit2D hit)//자동 공격
    {

    }

    void handleTrap(RaycastHit2D hit)
    {

    }
    IEnumerator Move(Vector2 Destination)
    {
        moving = true;
        while (((Vector2)transform.position - Destination).magnitude > 0.01f)
        {
            Vector3 newD = (Vector3)Destination;
            transform.position += (transform.position - newD).normalized * movespeed * Time.deltaTime;
            yield return null;

        }
        moving = false;
        yield return null;
    }
}
