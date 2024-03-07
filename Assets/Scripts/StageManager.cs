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


    public int padding = 5;

    public List<GameObject> rooms;

    public int currentFloor = 0;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
