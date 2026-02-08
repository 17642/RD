using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class ItemTags
{

    public string NaN = "None";
    //Type Tag
    public string Throwable = "Throw";
    public string Usable = "Use";
    public string Equipable = "Equip";

    //Equip Tag
    public string Armor = "Armor";
    public string Weapon = "Weapon";

    //Throw Tag
    public string Straight = "Straight";
    public string Up = "Up";

    //Use Tag
    public string OnUser = "OnUser";
    public string OnItem = "OnItem";
    public string OnEnvironment = "OnEnvironment";
    public string OnEnemy = "OnEnemy";

}


public class EffectTags
{
    public string NaN = "None";

    //EffectType
    public string Damage = "Damage";
    public string Buff = "Buff";
    public string Special = "Special";

    //EffectCondition
    public string Always = "Always";
    public string OnUse = "OnUse";
    public string OnEquip = "OnEquip";

    //Effect Area
    public string Self = "Self";
    public string AffectedEnemy = "AffectedEnemy";
    public string Environment = "Environment";
    public string Environment_Denywall = "Environment_Denywall";
}

public enum EffectType
{
    None, Damage, Buff, Special
}
public enum EffectCondition
{
    None, Always, OnUse, OnEquip 
}

public enum EffectArea
{
    None, Self, AffectedEnemy, Environment, Environment_DenyWall
}
[System.Serializable]
public class ItemEffect
{
    public EffectType effectType;
    public EffectCondition condition;
    public EffectArea effectArea;
    public int range;
    public int value;
    public int value2;
}
[CreateAssetMenu(fileName = "New Item", menuName = "ObtainableItem")]
public class ObtainableItem : ScriptableObject
{
    public int itemCode;

    public Sprite itemSprite;
    [Description("미사용")]
    public Sprite EquipSprite;
    public Color color;

    public string itemName;
    [TextArea]
    public string description;

    public string[] itemTags;

    public List<ItemEffect> effects = new List<ItemEffect>();
    [Description("-1인 경우 제한 없음")]
    public int level;//-1인 경우 제한 없음

    public int price;

}
