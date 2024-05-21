using System.Collections.Generic;
using System.IO;
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
            string BundleName = fileName.Replace(PathUtil.BuildResourcePath, "").ToLower();
            
            // assetBundle.assetBundleName = BundleName+".ab";
            assetBundle.assetBundleName = "Assets/"+BundleName+".ab";
          
            assetBundleBuilds.Add(assetBundle);
        }
        if (Directory.Exists(PathUtil.BuildOutPath))
        {
            //trueÊÇµÝ¹éÉ¾³ý
            Directory.Delete(PathUtil.BuildOutPath, true);
        }
        Directory.CreateDirectory(PathUtil.BuildOutPath);
        BuildPipeline.BuildAssetBundles(PathUtil.BuildOutPath,assetBundleBuilds.ToArray(),BuildAssetBundleOptions.None, target);

    }

   
}
