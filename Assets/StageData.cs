using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageObject
{
    public int startFloor;
    public int endFloor;

    public int levelMax;
    public int levelMin;

    public Object target;
    public float weight;
}
[System.Serializable]
public class SpecialFloor
{
    public enum AppearType
    {
        EACHFLOOR, SELECTFLOOR //각 N층마다 , 층 선택
    }
    public enum FloorType
    {
        TREASURENORMAL, TREASURESPECIAL, CHECKPOINT, BOSSROOM
    }

    public AppearType appearType;
    public FloorType floorType;

    public int floor;

    public int roomType;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "StageData")]
public class StageData : ScriptableObject
{
    public string stageName; // 스테이지 이름
    public string description; //설명

    public float itemWeight;//아이템 생성 빈도
    public float enemyWeight;//적 생성 빈도

    public int maxEnemyNum; // 최대 적 수
    public int maxItemNum; // 최대 아이템 수

    public bool canAppearShop;
    public bool canAppearHiddenRoom;

    public StageObject[] itemCanAppear;
    public StageObject[] enemyCanAppear; 

    public SpecialFloor[] specialFloors; // 특수 층

    public Vector2Int StageSize; //스테이지 및 방 설정
    public Vector2Int MaxNodeSize;
    public Vector2Int MaxRoomSize;
    public Vector2Int MinRoomSize;

    public int recursiveCount; // 방 생성 횟수

    public int maxRoomCount; //최대/최소 방 수
    public int minRoomCount;

    public int stageType; // 맵 타입(색상 변경 예정)

    public bool isWater; // 물이 있는 맵
    public bool isHot; // 더운 맵(물이 마그마로 대체)

    public int rewardCoin; //보상
    public StageObject rewardItem;

}
