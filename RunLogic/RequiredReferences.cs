using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

public class RequiredReferences
{
    public static MetadataReference[] GetMetadataReferences()
    {
        List<MetadataReference> list = new List<MetadataReference>();
        list.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
        var dotnetCoreDirectory = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);

        list.Add(MetadataReference.CreateFromFile(typeof(CSharpCompilation).GetTypeInfo().Assembly.Location));
        list.Add(MetadataReference.CreateFromFile(typeof(SyntaxToken).GetTypeInfo().Assembly.Location));
        list.Add(MetadataReference.CreateFromFile(typeof(FileStream).GetTypeInfo().Assembly.Location));
        list.Add(MetadataReference.CreateFromFile(typeof(Console).GetTypeInfo().Assembly.Location));
        //list.Add(MetadataReference.CreateFromFile(typeof(AppDomain).GetTypeInfo().Assembly.Location));
        list.Add(MetadataReference.CreateFromFile(typeof(Encoding).GetTypeInfo().Assembly.Location));
        list.Add(MetadataReference.CreateFromFile(typeof(CancellationToken).GetTypeInfo().Assembly.Location));
        list.Add(MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "System.Runtime.dll")));
        return list.ToArray();
    }
}