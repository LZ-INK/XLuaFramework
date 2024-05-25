using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaManager : MonoBehaviour
{
    public List<string> LuaNames = new List<string>();

    private Dictionary<string, byte[]> m_LuaScripts;

    public LuaEnv LuaEnv; //lua


    public void Init()
    {
      
        m_LuaScripts = new Dictionary<string, byte[]>();
        LuaEnv = new LuaEnv();
        LuaEnv.AddLoader(Loader);
#if UNITY_EDITOR
        if (AppConst.GameMode == GameMode.EditorMode)
            EditorLoadLuaScript();
        else
#endif
            LoadLuaScript();
        
    }
    public byte[] Loader(ref string name)
    {
        return GetLuaScript(name);
    }

    public byte[] GetLuaScript(string name)
    {
       name = name.Replace(".","/");
       string FileName = PathUtil.GetLuaPath(name).ToLower();

        byte[] luaScript = null;
        if (!m_LuaScripts.TryGetValue(FileName,out luaScript))
        {
            Debug.LogError("Lua Script is not exist!");
        }
        return luaScript;
    }
     void  LoadLuaScript()
    {
       foreach (string name in LuaNames)
        {
            Manager.Resource.LoadLua(name, (UnityEngine.Object obj) =>
            {
                AddLuaScript(name, (obj as TextAsset).bytes);
                if (m_LuaScripts.Count >= LuaNames.Count)
                {
                    Manager.Event.Fire(10000);
                    //所有Lua加载完成
                    LuaNames.Clear();
                    LuaNames = null;
                }
            });
        }
    }
    private void AddLuaScript(string assetsName, byte[] LusScript)
    {
       m_LuaScripts[(assetsName).ToLower()] = LusScript;
    }

    public void StartLua(string Name)
    {
        LuaEnv.DoString(String.Format("require'{0}'", Name));
    }
#if UNITY_EDITOR

    void EditorLoadLuaScript()
    {
        string[] LuaFiles = Directory.GetFiles(PathUtil.LuaPath,"*.bytes",SearchOption.AllDirectories);

        for (int i = 0; i < LuaFiles.Length; i++)
        {
            string fileName = PathUtil.GetStandarPath(LuaFiles[i]);
            byte[] file = File.ReadAllBytes(fileName);
            AddLuaScript(PathUtil.GetUnityPath(fileName),file);
        }
        Manager.Event.Fire(10000);
    }
#endif

    private void Update()
    {
        if (LuaEnv != null)
        {
           
            LuaEnv.Tick();
        }
    }
    private void OnDestroy()
    {
        if (LuaEnv != null)
        {
            LuaEnv.Dispose();
            LuaEnv = null;
        }
    }
}
