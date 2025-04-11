namespace CssgoConsole;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
using CssgoConsoleLibrary;
using Godot;
using GodotPlugins;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using CSharpScript = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript;

public class CssgoConsoleHost
{
    public static void Main()
    {
        var host = new CssgoConsoleHost();
        host.ExecuteAsync("X+Y", new ExpandoObject()).Wait();
    }

    private ScriptOptions _scriptOptions;
    public ScriptOptions ScriptOptions => _scriptOptions;

    public CssgoConsoleHost()
    {
        _scriptOptions = ScriptOptions.Default
            .AddReferences(
                typeof(object).Assembly,
                typeof(Console).Assembly,
                typeof(System.Linq.Enumerable).Assembly,
                typeof(List<Globals>).Assembly,
                typeof(CodeEdit).Assembly,
                typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly,
                typeof(ExpandoObject).Assembly
            )
            .AddImports(
                "System",
                "System.Linq",
                "CssgoConsoleLibrary"
            );
    }

    public async Task<object> ExecuteAsync(string code, dynamic vars)
    {
        using var loader = new InteractiveAssemblyLoader();
        var assemblyLoadContext = PluginLoadContext.Default;
        foreach (var assembly in assemblyLoadContext.Assemblies)
        {
            loader.RegisterDependency(assembly);
        }

        Console.WriteLine(await CSharpScript.Create(code, options: ScriptOptions, assemblyLoader: loader).RunAsync());
        return 1;
    }
}
