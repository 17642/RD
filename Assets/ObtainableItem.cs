using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Throw, Use, Equip, None
}

public enum EquipType
{
    None, Armor, Weapon
}

public enum ThrowType
{
    None, Straight, Up
}

public enum UseType {
    None ,OnUser, OnItem, OnEnvironment, OnEnemy
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

public class ItemEffect
{
    public EffectType effectType;
    public EffectCondition condition;
    public EffectArea effectArea;
    public int range;
    public int value;
    public int value2;
}

public class ObtainableItem : ScriptableObject
{
    public int itemCode;

    public Sprite itemSprite;
    public Sprite EquipSprite;

    public string itemName;
    [TextArea]
    public string description;

    public ItemType itemType;
    public EquipType equipType = EquipType.None;
    public ThrowType throwType = ThrowType.None;
    public UseType useType = UseType.None;

    public List<ItemEffect> effects = new List<ItemEffect>();

    public int level;

}
