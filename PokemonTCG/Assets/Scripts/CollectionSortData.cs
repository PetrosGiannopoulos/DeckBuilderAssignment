using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionSortData
{
    public Sprite sprite;
    public TextAsset textAsset;
    public string name;
    public string type;
    public int HP;
    public int rarity;

    public CollectionSortData(Sprite sprite, TextAsset textAsset)
    {
        this.sprite = sprite;
        this.textAsset = textAsset;
    }

}
