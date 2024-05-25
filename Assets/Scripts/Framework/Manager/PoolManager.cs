using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    Transform m_PoolParent;
    Dictionary<string , PoolBase> m_poos = new Dictionary<string , PoolBase>();
    // Start is called before the first frame update
    void Awake()
    {
        m_PoolParent = this.transform.parent.Find("Pool");
    }

    private void CreatePool<T>(string poolName,float releaseTime) where T : PoolBase
    {
        if (!m_poos.TryGetValue(poolName,out PoolBase pool))
        {
            GameObject go = new GameObject(poolName);
            go.transform.SetParent(m_PoolParent);
            pool = go.AddComponent<T>();
            pool.Init(releaseTime);
            m_poos.Add(poolName, pool);
        }
    }
    //创建物体对象池
    public  void CreateGameObjectPool(string poolName,float releaseTime)
    {
        CreatePool<GameObjectPool>(poolName,releaseTime);
    }
    //创建资源对象池
    public void CreateAssetPool(string poolName, float releaseTime)
    {
        CreatePool<AssetPool>(poolName, releaseTime);
    }
}

