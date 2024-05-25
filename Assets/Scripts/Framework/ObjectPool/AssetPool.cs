using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPool : PoolBase
{
    public override Object Spwan(string name)
    {
        return base.Spwan(name);
    }

    public override void UnSpwan(string name, Object @object)
    {
        base.UnSpwan(name, @object);
    }

    public override void Release()
    {
        base.Release();
        foreach (PoolObject item in m_Objects)
        {
            if (System.DateTime.Now.Ticks - item.LastUseTime.Ticks >= m_ReleaseTime * 10000000)
            {
                Debug.Log("GameObjectPool Release time" + System.DateTime.Now);
                Manager.Resource.UnLoadBundle(name);
                m_Objects.Remove(item);
                Release();
                return;
            }
        }
    }
}
