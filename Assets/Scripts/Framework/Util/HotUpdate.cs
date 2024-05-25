using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HotUpdate : MonoBehaviour
{
    private byte[] M_ReadPathFileData;
    private byte[] M_ServerFileListData;

    private void Start()
    {
        if (IsFirstInstall())
        {
            ReleaseResources();
        }
        else
        {
            CheckUpdate();
        }
    }
    private bool IsFirstInstall()
    {
        //判断只读目录是否存在版本文件
        bool isExistReadPath = FileUtil.IsExists(Path.Combine(PathUtil.ReadPath, AppConst.FileListName));

        //判断可读写目录是否存在版本文件
        bool isExistReadWritePath = FileUtil.IsExists(Path.Combine(PathUtil.ReadWritePath,AppConst.FileListName));

        return isExistReadPath && !isExistReadWritePath;
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void ReleaseResources()
    {
        string url = Path.Combine(PathUtil.ReadPath,AppConst.FileListName);
        DownFileInfo info = new DownFileInfo();
        info.url = url;
        StartCoroutine(DownLoadFile(info,OnDownLoadReadPathFileListComplete));
    }

    private void OnDownLoadReadPathFileListComplete(DownFileInfo file)
    {
        M_ReadPathFileData = file.fileData.data;
        List<DownFileInfo> fileInfos = GetFileList(file.fileData.text, PathUtil.ReadPath);
        StartCoroutine(DownLoadFiles(fileInfos, OnReleaseFileComplete, OnReleaseAllFileComplete)); ;
    }

    private void OnReleaseAllFileComplete()
    {
        FileUtil.WriteFile(Path.Combine(PathUtil.ReadWritePath,AppConst.FileListName),M_ReadPathFileData);
        CheckUpdate();
    }

    /// <summary>
    /// 释放资源文件
    /// </summary>
    /// <param name="fileInfo"></param>
    private void OnReleaseFileComplete(DownFileInfo fileInfo)
    {
        Debug.Log("OnReleaseFileComplete" + fileInfo.url);
        string writeFile = Path.Combine(PathUtil.ReadWritePath, fileInfo.fileName);
        FileUtil.WriteFile(writeFile,fileInfo.fileData.data);
    }

    private void CheckUpdate()
    {
       string url =  Path.Combine(AppConst.ResourceUrl, AppConst.FileListName);
        DownFileInfo info = new DownFileInfo();
        info.url = url;
        StartCoroutine(DownLoadFile(info, OnDownLoadServerFileListComplete));
    }

    private void OnDownLoadServerFileListComplete(DownFileInfo info)
    {
        M_ServerFileListData = info.fileData.data;  
        List<DownFileInfo> fileInfos = GetFileList(info.fileData.text, AppConst.ResourceUrl);
        List<DownFileInfo> downFileInfos = new List<DownFileInfo>();
        for (int i = 0; i < fileInfos.Count; i++)
        {
          string localFile = Path.Combine(PathUtil.ReadWritePath, fileInfos[i].fileName);
            if (!FileUtil.IsExists(localFile))
            {
               fileInfos[i].url = Path.Combine(AppConst.ResourceUrl, fileInfos[i].fileName);
               downFileInfos.Add(fileInfos[i]);
            }
        }
        if (downFileInfos.Count>0)
        {
            StartCoroutine(DownLoadFiles(fileInfos, OnUpdateFileComplete,OnUpdateAllFileComplete));
        }
    }

    /// <summary>
    /// 下载更新资源
    /// </summary>
    /// <param name="info"></param>
    private void OnUpdateFileComplete(DownFileInfo info)
    {
        Debug.Log("OnUpdateFileComplete" + info.url);
        string writeFile = Path.Combine(PathUtil.ReadWritePath, info.fileName);
        FileUtil.WriteFile(writeFile, info.fileData.data);
    }

    private void OnUpdateAllFileComplete()
    {
        FileUtil.WriteFile(Path.Combine(PathUtil.ReadWritePath,AppConst.FileListName),M_ServerFileListData);
        EnterGame();
    }


    internal class DownFileInfo
    {
        public string fileName;
        public string url;
        public DownloadHandler fileData;
    }

    IEnumerator DownLoadFile(DownFileInfo info,Action<DownFileInfo> Complete)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(info.url);
        yield return webRequest.SendWebRequest();
        if (webRequest.isHttpError|| webRequest.isNetworkError)
        {
            Debug.Log("网络错误"+ info.url);
            yield break;
        }
        info.fileData = webRequest.downloadHandler;
        Complete?.Invoke(info);
        webRequest.Dispose();
    }
    IEnumerator DownLoadFiles(List<DownFileInfo> infos, Action<DownFileInfo> Complete,Action DownloadAllComplete)
    {
        foreach (DownFileInfo info in infos)
        {
            yield return DownLoadFile(info,Complete);
        }

        DownloadAllComplete?.Invoke();
    }

    private List<DownFileInfo> GetFileList(string FileData,string path)
    {
        string content = FileData.Trim().Replace("\r", "");
        string[] files  =  content.Split('\n');
        List<DownFileInfo> downFileInfos = new List<DownFileInfo>(files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            string[] info = files[i].Split('|');
            DownFileInfo fileInfo = new DownFileInfo();
            fileInfo.fileName = info[1];
            fileInfo.url = Path.Combine(path, info[1]);
            downFileInfos.Add(fileInfo);
        }
        return downFileInfos;
    }
    private void EnterGame()
    {
        Manager.Resource.ParseVersionFile();
        Manager.Resource.LoadUI("Login/LoginUI", OnComplete);

    }

    private void OnComplete(UnityEngine.Object @object)
    {
        GameObject go = Instantiate(@object) as GameObject;
        go.transform.SetParent(transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }
}

