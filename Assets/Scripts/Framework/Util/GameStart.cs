using UnityEngine;
using XLua;

public class GameStart : MonoBehaviour
{
    public GameMode Mode;
    // Start is called before the first frame update
    void Start()
    {
        Manager.Event.Subscribe(10000, OnLuaInit);
        AppConst.GameMode = this.Mode;
        DontDestroyOnLoad(this);

        Manager.Resource.ParseVersionFile();
        Manager.Lua.Init();
    }
    void OnLuaInit(object args)
    {
        Manager.Lua.StartLua("Main");
        XLua.LuaFunction Func = Manager.Lua.LuaEnv.Global.Get<XLua.LuaFunction>("Main");
        Func.Call();
    }
    public void OnApplicationQuit()
    {
        Manager.Event.UnSubscribe(10000, OnLuaInit);
    }
}
