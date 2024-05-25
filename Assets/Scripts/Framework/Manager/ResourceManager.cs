
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

public class ResourceManager : MonoBehaviour
{

    private class BundleInfo
    {
        public string AssetName;
        public string BundleName;
        public List<string> Dependences;
    }

    //存放BUndle信息集合
    private Dictionary<string , BundleInfo> m_BundleInfos = new Dictionary<string , BundleInfo>();

    //存放bundle资源
    private Dictionary<string, AssetBundle> m_AssetBundles = new Dictionary<string, AssetBundle>();
    /// <summary>
    /// 解析版本文件
    /// </summary>
    public void ParseVersionFile()
   {
        string url = Path.Combine(PathUtil.BundleResourcePath,AppConst.FileListName) ;

        string[] date = File.ReadAllLines(url);
        for (int i = 0; i < date.Length; i++)
        {
            BundleInfo bundleInfo =  new BundleInfo();
            string[] info = date[i].Split("|");

            bundleInfo.AssetName = info[0];
            bundleInfo.BundleName = info[1];
            bundleInfo.Dependences = new List<string>(info.Length-2);
            for (int j = 2; j < info.Length; j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetName,bundleInfo);
            
            if (info[0].IndexOf("LuaScripts")>0)
            {
                Manager.Lua.LuaNames.Add(info[0]);
            }
        }
   }

    /// <summary>
    /// 异步资源加载
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="action">信息完成是回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetName,Action<UObject> action =null)
    {
        string bundleName = m_BundleInfos[assetName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BundleResourcePath, bundleName);
        List<string> dependences = m_BundleInfos[assetName].Dependences;

        AssetBundle bundle = GetBundle(bundleName);
        if (bundle==null)
        {
            if (dependences != null && dependences.Count > 0)
            {
                for (int i = 0; i < dependences.Count; i++)
                {
                    yield return LoadBundleAsync(dependences[i]);
                }
            }
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
            yield return request;
            bundle = request.assetBundle;
            m_AssetBundles.Add(bundleName, bundle);
        }
       
        if (assetName.EndsWith(".unity"))
        {
            action?.Invoke(null);
            yield break;
        }
        AssetBundleRequest bundleRequest = bundle.LoadAssetAsync(assetName);
        yield return bundleRequest;
        
        action?.Invoke(bundleRequest?.asset);
    }


    AssetBundle GetBundle(string bundleName)
    {
        m_AssetBundles.TryGetValue(bundleName, out AssetBundle bundle);
        return  bundle;
    }
    /// <summary>
    /// 编辑器环境加载资源
    /// </summary>
    void EditorLoadAsset(string assetName,Action<UObject> action = null)
    {
#if UNITY_EDITOR
       UObject obj =  AssetDatabase.LoadAssetAtPath(assetName,typeof(UObject));
    
        if (obj == null)
        {
            Debug.Log("assetName is not Exist");

        }
        Debug.Log("this is EditorLoadAsset");
        action?.Invoke(obj);
#endif
    }
    private void LoadAsset(string assetName , Action<UObject> action)
    {
        if (AppConst.GameMode == GameMode.EditorMode)
        {
            EditorLoadAsset(assetName, action);
        }else
        StartCoroutine(LoadBundleAsync(assetName,action));
    }
    //Tag:卸载方法待做


    //加载UI
    public void LoadUI(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetUIPath(assetName), action);
    }

    public void LoadMusic(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetMusicPath(assetName), action);
    }

    public void LoadSound(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetSoundPath(assetName), action);
    }

    public void LoadEffect(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetEffectPath(assetName), action);
    }

    public void LoadScene(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(PathUtil.GetScenePath(assetName), action);
    }

    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="action"></param>
    public void LoadLua(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(assetName, action);
    }

    public void LoadPrefab(string assetName, Action<UnityEngine.Object> action = null)
    {
        LoadAsset(assetName, action);
    }

    public void UnLoadBundle(string name)
    {
      
    }
}
