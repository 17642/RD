using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Damage, Buff, Environment, Map, Special
}

public enum SkillTarget
{
    Self, Enemy, Ally, Everyone, Environment, None
}

public enum SkillShape
{
    Straight, Only, Square, Triangle
}
[System.Serializable]
public class EnemySkill
{
    public string skillName;

    public int range;
    public int damage;
    public int value;
    public int value2;

    public SkillTarget target;
    public SkillType type;
    public SkillShape shape;

    public int minLevel;
    public int maxLevel;

    public float weight;
}
[System.Serializable]
public class DropItem
{
    public ObtainableItem item;

    public int minLevel;
    public int maxLevel;

    public float weight;
}

public enum EnemyBehaviorType
{
    기본, 돌격, 도주, 이동불가
}
[CreateAssetMenu(fileName = "new Enemy", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public float skillWeight;


    public int code;
    public string enemyName;
    [TextArea]
    public string description;

    public int maxHealth;
    public int damage;
    public int range;

    public float healthByLevel;
    public float damageByLevel;

    public EnemyBehaviorType behaviorType;

    public EnemySkill[] skills;
    public int dropCoin;
    public DropItem[] dropItems;

}
