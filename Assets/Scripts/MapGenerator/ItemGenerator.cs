using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemGenerator : MonoBehaviour
{

    public Tilemap tilemap;
    public GameObject defaultItemPrefab;

    public List<GameObject> items;

    public string itemPath = "/Items";
    public void Initalize()
    {
        defaultItemPrefab = StageManager.Instance.defaultItemPrefab;
        tilemap = StageManager.Instance.tileMap;

        items = new List<GameObject>();

        ChooseRandomPosition();
        Debug.Log("Initialize Item Generator");
    }

    public void ResetItem()
    {
        foreach(GameObject item in items)
        {
            Destroy(item);
        }
        items.Clear();
    }

    public void ChooseRandomPosition()
    {
        for (int i = 0; i < StageManager.Instance.stageData.maxItemNum; i++)
        {
            if (Random.Range(0f, 1.0f) <= StageManager.Instance.stageData.itemWeight)
            {
                GameObject selectedRoom = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count)];
                Debug.Log("Room Selected #" + i);
                RoomData rd = selectedRoom.GetComponent<RoomIndicator>().roomData;
                Vector2Int position = new Vector2Int(Random.Range(rd.position.x, rd.position.x + rd.size.x), Random.Range(rd.position.y, rd.position.y + rd.size.y));
                position.x += StageManager.Instance.padding;
                position.y += StageManager.Instance.padding;
                Debug.Log("Item Set On " + position);
                if (!ThereIsItem((Vector3Int)position))
                {
                    SetItem(Instantiate(original: defaultItemPrefab, position: new Vector3(position.x+0.5f,position.y+0.5f,0), rotation: Quaternion.identity)) ;
                }
            }
        }
    }
    public bool ThereIsItem(Vector3Int pos)
    {
        // �ش� ��ġ���� ��ġ�� ��� collider�� �����ɴϴ�.
        Collider2D[] colliders = Physics2D.OverlapPointAll(tilemap.GetCellCenterWorld(pos));

        // ������ colliders�� �ݺ��Ͽ� �������� �ִ��� Ȯ���մϴ�.
        foreach (Collider2D collider in colliders)
        {
            // collider�� ����� GameObject�� Ȯ���մϴ�.
            GameObject obj = collider.gameObject;

            // ���������� Ȯ���մϴ�. ���� �������̶�� true�� ��ȯ�մϴ�.
            if (obj.CompareTag("Item"))
            {
                return true;
            }
        }

        // �������� ���� ��쿡�� false�� ��ȯ�մϴ�.
        return false;
    }
    public void SetItem(GameObject item)
    {

        items.Add(item);
        float itemWeight = 0f;
        foreach(StageObject set in StageManager.Instance.stageData.itemCanAppear)
        {
            if (set.startFloor <= StageManager.Instance.currentFloor && set.endFloor >= StageManager.Instance.currentFloor)
            {
                itemWeight += set.weight;
            }
        }
        float itemnum = Random.Range(0.0f, itemWeight);
        float currentWeight = 0.0f;
        ObtainableItem selected = null;

        foreach (StageObject set in StageManager.Instance.stageData.itemCanAppear)
        {
            if (set.startFloor <= StageManager.Instance.currentFloor && set.endFloor >= StageManager.Instance.currentFloor)
            {
                if (itemnum - currentWeight < set.weight)
                {
                    selected = (ObtainableItem)set.target;
                    break;
                }

                currentWeight += set.weight;
            }

            selected = (ObtainableItem)set.target;
        }

        item.GetComponent<ItemScript>().itemData = selected;
        item.GetComponent<ItemScript>().Initialize();
    }

    
}
