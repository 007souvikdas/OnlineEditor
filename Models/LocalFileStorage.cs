using System;
using OnlineEditor.Models;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

public class LocalFileStorage : IFileStorage
{
    IConfiguration myConfiguration;
    string basePath;
    public LocalFileStorage(IConfiguration configuration)
    {
        myConfiguration = configuration;
        basePath = myConfiguration["CompilePath"];
    }
    
    public bool AddFile(string path, string name)
    {
        bool result = false;
        try
        {
            string dirPath = Path.Combine(basePath, path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (File.OpenWrite(Path.Combine(basePath, path, name)))
            {
                result = true;
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Some excetion has occured:" + e.Message);
        }
        return result;
    }

    public SourceCode GetFileContents(string path, string name)
    {
        SourceCode sourceCode = new SourceCode();
        var result = File.ReadAllText(Path.Combine(basePath, path, name));
        if (result != null)
        {
            sourceCode.FileName = name;
            sourceCode.sourceContent = result;
        }
        return sourceCode;
    }

    public string[] GetFileNames(string path)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(basePath, path));
        List<string> fileLists = new List<string>();
        if (directoryInfo.Exists)
        {
            foreach (var dir in directoryInfo.GetFiles("*.cs"))
            {
                fileLists.Add(dir.Name);
            }
        }
        return fileLists.ToArray();
    }

    public bool RemoveFile(string path, string name)
    {
        bool result = false;
        try
        {
            File.Delete(Path.Combine(basePath, path, name));
            result = true;
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Some exception occured with message:" + e.Message);
        }
        return result;
    }

    public bool SaveFile(string path, string name, string sourceCode)
    {
        bool result = false;
        try
        {
            Console.WriteLine("Inside save file ops");
            string dirPath = Path.Combine(basePath, path);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string fullPath = Path.Combine(basePath, path, name);
            File.WriteAllText(fullPath, sourceCode);
            result = true;
            Console.WriteLine("file saved successfully at path:" + fullPath);
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Some exception occured with message:" + e.Message);
        }
        return result;
    }
}