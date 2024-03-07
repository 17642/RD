using JetBrains.Annotations;

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class MapNode
{
    public Vector2Int position;//위치(처음엔 0,0) / 좌상단 기준
    public Vector2Int size; // 크기

    public MapNode left;//연결된 노드
    public MapNode right;

    public RoomData roomData;//방 정보(방이 있을 경우. 없을 경우에는 Null)

    public MapNode(Vector2Int pos, Vector2Int size)
    {
        this.position = pos;
        this.size = size;
        left = null;
        right = null;
        roomData = null;
    }
    public  void SetRoomData(RoomData roomData)
    {
        this.roomData = roomData;
    }

    void FindBottomNodeDFS(MapNode node, List<MapNode> mapNodes)
    {
        if(node == null)
        {
            //존재하지 않는 노드일 경우 종료
            return;
        }

        if(node.left==null && node.right == null)
        {
            mapNodes.Add(node);//자식이 없을 경우 노드 리스트에 추가
        }

        FindBottomNodeDFS(node.left, mapNodes);//자식이 있을 경우 하위 자식 탐색
        FindBottomNodeDFS(node.right, mapNodes);
    }



    public MapNode[] FindBottomNode()
    {
        List<MapNode> mapNodes = new List<MapNode>();


        FindBottomNodeDFS(this,mapNodes);

        List<MapNode> vaildNode = new List<MapNode>();

        foreach(MapNode node in mapNodes)
        {
            if (node.size.x >= 6 && node.size.y >= 6) // 우선 최소 노드 크기를 6으로 지정
            {
                vaildNode.Add(node);
            }
        }

        return vaildNode.ToArray();
    }

}

public class RoomConnection
{
    public int startPos;
    public int endPos;

    public float weight;

    public RoomConnection(int startPos, int endPos, float weight)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.weight = weight;
    }
}
public class RoomData
{
    
    public enum RoomType
    {
        Normal,Passage,Treasure,Shop,M_House,Hidden
    }

    public Vector2Int position;
    public Vector2Int size; //방 위치 및 크기. 노드에 따라 상대적으로 구성

    public List<RoomConnection> passage; // 연결 배열
    public RoomType type; // 방 타입
    

    public RoomData(int roomMinimumSize, int roomMaximumSize, Vector2Int nodeSize)//부모 공간 기반으로 방 공간 랜덤하게 생성
    {
        int roomMaxSizeX = Mathf.Min(nodeSize.x, roomMaximumSize); // 방이 작은 경우에는 자동으로 공간 소멸 
        int roomMaxSizeY = Mathf.Min(nodeSize.y, roomMaximumSize);

        size.x = Random.Range(roomMinimumSize, roomMaxSizeX); //방 크기 랜덤하게 지정
        size.y = Random.Range(roomMinimumSize, roomMaxSizeY);

        position.x = Random.Range(0, nodeSize.x - size.x); //방 크기와 노드 크기 기반으로 공간 지정
        position.y = Random.Range(0,nodeSize.y -  size.y);

        passage = new List<RoomConnection>();
    }

    public void MakeConnection()
    {

    }

    public void SetRoomDataOnRandomValue()
    {

    }

}

public class RoomPassages
{
    public Vector2Int startPos;
    public Vector2Int endPos;

    public RoomConnection connection;

    public RoomPassages(RoomConnection connection, List<RoomData> roomDatas)
    {

        this.connection = connection;

        MakeConnectionByRoom(roomDatas);
    }

    public void MakeConnectionByRoom(List<RoomData> roomDatas)
    {
        startPos = new Vector2Int(Random.Range(roomDatas[connection.startPos].position.x, roomDatas[connection.startPos].position.x+ roomDatas[connection.startPos].size.x), Random.Range(roomDatas[connection.startPos].position.y, roomDatas[connection.startPos].position.y + roomDatas[connection.startPos].size.y));
        endPos = new Vector2Int(Random.Range(roomDatas[connection.endPos].position.x, roomDatas[connection.endPos].position.x + roomDatas[connection.endPos].size.x), Random.Range(roomDatas[connection.endPos].position.y, roomDatas[connection.endPos].position.y + roomDatas[connection.endPos].size.y));
    }
}

