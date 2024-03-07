using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemGenerator : MonoBehaviour
{

    public List<GameObject> rooms;
    public Tilemap tilemap;
    public GameObject defaultItemPrefab;
    public void Initalize()
    {
        defaultItemPrefab = StageManager.Instance.defaultItemPrefab;
        tilemap = StageManager.Instance.tileMap;
    }

    public void SetItem(GameObject item)
    {

    }

    public void GenerateItem(int index)
    {

    }
    
}
