using System;
using UnityEngine;
using XLua;

public class LuaBehaviour : MonoBehaviour
{
    private LuaEnv luaEnv = Manager.Lua?.LuaEnv;
    protected LuaTable m_ScriptEnv;
    
    private Action m_LuaDestroy;
    private Action m_LuaUpdate;
    private Action m_LuaInit;

    
    // Start is called before the first frame update
    void Awake()
    {
        m_ScriptEnv = luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        m_ScriptEnv.SetMetaTable(meta);
        meta.Dispose();


        m_ScriptEnv.Set("self", this);

      
    }

    public virtual void Init(String luaName)
    {
        luaEnv.DoString(Manager.Lua.GetLuaScript(luaName), luaName, m_ScriptEnv);

        //m_ScriptEnv.Get("Awake", out m_LuaAwake);
        //m_ScriptEnv.Get("Start", out m_LuaStart);
        m_ScriptEnv.Get("Update", out m_LuaUpdate);
        m_ScriptEnv.Get("OnInit", out m_LuaInit);
        m_LuaInit?.Invoke();
    }
   
    // Update is called once per frame
    void Update()
    {
        m_LuaUpdate?.Invoke();
    }
    protected virtual void Clear()
    {
        m_LuaUpdate = null;
        m_LuaDestroy = null;
        m_LuaInit = null;
        m_ScriptEnv?.Dispose();
        m_ScriptEnv = null;
    }
    private void OnDestroy()
    {
        m_LuaDestroy?.Invoke();
        Clear();
    }
    private void OnApplicationQuit()
    {
        Clear();
    }
}
