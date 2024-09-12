using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    int blockSize = 1;
    [SerializeField]
    Vector2 Zero = new Vector2(0.5f, 0.5f);

    public EnemyData data;

    SpriteRenderer sprite;

    public Vector2Int pos;

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

        pos.x = (int)(transform.localPosition.x - 0.5);
        pos.y = (int)(transform.localPosition.y - 0.5);

        initialized = true;

    }

    void MoveToLocation(int direction)
    {
        if (thereIsNoObject(direction))
        {

            Vector2 target = GetPositionToPN(new Vector2Int(pos.x + directionX[direction], pos.y + directionY[direction]));
            pos.x += directionX[direction];
            pos.y += directionY[direction];
            StartCoroutine(Move(target));
        }

        usableturn--;
    }

    Vector2 GetPositionToPN(Vector2Int position)
    {
        return new Vector2(position.x*blockSize+Zero.x, position.y*blockSize+ Zero.y);
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
        //못느터는 함정을 무시하고 지나감
    }
    IEnumerator Move(Vector2 destination)
    {
        moving = true;
        while (Vector2.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, destination, movespeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destination;
        moving = false;
    }
}
