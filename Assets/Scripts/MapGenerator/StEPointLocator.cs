using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StEPointLocator : MonoBehaviour
{
    public GameObject StartLocation;
    public GameObject EndLocation;
    public Tilemap tilemap;
    public void Init()
    {
        bool setted = false;

        tilemap = StageManager.Instance.tileMap;

        StartLocation = StageManager.Instance.StartLocation;
        EndLocation = StageManager.Instance.EndLocation;



        RoomData rd = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count - 1)].GetComponent<RoomIndicator>().roomData;
        Vector2Int position = new Vector2Int(Random.Range(rd.position.x, rd.position.x + rd.size.x), Random.Range(rd.position.y, rd.position.y + rd.size.y));
        position.x += StageManager.Instance.padding;
        position.y += StageManager.Instance.padding;
        do {
            if (!ThereIsItem((Vector3Int)position))
            {
                StageManager.Instance.endPoint = Instantiate(original: EndLocation, position: new Vector3(position.x + 0.5f, position.y + 0.5f, 0), rotation: Quaternion.identity);
                setted = true;
            }

            rd = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count - 1)].GetComponent<RoomIndicator>().roomData;
            position = new Vector2Int(Random.Range(rd.position.x, rd.position.x + rd.size.x), Random.Range(rd.position.y, rd.position.y + rd.size.y));
            position.x += StageManager.Instance.padding;
            position.y += StageManager.Instance.padding;

            InfiniteLoopDetector.Run();
        } while (!setted);

        setted = false;


        RoomData pastRd = rd;
        
        
        do
        {
            rd = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count - 1)].GetComponent<RoomIndicator>().roomData;

            if (rd == pastRd)
            {
                rd = StageManager.Instance.rooms[Random.Range(0, StageManager.Instance.rooms.Count - 1)].GetComponent<RoomIndicator>().roomData;
            }
            position = new Vector2Int(Random.Range(rd.position.x, rd.position.x + rd.size.x), Random.Range(rd.position.y, rd.position.y + rd.size.y));
            position.x += StageManager.Instance.padding;
            position.y += StageManager.Instance.padding;


            if (!ThereIsItem((Vector3Int)position))
            {
                StageManager.Instance.startPoint = Instantiate(original: StartLocation, position: new Vector3(position.x + 0.5f, position.y + 0.5f, 0), rotation: Quaternion.identity);
                setted = true;
            }
            InfiniteLoopDetector.Run();
        } while (!setted);

        

    }

    public void Rset()//임시 리셋 함수
    {
        Destroy(StageManager.Instance.startPoint);
        Destroy(StageManager.Instance.endPoint);
    }

    public bool ThereIsItem(Vector3Int pos)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(tilemap.GetCellCenterWorld(pos));

        foreach (Collider2D collider in colliders)
        {

            GameObject obj = collider.gameObject;

            if (obj.CompareTag("Item") || obj.CompareTag("Location"))
            {
                return true;
            }
        }

        return false;
    }
}
