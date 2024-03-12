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
        // 해당 위치에서 겹치는 모든 collider를 가져옵니다.
        Collider2D[] colliders = Physics2D.OverlapPointAll(tilemap.GetCellCenterWorld(pos));

        // 가져온 colliders를 반복하여 아이템이 있는지 확인합니다.
        foreach (Collider2D collider in colliders)
        {
            // collider에 연결된 GameObject를 확인합니다.
            GameObject obj = collider.gameObject;

            // 아이템인지 확인합니다. 만약 아이템이라면 true를 반환합니다.
            if (obj.CompareTag("Item") || obj.CompareTag("Location"))
            {
                return true;
            }
        }

        // 아이템이 없는 경우에는 false를 반환합니다.
        return false;
    }
}
