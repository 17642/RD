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
        EACHFLOOR, SELECTFLOOR //�� N������ , �� ����
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
    public string stageName; // �������� �̸�
    public string description; //����

    public float itemWeight;//������ ���� ��
    public float enemyWeight;//�� ���� ��

    public int maxEnemyNum; // �ִ� �� ��
    public int maxItemNum; // �ִ� ������ ��

    public bool canAppearShop;
    public bool canAppearHiddenRoom;

    public StageObject[] itemCanAppear;
    public StageObject[] enemyCanAppear; 

    public SpecialFloor[] specialFloors; // Ư�� ��

    public Vector2Int StageSize; //�������� �� �� ����
    public Vector2Int MaxNodeSize;
    public Vector2Int MaxRoomSize;
    public Vector2Int MinRoomSize;

    public int recursiveCount; // �� ���� Ƚ��

    public int maxRoomCount; //�ִ�/�ּ� �� ��
    public int minRoomCount;

    public int stageType; // �� Ÿ��(���� ���� ����)

    public bool isWater; // ���� �ִ� ��
    public bool isHot; // ���� ��(���� ���׸��� ��ü)

    public int rewardCoin; //����
    public StageObject rewardItem;

}
