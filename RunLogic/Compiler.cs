using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;

public class Compiler : ICompiler
{
    IConfiguration myConfiguration;
    string compilationPath;
    public Compiler(IConfiguration configuration)
    {
        myConfiguration = configuration;
        compilationPath = myConfiguration["CompilePath"];
    }
    //takes the files path and returns the path of compiled file
    public (bool, string) CompileFiles(string[] fileContents, string path)
    {
        List<SyntaxTree> trees = new List<SyntaxTree>();
        foreach (var fileContent in fileContents)
        {
            trees.Add(CSharpSyntaxTree.ParseText(fileContent));
        }

        string guid = Guid.NewGuid().ToString();
        string guidFileName = guid + ".dll";
        var dotnetCoreDirectory = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);
        System.Console.WriteLine("Dotnet core directory:" + dotnetCoreDirectory);
        DeleteUnwantedFiles(path);
        string message = string.Empty;

        string classNameWithMain = new ParseSyntaxTree().ReturnClassnameWithMain(trees);
        if (!string.IsNullOrEmpty(classNameWithMain))
        {
            var compilation = CSharpCompilation.Create(guidFileName)
                    .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication, mainTypeName: classNameWithMain))
                    .AddReferences(
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Console).GetTypeInfo().Assembly.Location),
                        MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "System.Runtime.dll")))
                    .AddSyntaxTrees(trees);
            ;
            string fileName = Path.Combine(compilationPath, path, guidFileName);
            string runtimeConfigPath = Path.Combine(compilationPath, "runtimeConfig", "Test.runtimeconfig.json");
            File.Copy(runtimeConfigPath, Path.Combine(compilationPath, path, guid + ".runtimeconfig.json"));

            var emitResult = compilation.Emit(fileName);

            if (!emitResult.Success)
            {
                foreach (var diagnostic in emitResult.Diagnostics)
                {
                    message += diagnostic;
                }
                return (false, message);
            }
        }
        else
        {
            message = "Main methd is not found in any of the classes";
            return (false, message);
        }
        return (true, guidFileName);
    }
    public void DeleteUnwantedFiles(string path)
    {
        string deletionPath = Path.Combine(compilationPath, path);
        foreach (string name in Directory.EnumerateFiles(deletionPath, "*.dll"))
        {
            try
            {
                File.Delete(name);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception while deleteing the file" + e.Message);
            }
        }
        foreach (string name in Directory.EnumerateFiles(deletionPath, "*.json"))
        {
            try
            {
                File.Delete(name);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("exception while deleteing the file" + e.Message);
            }
        }
    }
    public (bool, string) ExecuteFiles(string fileName, string path)
    {
        var fullPath = Path.Combine(compilationPath, path, fileName);
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "dotnet.exe";
            process.StartInfo.Arguments = fullPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
            //wait 2 mins
            process.WaitForExit(2 * 60 * 1000);

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                return (true, output);
            }
            else if (!string.IsNullOrEmpty(error))
            {
                return (true, error);
            }
            else
            {
                return (true, "No Output recieved from the program");
            }
        }
        catch (Exception e)
        {
            return (false, e.Message);
        }
    }
}
public interface ICompiler
{
    (bool, string) CompileFiles(string[] fileContents, string path);
    (bool, string) ExecuteFiles(string fileName, string path);
}