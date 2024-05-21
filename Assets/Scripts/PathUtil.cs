using UnityEngine;

public class PathUtil
{
    //��Ŀ¼
    public static readonly string AsetsPath = Application.dataPath;

    //��bundleĿ¼
    public static readonly string BuildResourcePath = Application.dataPath + "/BuildResources";

    //bundle���Ŀ¼
    public static readonly string BuildOutPath = Application.streamingAssetsPath;
    /// <summary>
    /// ��ȡunity���Ŀ¼
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
    /// ��ȡ��׼·��
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
