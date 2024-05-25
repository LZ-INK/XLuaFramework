using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{
    [MenuItem("Tools/Build Windows Buundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Tools/Build Android Buundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/Build IOS Buundle")]
    static void BundleiOSBuild()
    {
        Build(BuildTarget.iOS);
    }
    // Start is called before the first frame update
    static void Build(BuildTarget target)
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild> ();
        //
        List<string> bundleInfos = new List<string>();
       
        string[] files = Directory.GetFiles(PathUtil.BuildResourcePath,"*",SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
                continue;
         
            AssetBundleBuild assetBundle = new AssetBundleBuild();
          

            string fileName = PathUtil.GetStandarPath(files[i]);
            Debug.Log("file:" + fileName);
            string assetName = PathUtil.GetUnityPath(files[i]);
            assetBundle.assetNames = new string[] { assetName };
            
            string BundleName = "Assets" + fileName.Replace(PathUtil.BuildResourcePath, "").ToLower();
            // assetBundle.assetBundleName = BundleName+".ab";
            assetBundle.assetBundleName = BundleName+".ab";
          
            assetBundleBuilds.Add(assetBundle);

            //添加文件和依赖信息
            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo =  assetName + "|" + BundleName + ".ab";
            if (dependenceInfo.Count>0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);
            }
            bundleInfos.Add(bundleInfo);
        }
      
        if (Directory.Exists(PathUtil.BuildOutPath))
        {
            //true是递归删除
            Directory.Delete(PathUtil.BuildOutPath, true);
        }
        Directory.CreateDirectory(PathUtil.BuildOutPath);

        File.WriteAllLines(PathUtil.BuildOutPath + "/" + AppConst.FileListName, bundleInfos);
        BuildPipeline.BuildAssetBundles(PathUtil.BuildOutPath,assetBundleBuilds.ToArray(),BuildAssetBundleOptions.None, target);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
   static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        
        dependence = files.Where(file => !file.EndsWith(".cs")&&!file.Equals(curFile)).ToList();
        return  dependence;
    } 
}
