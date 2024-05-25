using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : PoolBase
{
    public override Object Spwan(string name)
    {
        Object @object = base.Spwan(name);
        if (@object == null)
        {
            return null;
        }
        GameObject go =  @object as GameObject;
        go.SetActive(true);
        return go;
    }

    public override void UnSpwan(string name, Object @object)
    {
        GameObject go = @object as GameObject;
        go.SetActive(false);
        go.transform.SetParent(this.transform, false);
        base.UnSpwan(name, @object);
    }

    public override void Release()
    {
        base.Release();
        foreach (PoolObject item in m_Objects)
        {
            if (System.DateTime.Now.Ticks - item.LastUseTime.Ticks >= m_ReleaseTime *10000000)
            {
                Debug.Log("GameObjectPool Release time" + System.DateTime.Now);
                Destroy(item.Object);
                m_Objects.Remove(item);
                Release();
                return;
            }
        }
    }
}
