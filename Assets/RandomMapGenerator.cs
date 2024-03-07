using JetBrains.Annotations;

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class MapNode
{
    public Vector2Int position;//��ġ(ó���� 0,0) / �»�� ����
    public Vector2Int size; // ũ��

    public MapNode left;//����� ���
    public MapNode right;

    public RoomData roomData;//�� ����(���� ���� ���. ���� ��쿡�� Null)

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
            //�������� �ʴ� ����� ��� ����
            return;
        }

        if(node.left==null && node.right == null)
        {
            mapNodes.Add(node);//�ڽ��� ���� ��� ��� ����Ʈ�� �߰�
        }

        FindBottomNodeDFS(node.left, mapNodes);//�ڽ��� ���� ��� ���� �ڽ� Ž��
        FindBottomNodeDFS(node.right, mapNodes);
    }



    public MapNode[] FindBottomNode()
    {
        List<MapNode> mapNodes = new List<MapNode>();


        FindBottomNodeDFS(this,mapNodes);

        List<MapNode> vaildNode = new List<MapNode>();

        foreach(MapNode node in mapNodes)
        {
            if (node.size.x >= 6 && node.size.y >= 6) // �켱 �ּ� ��� ũ�⸦ 6���� ����
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
    public Vector2Int size; //�� ��ġ �� ũ��. ��忡 ���� ��������� ����

    public List<RoomConnection> passage; // ���� �迭
    public RoomType type; // �� Ÿ��
    

    public RoomData(int roomMinimumSize, int roomMaximumSize, Vector2Int nodeSize)//�θ� ���� ������� �� ���� �����ϰ� ����
    {
        int roomMaxSizeX = Mathf.Min(nodeSize.x, roomMaximumSize); // ���� ���� ��쿡�� �ڵ����� ���� �Ҹ� 
        int roomMaxSizeY = Mathf.Min(nodeSize.y, roomMaximumSize);

        size.x = Random.Range(roomMinimumSize, roomMaxSizeX); //�� ũ�� �����ϰ� ����
        size.y = Random.Range(roomMinimumSize, roomMaxSizeY);

        position.x = Random.Range(0, nodeSize.x - size.x); //�� ũ��� ��� ũ�� ������� ���� ����
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
        bool splitHorizontal = Random.Range(0, 2) == 0; // �� ���η� ������

        int splitSize = splitHorizontal ? Random.Range(minimalSize, node.size.y - minimalSize) : Random.Range(minimalSize, node.size.x-minimalSize);//�� ���� ��ǥ

        if (splitSize > minimalSize-1) // ������ �߸��Ǿ��� �� ���� �ƿ� X
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
            RoomData rd = new RoomData(3,100,node.size); // �켱 �� ũ�⸦ 3~6���� ����
            rd.SetRoomDataOnRandomValue();

            node.SetRoomData(rd);

            node.roomData.position += node.position; // Room ��ġ �缳��

            roomDatas.Add(node.roomData);

        }
    }

    public void SetRoomNodes(int minRoomCount, int maxResetCount)
    {
        int resetCount = 0; // ��õ� Ƚ�� ī��Ʈ ���� �߰�

        // �ִ� ��õ� Ƚ�� ������ ���� �Ʒ��� �ִ� ������ ã��
        MapNode[] mapNodes = root.FindBottomNode();

        // �ּ� �� �������� ���� ������ ã�ų� �ִ� ��õ� Ƚ���� �ʰ��� ������ �ݺ�
        while (mapNodes.Length < minRoomCount && resetCount < maxResetCount)
        {
            // �߰� Ž���� ���� ��Ʈ ��忡������ �ٽ� Ž��
            root = new MapNode(new Vector2Int(0,0),maxSize);//��Ʈ �� �ʱ�ȭ
            Split(root);//����
            mapNodes = root.FindBottomNode();

            // ��õ� Ƚ�� ����
            resetCount++;
        }

        // �ִ� ��õ� Ƚ���� �ʰ��Ͽ� ������ ��� ��� �޽��� ���
        if (resetCount == maxResetCount)
        {
            Debug.LogWarning("�ִ� �ݺ� Ƚ����ŭ �� ������ �õ��߽��ϴ�. �ּ� ũ�⺸�� ���� ���� ������ �� �ֽ��ϴ�.");
        }

        // roomNodes �迭�� ���� �Ʒ��� �ִ� ������ �Ҵ�
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

        while (visitedRoom.Count < roomDatas.Count) // �� Ž�� �� �ּ� ��� ���� �õ�
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

        // ���� ���� Ʈ���� Ž���Ͽ� null �Ҵ�
        ClearTree(node.left);

        // ������ ���� Ʈ���� Ž���Ͽ� null �Ҵ�
        ClearTree(node.right);

        // leaf ��忡 �����ϸ� null �Ҵ�
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
    Vector2Int mapSize; // ��ü �� ũ��

    [SerializeField]
    int recursiveCount; // �� ���� ��� ��
    [SerializeField]
    int minimalNodeSize; // ��� �ּ� ũ��(6���� ����)
    [SerializeField]
    int minimalRoomSize; // �ּ� �� ũ��
    [SerializeField]
    int maximalRoomSize; // �ִ� �� ũ��
    [SerializeField]
    int minimalRoomCount; // �ּ� �� ��
    [SerializeField]
    int maxSetRoomCount; // �ִ� �� ��(����)

    [SerializeField]
    int maxFloor;
    

    BTMap map;
    
    // Start is called before the first frame update
    void Start()
    {
        map = new BTMap(mapSize, minimalRoomCount, minimalNodeSize,recursiveCount);
        Debug.Log("���ҵ� �� ��: "+map.roomNodes.Length);
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