public class BTMap
{
    public MapNode root;
    public MapNode[] roomNodes;
    public List<RoomData> roomDatas;
    public List<RoomPassages> passages;

    public Vector2Int maxSize;

    int minRoomCount;
    int minimalSize;
    int recursiveSize;
    public BTMap(Vector2Int size, int minRoomCount, int minimalSize,int recursiveCount)
    {
        roomDatas = new List<RoomData>();
        passages = new List<RoomPassages>();

        this.maxSize = size;
        root = new MapNode(new Vector2Int(0, 0), size);
        this.minRoomCount = minRoomCount;
        this.minimalSize = minimalSize;
        this.recursiveSize = recursiveCount;

        Split(root);
        SetRoomNodes(minRoomCount, recursiveCount);
        SetRoomData();
        ConnectRoom();
    }

    public void Split(MapNode node, int recursiveCount = 0)
    {
        if (node == null || recursiveCount >= recursiveSize)
        {
            return;
        }
        bool splitHorizontal = Random.Range(0, 2) == 0; // 방 세로로 나누기

        int splitSize = splitHorizontal ? Random.Range(minimalSize, node.size.y - minimalSize) : Random.Range(minimalSize, node.size.x-minimalSize);//방 분할 좌표

        if (splitSize > minimalSize-1) // 분할이 잘못되었을 때 분할 아예 X
        {
            if (splitHorizontal)
            {
                node.left = new MapNode(node.position, new Vector2Int(node.size.x, splitSize));
                node.right = new MapNode(new Vector2Int(node.position.x, node.position.y + splitSize), new Vector2Int(node.size.x, node.size.y - splitSize));
            }
            else
            {
                node.left = new MapNode(node.position, new Vector2Int(splitSize, node.size.y));
                node.right = new MapNode(new Vector2Int(node.position.x + splitSize, node.position.y), new Vector2Int(node.size.x - splitSize, node.size.y));
            }

            recursiveCount++;

            Split(node.left,recursiveCount);
            Split(node.right,recursiveCount);
        }
        else
        {
            return;
        }
    }

    public void SetRoomData()
    {
        /*
        bool canHaveHiddenRoom = true;
        bool canHaveShopRoom = true;
        bool canHaveTreasureRoom = true;
        bool canHaveM_House = true;

        int maxPassageRoom = Mathf.Max(0,roomNodes.Length-minRoomCount);
        int maxHiddenRoom = canHaveHiddenRoom?Mathf.Clamp(0, roomNodes.Length - minRoomCount, 1):0;
        int maxShopRoom = canHaveShopRoom?1:0;
        int maxM_House = canHaveM_House?1:0;

        int maxRoomIndex = roomNodes.Length - 1;

        int treasureRoomIndex = -1;
        int shopRoomIndex = -1;
        int m_HouseIndex = -1;
        if (canHaveTreasureRoom)
        {
            treasureRoomIndex = Random.Range(0, maxRoomIndex);
        }
        */


        
        

        foreach (MapNode node in roomNodes)
        {
            RoomData rd = new RoomData(3,100,node.size); // 우선 방 크기를 3~6으로 지정
            rd.SetRoomDataOnRandomValue();

            node.SetRoomData(rd);

            node.roomData.position += node.position; // Room 위치 재설정

            roomDatas.Add(node.roomData);

        }
    }

    public void SetRoomNodes(int minRoomCount, int maxResetCount)
    {
        int resetCount = 0; // 재시도 횟수 카운트 변수 추가

        // 최대 재시도 횟수 내에서 가장 아래에 있는 노드들을 찾음
        MapNode[] mapNodes = root.FindBottomNode();

        // 최소 방 개수보다 적은 노드들을 찾거나 최대 재시도 횟수를 초과할 때까지 반복
        while (mapNodes.Length < minRoomCount && resetCount < maxResetCount)
        {
            // 추가 탐색을 위해 루트 노드에서부터 다시 탐색
            root = new MapNode(new Vector2Int(0,0),maxSize);//루트 맵 초기화
            Split(root);//분할
            mapNodes = root.FindBottomNode();

            // 재시도 횟수 증가
            resetCount++;
        }

        // 최대 재시도 횟수를 초과하여 종료한 경우 경고 메시지 출력
        if (resetCount == maxResetCount)
        {
            Debug.LogWarning("최대 반복 횟수만큼 방 생성을 시도했습니다. 최소 크기보다 적은 방이 생성될 수 있습니다.");
        }

        // roomNodes 배열에 가장 아래에 있는 노드들을 할당
        roomNodes = mapNodes;
    }

