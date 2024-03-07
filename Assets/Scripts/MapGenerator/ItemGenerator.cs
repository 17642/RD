using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemGenerator : MonoBehaviour
{

    public List<GameObject> rooms;
    public Tilemap tilemap;
    public GameObject defaultItemPrefab;

    public string itemPath = "/Items";
    public void Initalize()
    {
        defaultItemPrefab = StageManager.Instance.defaultItemPrefab;
        tilemap = StageManager.Instance.tileMap;
    }

    public void ChooseRandomPosition()
    {
        for (int i = 0; i < StageManager.Instance.stageData.maxItemNum; i++)
        {
            if (Random.Range(0f, 1.0f) <= StageManager.Instance.stageData.itemWeight)
            {
                GameObject selectedRoom = StageManager.Instance.rooms[Random.Range(0,StageManager.Instance.rooms.Count - 1)];
                BoundsInt bound = new BoundsInt((Vector3Int)selectedRoom.GetComponent<RoomIndicator>().roomData.position, (Vector3Int)selectedRoom.GetComponent<RoomIndicator>().roomData.size);
                List<Vector3Int> tilesInBounds = new List<Vector3Int>();
                foreach (Vector3Int pos in bound.allPositionsWithin)
                {
                    if (tilemap.HasTile(pos))
                    {
                        tilesInBounds.Add(pos);
                    }
                }

                // ���� ������ ������ Ÿ���� �����Ѵٸ�
                if (tilesInBounds.Count > 0)
                {
                    // ����Ʈ���� �����ϰ� Ÿ���� �����մϴ�.
                    Vector3Int selectedTilePos = tilesInBounds[Random.Range(0, tilesInBounds.Count)];
                    TileBase selectedTile = tilemap.GetTile(selectedTilePos);
                    if (!tilemap.HasTile(selectedTilePos)&&!ThereIsItem(selectedTilePos))
                    {
                        SetItem(Instantiate(original: defaultItemPrefab, position: tilemap.GetCellCenterWorld(selectedTilePos), rotation: Quaternion.identity));
                    }
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
        float itemWeight = 0f;

        foreach(StageObject set in StageManager.Instance.stageData.itemCanAppear)
        {
            if (set.startFloor >= StageManager.Instance.currentFloor && set.endFloor <= StageManager.Instance.currentFloor)
            {
                itemWeight += set.weight;
            }
        }

        float itemnum = Random.Range(0.0f, itemWeight);
        float currentWeight = 0.0f;

        ObtainableItem selected = null;

        foreach (StageObject set in StageManager.Instance.stageData.itemCanAppear)
        {
            if (set.startFloor >= StageManager.Instance.currentFloor && set.endFloor <= StageManager.Instance.currentFloor)
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
