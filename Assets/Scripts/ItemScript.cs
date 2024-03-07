using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;


public class ItemScript : MonoBehaviour
{
    public ObtainableItem itemData;

    SpriteRenderer sprite;
    public void Initialize()
    {
        sprite = GetComponent<SpriteRenderer>();

        sprite.sprite = itemData.itemSprite;
        sprite.color = itemData.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