    class Edge
    {
        public RoomData fromRoom;
        public RoomData toRoom;
        public float weight;

        public Edge(RoomData fromRoom, RoomData toRoom, float weight)
        {
            this.fromRoom = fromRoom;
            this.toRoom = toRoom;
            this.weight = weight;
        }
    }
    public void ConnectRoom()
    {
        List<RoomConnection> edges = new List<RoomConnection>();
        HashSet<RoomData> visitedRoom = new HashSet<RoomData>();

        RoomData startRoom = roomDatas[Random.Range(0, roomDatas.Count)];
        visitedRoom.Add(startRoom);

        for(int i=0;i<roomDatas.Count;i++)
        {
            RoomData roomData = roomDatas[i];

            for(int j = 0;j<roomDatas.Count;j++)
            {
                if (j != i)
                {
                    RoomConnection rpssage = new RoomConnection(i, j, Vector2.Distance(roomData.position, roomDatas[j].position));

                    roomData.passage.Add(rpssage);
                }
            }


        }

        while (visitedRoom.Count < roomDatas.Count) // 방 탐색 및 최소 경로 연결 시도
        {
            RoomConnection minEdge = new RoomConnection(-1,-1,-1);
            float minWeight = float.MaxValue;



            foreach(RoomData targetRoom  in visitedRoom)
            {
                foreach(RoomConnection edge in targetRoom.passage)
                {
                    if (!visitedRoom.Contains(roomDatas[edge.endPos]))
                    {
                        if (edge.weight < minWeight)
                        {
                            minWeight = edge.weight;
                            minEdge = edge;
                        }
                    }
                }

            }

            visitedRoom.Add(roomDatas[minEdge.endPos]);
            edges.Add(minEdge);
        }

        foreach(RoomConnection edge in edges)
        {
            passages.Add(new RoomPassages(edge, roomDatas));
        }
        
    }

    public void ClearTree(MapNode node)
    {
        if (node == null)
        {
            return;
        }

        // 왼쪽 하위 트리를 탐색하여 null 할당
        ClearTree(node.left);

        // 오른쪽 하위 트리를 탐색하여 null 할당
        ClearTree(node.right);

        // leaf 노드에 도달하면 null 할당
        node.left = null;
        node.right = null;
    }

    ~BTMap()
    {
        ClearTree(root);
        root = null;
        roomNodes = null;
        roomDatas.Clear();
        passages.Clear();
    }
}
public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField]
    Vector2Int mapSize; // 전체 맵 크기

    [SerializeField]
    int recursiveCount; // 방 분할 재귀 수
    [SerializeField]
    int minimalNodeSize; // 노드 최소 크기(6으로 지정)
    [SerializeField]
    int minimalRoomSize; // 최소 방 크기
    [SerializeField]
    int maximalRoomSize; // 최대 방 크기
    [SerializeField]
    int minimalRoomCount; // 최소 방 수
    [SerializeField]
    int maxSetRoomCount; // 최대 방 수(미정)

    [SerializeField]
    int maxFloor;
    

    BTMap map;
    
    // Start is called before the first frame update
    void Start()
    {
        map = new BTMap(mapSize, minimalRoomCount, minimalNodeSize,recursiveCount);
        Debug.Log("분할된 방 수: "+map.roomNodes.Length);
        foreach(MapNode node in map.roomNodes)
        {
            Debug.Log(node.position +" "+ node.size);
            
        }
        
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            foreach (MapNode node in map.roomNodes)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube((Vector3Int)(node.position + node.size / 2), (Vector3Int)node.size);
                if (node.roomData != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube((Vector3Int)(node.roomData.position + node.roomData.size / 2), (Vector3Int)node.roomData.size);
                }
            }
        }
        
    }
}
