using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkinDataAssets
{
    [SerializeField] private SkinEnum _type;
    [SerializeField] private Sprite _visual;

    public SkinEnum Type => _type;
    public Sprite Visual => _visual;
}

[Serializable]
public class ItemDataAssets 
{
    [SerializeField] private ItemEnum _type;
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<SkinDataAssets> _skinList;

    public ItemEnum Type => _type;
    public string Name => _name;
    public GameObject Prefab => _prefab;
    public List<SkinDataAssets> SkinList => _skinList;
}

[CreateAssetMenu(fileName = "DataAssets", menuName = "SO/DataAssets")]
public class DataAssets : ScriptableObject
{
    [SerializeField] private List<ItemDataAssets> _itemDataAssetsList;

    public List<ItemDataAssets> ItemDataAssetsList => _itemDataAssetsList;
}
