using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField]
    public StageData stageData;
    [SerializeField]
    public Tilemap tileMap;
    [SerializeField]
    public TileBase tile;
    [SerializeField]
    public GameObject defaultItemPrefab;
    [SerializeField]
    public GameObject roomIndicatorPrefab;
    [SerializeField]
    public GameObject defaultEnemyPrefab;
    [SerializeField]
    public GameObject StartLocation;
    [SerializeField]
    public GameObject EndLocation;

    [SerializeField]
    public int padding = 5;

    public List<GameObject> rooms;


    public RoomDrawer rD;
    public ItemGenerator iG;
    public StEPointLocator sEPL;
    public EnemyGenerator eG;

    public GameObject startPoint;
    public GameObject endPoint;

    public int currentFloor = 0;

    private void Awake()
    {
        Instance = this;
        rD = gameObject.AddComponent<RoomDrawer>();
        iG = gameObject.AddComponent<ItemGenerator>();
        sEPL = gameObject.AddComponent<StEPointLocator>();
        eG = gameObject.AddComponent<EnemyGenerator>();
    }

    void MapInit()
    {
        rD.Initialize();
        iG.Initalize();
        sEPL.Init();
        eG.Initialize();
    }

    void MapEnd()
    {
        rD.ResetMap();
        iG.ResetItem();
        sEPL.Rset();
        eG.ResetEnemy();
    }

    public IEnumerator MIA()
    {
        while (true)
        {
            MapInit();
            yield return new WaitForSeconds(4.5f);
            MapEnd();
        }
    }
    void Start()
    {
        StartCoroutine(MIA());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
