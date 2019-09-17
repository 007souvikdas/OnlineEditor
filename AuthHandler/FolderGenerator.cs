using System;
class FolderGenerator
{
    public string Generate(string IpAddress)
    {
        return IpAddress.ToString().Replace(".","");   
    }
}