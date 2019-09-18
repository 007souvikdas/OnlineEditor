using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ParseSyntaxTree
{
    (bool, string) ClassHandler(ClassDeclarationSyntax classDeclarationSyntax)
    {
        string classNameWithMain = classDeclarationSyntax.Identifier.Text;
        foreach (var method in classDeclarationSyntax.ChildNodes())
        {
            Console.WriteLine($"\t{method}");
            MethodDeclarationSyntax methodDeclarationSyntax = method as MethodDeclarationSyntax;
            if (methodDeclarationSyntax != null)
            {
                string s2 = methodDeclarationSyntax.Modifiers.ToString();
                string p = methodDeclarationSyntax.Identifier.Text;
                if (p.Equals("Main") && (s2.Equals("public static") || s2.Equals("static public")))
                {
                    Console.WriteLine(classNameWithMain);
                    return (true, classNameWithMain);
                }
            }
        }
        return (false, string.Empty);
    }

    internal string ReturnClassnameWithMain(IEnumerable<SyntaxTree> trees)
    {
        string classNameWithMain = string.Empty;
        foreach (var tree in trees)
        {
            CompilationUnitSyntax compilationUnitSyntax = tree.GetCompilationUnitRoot();
            foreach (var className in compilationUnitSyntax.Members)
            {
                NamespaceDeclarationSyntax namespaceDeclarationSyntax = className as NamespaceDeclarationSyntax;
                if (namespaceDeclarationSyntax != null)
                {
                    foreach (var classname12 in namespaceDeclarationSyntax.Members)
                    {
                        ClassDeclarationSyntax classDeclarationSyntax = classname12 as ClassDeclarationSyntax;
                        if (classDeclarationSyntax != null)
                        {
                            var res = ClassHandler(classDeclarationSyntax);
                            if (res.Item1)
                            {
                                classNameWithMain = namespaceDeclarationSyntax.Name + "." + res.Item2;
                                break;
                            }
                        }
                    }
                }
                else
                {

                    ClassDeclarationSyntax classDeclarationSyntax = className as ClassDeclarationSyntax;
                    if (classDeclarationSyntax != null)
                    {
                        var res = ClassHandler(classDeclarationSyntax);
                        if (res.Item1)
                        {
                            classNameWithMain = res.Item2;
                            break;
                        }
                    }
                    else
                    {
                        classNameWithMain = string.Empty;
                    }
                }
            }
        }
        return classNameWithMain;

    }
}