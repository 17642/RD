using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

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
                GameObject selectedRoom = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count - 1)];
                Debug.Log("Room Selected #" + i);
                RoomData rd = selectedRoom.GetComponent<RoomIndicator>().roomData;
                Vector2Int position = new Vector2Int(Random.Range(rd.position.x, rd.position.x + rd.size.x), Random.Range(rd.position.y, rd.position.y + rd.size.y));
                position.x += StageManager.Instance.padding;
                position.y += StageManager.Instance.padding;
                Debug.Log("Item Set On " + position);
                if (!ThereIsObject((Vector3Int)position))
                {
                    SetEnemy(Instantiate(original: defaultEnemyPrefab, position: new Vector3(position.x + 0.5f, position.y + 0.5f, 0), rotation: Quaternion.identity));
                }
            }
        }
    }

    public bool ThereIsObject(Vector3Int position)
    {

    }

    public void SetEnemy(GameObject enemyPrefab)
    {

    }

}
