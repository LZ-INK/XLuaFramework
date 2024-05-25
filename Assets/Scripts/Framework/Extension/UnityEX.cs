using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public static class UnityEX 
{
    /// <summary>
    /// lua¼àÌýÀ©Õ¹
    /// </summary>
    /// <param name="button"></param>
    /// <param name="callBack"></param>
   public static void OnClickSet(this Button button, object callBack)
    {
        XLua.LuaFunction func =  callBack as XLua.LuaFunction;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            func?.Call();
        });
   }

    public static void OnValueChangedSet(this Slider slider, object callBack)
    {
        XLua.LuaFunction func = callBack as XLua.LuaFunction;
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener((float value) =>
        {
            func?.Call(value);
        });
    }
}
