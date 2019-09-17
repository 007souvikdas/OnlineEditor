using OnlineEditor.Models;

public interface IFileStorage
{
    bool AddFile(string path,string name);
    bool SaveFile(string path,string name,string sourceCode);
    SourceCode GetFileContents(string path,string name);
    //Todo : Make it scalable for multiple users  
    string[] GetFileNames(string path);
    bool RemoveFile(string path,string name);


}