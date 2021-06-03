using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 文件读取
/// </summary>
public class FileHandle
{
    private FileHandle() { }

    public static readonly FileHandle Instance = new FileHandle();

    /// <summary>
    /// 该路径是否存在
    /// </summary>
    /// <param name="filepath">路径</param>
    /// <returns></returns>
    public bool IsExistFile(string filepath)
    {
        return File.Exists(filepath);
    }
    /// <summary>
    /// 保存文件，默认System.IO.FileMode.Create, System.IO.FileAccess.Write
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="FilePath"></param>
    public void SaveFile(string msg, string FilePath)
    {
        var fss = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        var sws = new System.IO.StreamWriter(fss);
        sws.Write(msg);
        sws.Close();
        fss.Close();
    }
    /// <summary>
    /// 判断文件夹是否存在
    /// </summary>
    /// <param name="Folderpath"></param>
    public void IsExisFolder(string Folderpath)
    {
        if (!System.IO.Directory.Exists(Folderpath))
            System.IO.Directory.CreateDirectory(Folderpath);
    }
    /// <summary>
    ///  读取文件内容
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public string FileToString(string filepath)
    {
        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(fs, Encoding.UTF8);
        try
        {
            return reader.ReadToEnd();
        }
        catch
        {
            return string.Empty;
        }
        finally
        {
            fs.Close();
            reader.Close();
        }
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filepath"></param>
    public void ClearFile(string filepath)
    {
        File.Delete(filepath);
    }
}