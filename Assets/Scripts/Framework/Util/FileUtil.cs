using System;
using System.IO;
using UnityEngine;

public class FileUtil 
{
    /// <summary>
    /// ����ļ��Ƿ����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsExists(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        return fileInfo.Exists;
    }
    /// <summary>
    /// д���ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void WriteFile(string path, byte[] data)
    {
        path = PathUtil.GetStandarPath(path);
        //�ļ���·��
        string dir = path.Substring(0, path.LastIndexOf("/"));
        if (!Directory.Exists(dir) )
        {
            Directory.CreateDirectory(dir);
        }
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            file.Delete();
        }
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
        catch (IOException e)
        {
            Debug.LogError(e);
        }
    }

    internal static void WriteFile(string v, object m_ReadPathFileData)
    {
        throw new NotImplementedException();
    }
}
