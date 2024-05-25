using UnityEngine;

public class PathUtil
{
    //根目录
    public static readonly string AsetsPath = Application.dataPath;

    //打bundle目录
    public static readonly string BuildResourcePath = Application.dataPath + "/BuildResources";

    //bundle输出目录
    public static readonly string BuildOutPath = Application.streamingAssetsPath;

    //只读目录
    public static readonly string ReadPath = Application.streamingAssetsPath;

    //lua路径
    public static readonly string LuaPath = "Assets/BuildResources/LuaScripts";

    //bundle资源目录
    public static readonly string ReadWritePath = Application.persistentDataPath;

    public static string BundleResourcePath {
        get {
            if (AppConst.GameMode == GameMode.UpdateMode)
                return ReadWritePath;
            return ReadPath;
        }
    }
    /// <summary>
    /// 获取unity相对目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path)) { 
        return string.Empty;
        }
        return GetStandarPath(path.Substring(path.IndexOf("Assets")));
    }

    /// <summary>
    /// 获取标准路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStandarPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }
        return path.Trim().Replace("\\", "/");
    }
    public static string GetLuaPath(string name)
    {
        return string.Format("Assets/BuildResources/LuaScripts/{0}.bytes", name);
    }

    public static string GetUIPath(string name)
    {
        return string.Format("Assets/BuildResources/UI/Prefabs/{0}.prefab", name);
    }

    public static string GetMusicPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Music/{0}", name);
    }

    public static string GetSoundPath(string name)
    {
        return string.Format("Assets/BuildResources/Audio/Sound/{0}", name);
    }

    public static string GetEffectPath(string name)
    {
        return string.Format("Assets/BuildResources/Effect/Prefabs/{0}.prefab", name);
    }

    public static string GetModelPath(string name)
    {
        return string.Format("Assets/BuildResources/Model/Prefabs/{0}.prefab", name);
    }

    public static string GetSpritePath(string name)
    {
        return string.Format("Assets/BuildResources/Sprites/{0}", name);
    }

    public static string GetScenePath(string name)
    {
        return string.Format("Assets/BuildResources/Scenes/{0}.unity", name);
    }

}
