using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool
{
    public string PoolName;
    public GameObject Prefab;
    public List<GameObject> PoolList;

    public ObjectPool(string poolName, GameObject prefab)
    {
        PoolName = poolName;
        Prefab = prefab;
        PoolList = new List<GameObject>();
    }
}

public class PoolManager : SingletonMono<PoolManager>
{
    [SerializeField] private DataAssets _dataAssets;
    [SerializeField] private List<ObjectPool> _pools = new List<ObjectPool>();
    [SerializeField] private List<Transform> _parents = new List<Transform>();

    private void Awake()
    {
        if (_dataAssets == null)
        {
            _dataAssets = Resources.Load<DataAssets>(Constants.DATA_ASSETS_PATH);
        }

        LoadDataAssets();
    }

    private void LoadDataAssets()
    {
        DataAssets asset = _dataAssets;

        foreach (var item in asset.ItemDataAssetsList)
        {
            ObjectPool pool = new ObjectPool(item.Name, item.Prefab);
            _pools.Add(pool);

            GameObject parent = new GameObject(pool.PoolName);
            parent.transform.localScale = Vector3.one;
            parent.transform.SetParent(transform);
            _parents.Add(parent.transform);
        }
    }

    public void ClearPoolData()
    {
        _pools.Clear();
    }

    public GameObject SpawnItem(NormalItem.eNormalType name, Transform parent = null)
    {
        return GetObjectFromPool(name.ToString(), parent);
    }

    public GameObject SpawnItem(BonusItem.eBonusType name, Transform parent = null)
    {
        return GetObjectFromPool(name.ToString(), parent);
    }

    private GameObject GetObjectFromPool(string poolName, Transform parent = null)
    {
        ObjectPool pool = _pools.Find(x => x.PoolName == poolName);
        GameObject obj = null;

        if (pool == null)
        {
            Debug.LogError("Pool name invalid " + poolName);
        }
        else
        {
            if (parent == null)
            {
                parent = _parents.Find(x => x.name == poolName);

                if (parent == null)
                {
                    GameObject newParent = new GameObject(poolName);
                    newParent.transform.localScale = Vector3.one;
                    newParent.transform.SetParent(parent);
                    _parents.Add(newParent.transform);

                    parent = newParent.transform;
                }
            }

            obj = pool.PoolList.Find(x => !x.activeSelf);

            if (obj == null)
            {
                obj = Instantiate(pool.Prefab);
                obj.name = poolName;
                pool.PoolList.Add(obj);
            }

            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
        }

        return obj;
    }

    public void ReturnObject(GameObject obj, string poolName = "")
    {
        if (string.IsNullOrEmpty(poolName))
        {
            poolName = obj.name;
        }

        obj.SetActive(false);
        obj.transform.localScale = Vector3.one;
        Transform parent = _parents.Find(x => x.name.Equals(poolName));

        if (parent != null)
        {
            obj.transform.SetParent(parent.transform);
        }
    }

    public void ReturnAllObjects()
    {
        foreach (var pool in _pools)
        {
            foreach (var obj in pool.PoolList)
            {
                ReturnObject(obj, pool.PoolName);
            }
        }
    }
}
