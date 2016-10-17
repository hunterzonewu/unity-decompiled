// Decompiled with JetBrains decompiler
// Type: UnityEditor.Scripting.Compilers.CSharpLanguage
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Modules;

namespace UnityEditor.Scripting.Compilers
{
  internal class CSharpLanguage : SupportedLanguage
  {
    public override string GetExtensionICanCompile()
    {
      return "cs";
    }

    public override string GetLanguageName()
    {
      return "CSharp";
    }

    internal static CSharpCompiler GetCSharpCompiler(BuildTarget targetPlatform, bool buildingForEditor, string assemblyName)
    {
      return ModuleManager.GetCompilationExtension(ModuleManager.GetTargetStringFromBuildTarget(targetPlatform)).GetCsCompiler(buildingForEditor, assemblyName);
    }

    public override ScriptCompilerBase CreateCompiler(MonoIsland island, bool buildingForEditor, BuildTarget targetPlatform, bool runUpdater)
    {
      switch (CSharpLanguage.GetCSharpCompiler(targetPlatform, buildingForEditor, island._output))
      {
        case CSharpCompiler.Microsoft:
          return (ScriptCompilerBase) new MicrosoftCSharpCompiler(island, runUpdater);
        default:
          return (ScriptCompilerBase) new MonoCSharpCompiler(island, runUpdater);
      }
    }

    public override string GetNamespace(string fileName)
    {
      using (IParser parser = ParserFactory.CreateParser(fileName))
      {
        parser.Parse();
        try
        {
          CSharpLanguage.NamespaceVisitor namespaceVisitor = new CSharpLanguage.NamespaceVisitor();
          CSharpLanguage.VisitorData visitorData = new CSharpLanguage.VisitorData() { TargetClassName = Path.GetFileNameWithoutExtension(fileName) };
          parser.get_CompilationUnit().AcceptVisitor((IAstVisitor) namespaceVisitor, (object) visitorData);
          return !string.IsNullOrEmpty(visitorData.DiscoveredNamespace) ? visitorData.DiscoveredNamespace : string.Empty;
        }
        catch
        {
        }
      }
      return string.Empty;
    }

    private class VisitorData
    {
      public string TargetClassName;
      public Stack<string> CurrentNamespaces;
      public string DiscoveredNamespace;

      public VisitorData()
      {
        this.CurrentNamespaces = new Stack<string>();
      }
    }

    private class NamespaceVisitor : AbstractAstVisitor
    {
      public NamespaceVisitor()
      {
        base.\u002Ector();
      }

      public virtual object VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, object data)
      {
        CSharpLanguage.VisitorData visitorData = (CSharpLanguage.VisitorData) data;
        visitorData.CurrentNamespaces.Push(namespaceDeclaration.get_Name());
        ((AbstractNode) namespaceDeclaration).AcceptChildren((IAstVisitor) this, (object) visitorData);
        visitorData.CurrentNamespaces.Pop();
        return (object) null;
      }

      public virtual object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
      {
        CSharpLanguage.VisitorData visitorData = (CSharpLanguage.VisitorData) data;
        if (typeDeclaration.get_Name() == visitorData.TargetClassName)
        {
          string str = string.Empty;
          using (Stack<string>.Enumerator enumerator = visitorData.CurrentNamespaces.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              str = !(str == string.Empty) ? current + "." + str : current;
            }
          }
          visitorData.DiscoveredNamespace = str;
        }
        return (object) null;
      }
    }
  }
}
