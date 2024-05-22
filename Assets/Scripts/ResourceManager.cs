
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    /// <summary>
    /// 解析版本文件
    /// </summary>
    private void ParseVersionFile()
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
        if (dependences != null && dependences.Count > 0)
        {
            for (int i = 0;i < dependences.Count;i++)
            {
                yield return LoadBundleAsync(dependences[i]);
            }
        }
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetName);
        yield return bundleRequest;

        action?.Invoke(bundleRequest?.asset);
    }

    public void LoadAsset(string assetName , Action<UObject> action)
    {
        StartCoroutine(LoadBundleAsync(assetName,action));
    }

    void Start()
    {
        ParseVersionFile();
        LoadAsset("Assets/BuildResources/UI/Prefab/LoadingUI.prefab", OnComplete);
    }

    private void OnComplete(UObject @object)
    {
        GameObject go = Instantiate(@object) as GameObject;
        go.transform.SetParent(transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }
}
