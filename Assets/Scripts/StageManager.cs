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
    public int padding = 5;

    public List<GameObject> rooms;


    public RoomDrawer rD;
    public ItemGenerator iG;

    public int currentFloor = 0;

    private void Awake()
    {
        Instance = this;
        rD = gameObject.AddComponent<RoomDrawer>();
        iG = gameObject.AddComponent<ItemGenerator>();
    }

    void MapInit()
    {
        rD.Initialize();
        iG.Initalize();
    }

    void MapEnd()
    {
        rD.ResetMap();
        iG.ResetItem();
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
