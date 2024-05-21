using UnityEngine;

public class PathUtil
{
    //根目录
    public static readonly string AsetsPath = Application.dataPath;

    //打bundle目录
    public static readonly string BuildResourcePath = Application.dataPath + "/BuildResources";

    //bundle输出目录
    public static readonly string BuildOutPath = Application.streamingAssetsPath;
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
        return path.Substring(path.IndexOf("Assets"));
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
}
