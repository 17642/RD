using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class EnemyGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject defaultEnemyPrefab;

    public List<GameObject> enemies;

    public void Initialize()
    {
        defaultEnemyPrefab = StageManager.Instance.defaultEnemyPrefab;
        tilemap = StageManager.Instance.tileMap;

        enemies = new List<GameObject>();

        ChooseRandomPosition();
        Debug.Log("Enemy Generator Initialized");

    }

    public void ResetEnemy()
    {
        foreach(GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public void ChooseRandomPosition()
    {
        for (int i = 0; i < StageManager.Instance.stageData.maxItemNum; i++)
        {
            if (Random.Range(0f, 1.0f) <= StageManager.Instance.stageData.enemyWeight)
            {
                GameObject selectedRoom = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count)];
                Debug.Log("Room Selected #" + i);
                RoomData rd = selectedRoom.GetComponent<RoomIndicator>().roomData;
                Vector2Int position = new Vector2Int(Random.Range(rd.position.x, rd.position.x + rd.size.x), Random.Range(rd.position.y, rd.position.y + rd.size.y));
                position.x += StageManager.Instance.padding;
                position.y += StageManager.Instance.padding;
                Debug.Log("Enemy Set On " + position);
                if (!ThereIsObject((Vector3Int)position))
                {
                    SetEnemy(Instantiate(original: defaultEnemyPrefab, position: new Vector3(position.x + 0.5f, position.y + 0.5f, 0), rotation: Quaternion.identity));
                }
            }
        }
    }

    public bool ThereIsObject(Vector3Int position)
    {
        // �ش� ��ġ���� ��ġ�� ��� collider�� �����ɴϴ�.
        Collider2D[] colliders = Physics2D.OverlapPointAll(tilemap.GetCellCenterWorld(position));

        // ������ colliders�� �ݺ��Ͽ� �������� �ִ��� Ȯ���մϴ�.
        foreach (Collider2D collider in colliders)
        {
            // collider�� ����� GameObject�� Ȯ���մϴ�.
            GameObject obj = collider.gameObject;

            // ���������� Ȯ���մϴ�. ���� �������̶�� true�� ��ȯ�մϴ�.
            if (obj==null || !obj.CompareTag("Ground"))
            {
                return true;
            }
        }

        // �������� ���� ��쿡�� false�� ��ȯ�մϴ�.
        return false;

    }

    public void SetEnemy(GameObject enemyPrefab)
    {
        enemies.Add(enemyPrefab);



        int randLevel = 0;
        float itemWeight = 0f;
        foreach (StageObject set in StageManager.Instance.stageData.enemyCanAppear)
        {
            if (set.startFloor >= StageManager.Instance.currentFloor && set.endFloor <= StageManager.Instance.currentFloor)
            {
                itemWeight += set.weight;
            }
        }
        float itemnum = Random.Range(0.0f, itemWeight);
        float currentWeight = 0.0f;

        EnemyData selected = null;

        foreach (StageObject set in StageManager.Instance.stageData.enemyCanAppear)
        {
            if (set.startFloor >= StageManager.Instance.currentFloor && set.endFloor <= StageManager.Instance.currentFloor)
            {
                if (itemnum - currentWeight < set.weight)
                {
                    selected = (EnemyData)set.target;
                    break;
                }

                currentWeight += set.weight;
            }

            selected = (EnemyData)set.target;
            randLevel = Random.Range(set.levelMin, set.levelMax);
        }

        enemyPrefab.GetComponent<EnemyScript>().data = selected;

        
        enemyPrefab.GetComponent<EnemyScript>().Init(randLevel);


    }

}
