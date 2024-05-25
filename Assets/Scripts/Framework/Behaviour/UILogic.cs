using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : LuaBehaviour
{
    Action m_luaOpen;
    Action m_luaClose;

    public override void Init( string luaName)
    {
        base.Init(luaName);
        m_ScriptEnv.Get("OnOpen",out m_luaOpen);
        m_ScriptEnv.Get("OnClose",out m_luaClose);
    }
    
    public void OnOpen()
    {
        m_luaOpen?.Invoke();
    }
    public void OnClose()
    {
        m_luaClose?.Invoke();
    }

    protected override void Clear()
    {
        base.Clear();
        m_luaOpen = null;
        m_luaClose = null;
    }
}
