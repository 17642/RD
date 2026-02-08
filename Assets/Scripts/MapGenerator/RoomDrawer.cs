using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour
{

    Tilemap tilemap;
    TileBase tile;
    BTMap map;
    [SerializeField]
    GameObject RoomIndicatorPrefab;

    public List<GameObject> RoomIndicators;

    int padding;

    bool setting = false;

    public void InitBTMap()
    {
        RoomIndicatorPrefab = StageManager.Instance.roomIndicatorPrefab;
        RoomIndicators = new List<GameObject>();
        tilemap = StageManager.Instance.tileMap;
        tile = StageManager.Instance.tile;

        padding = StageManager.Instance.padding;
        map = new BTMap(StageManager.Instance.stageData.StageSize, StageManager.Instance.stageData.minRoomCount, StageManager.Instance.stageData.MinRoomSize, StageManager.Instance.stageData.recursiveCount);

        for(int x = 0; x < StageManager.Instance.stageData.StageSize.x + padding*2; x++)
        {
            for (int y = 0; y < StageManager.Instance.stageData.StageSize.y + padding*2; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
    
    public void SetPassage()
    {

        Debug.Log("Setting Passage");

        foreach(RoomPassages pass in map.passages)
        {
            

            bool udown = Random.Range(0, 2) == 0 ? true : false;

            Vector2Int corner = udown ? new Vector2Int(pass.startPos.x, pass.endPos.y) : new Vector2Int(pass.endPos.x, pass.startPos.y);

            SetTileTwoPoint(corner, pass.startPos, null);
            SetTileTwoPoint(corner, pass.endPos, null);

            Debug.Log("Corner placed on " + corner);
        }
    }

    public void SetTileTwoPoint(Vector2Int A, Vector2Int B, Tile tile)
    {
        Vector2Int direction = B - A;
        int dx = Mathf.Abs(direction.x);
        int dy = Mathf.Abs(direction.y);

        int sx = direction.x < 0 ? -1 : 1;
        int sy = direction.y < 0 ? -1 : 1;

        Debug.Log("Drawing Line from " + A + " to " + B);

        if (dx == 0)
        {
            for(int y = 0; y < dy; y++)
            {
                tilemap.SetTile(new Vector3Int(A.x+padding, A.y + (y * sy)+padding, 0),tile);
            }
        }
        else
        {
            for (int x = 0; x < dx; x++)
            {
                tilemap.SetTile(new Vector3Int(A.x + (x * sx)+padding, A.y+padding, 0), tile);
            }
        }
    }
    public void DrawRoom()
    {
        foreach(RoomData rd in map.roomDatas)
        {
            if (rd == null) continue;

            for(int x = 0; x < rd.size.x; x++)
            {
                for(int y=0; y < rd.size.y; y++)
                {
                    tilemap.SetTile(new Vector3Int(x+rd.position.x+padding, y+rd.position.y+padding, 0), null);
                }
            }

            GameObject RIP = Instantiate(RoomIndicatorPrefab);
            //Initialize Room Indicator with RoomData
            RoomIndicator RI = RIP.GetComponent<RoomIndicator>();
            RI.SetRoomData(rd, padding);
            RI.SetPositionScale();

            RoomIndicators.Add(RIP);
        }

        StageManager.Instance.rooms = RoomIndicators;
    }

    public void Initialize()
    {
        InitBTMap();
        SetPassage();
        DrawRoom();
    }

    public void ResetMap()
    {
        map.roomDatas.Clear();
        tilemap.ClearAllTiles();

        foreach (GameObject rip in RoomIndicators)
        {
            if (rip == null) continue;
            Destroy(rip);
        }

        RoomIndicators.Clear();


        RoomIndicators = null;
        map = null;
    }

    IEnumerator RCycle()
    {
        while (true)
        {
            Initialize();
            yield return new WaitForSeconds(0.5f);
            ResetMap();
        }
    }


    void Start()
    {
        //StartCoroutine(RCycle());
    }
}
