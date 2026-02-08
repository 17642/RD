using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    int blockSize = 1;
    [SerializeField]
    Vector2 Zero = new Vector2(0.5f, 0.5f);

    public float moveRange = 1.00f;
    public int usableturn = 1;

    public int movespeed = 3;

    public bool moving = false;

    public Vector2Int pos;

    public int[] directionX = { -1, 0, 1, 1, 1, 0, -1, -1 };
    public int[] directionY = { 1, 1, 1, 0, -1, -1, -1, 0 };
    //DIRECTION
    //  0   1   2
    //  7  -1   3
    //  6   5   4
    void Start()
    {
        pos.x = (int)(transform.localPosition.x - 0.5);
        pos.y = (int)(transform.localPosition.y - 0.5);
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving&&usableturn>0)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    MoveToLocation(1);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    MoveToLocation(5);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    MoveToLocation(3);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    MoveToLocation(7);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    MoveToLocation(2);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    MoveToLocation(6);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    MoveToLocation(4);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    MoveToLocation(0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
        }
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

        
    }

    Vector2 GetPositionToPN(Vector2Int position)
    {
        return new Vector2(position.x * blockSize + Zero.x, position.y * blockSize + Zero.y);
    }
    bool thereIsNoObject(int direction)
    {
        float rng = 1f;

        RaycastHit2D hit2 = new RaycastHit2D();

        LayerMask lm = LayerMask.GetMask("User");

        Ray2D ray = new Ray2D((Vector2)transform.position, new Vector2(directionX[direction],directionY[direction]));

        hit2=Physics2D.Raycast(ray.origin,ray.direction,rng,lm);
        //hit2 = Physics2D.Raycast(ray.origin, ray.direction, rng);

        if (hit2.collider == null)
        {
            return true;
        }

        if (hit2.collider.CompareTag("Item"))//아이템: 이동가능
        {
            grabItem(hit2);
            return true;
        }else if (hit2.collider.CompareTag("Enemy"))//적: 이동불가능
        {
            moveToEnemy(hit2);
        }else if (hit2.collider.CompareTag("Ground"))//땅: 이동가능
        {
            return true;
        }else if (hit2.collider.CompareTag("Trap"))//함정: 이동가능
        {
            handleTrap(hit2);
            return true;
        }


        return false;
    }

    void grabItem(RaycastHit2D hit)//자동 줍기
    {

    }

    void moveToEnemy(RaycastHit2D hit)//자동 공격
    {

    }

    void handleTrap(RaycastHit2D hit)
    {

    }
    IEnumerator Move(Vector2 destination)
    {
        usableturn--;
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
