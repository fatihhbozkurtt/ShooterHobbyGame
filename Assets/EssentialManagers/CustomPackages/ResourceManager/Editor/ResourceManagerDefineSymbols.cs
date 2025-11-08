#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

[InitializeOnLoad]
public class ResourceManagerDefineSymbols
{
    static readonly string defineSymbol = "CUSTOM_RESOURCE_INCLUDED";
    static ResourceManagerDefineSymbols()
    {
        string scriptingDefinesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> scriptingDefinesStringList = new List<string>(scriptingDefinesString.Split(';'));

        if (!scriptingDefinesString.Contains(defineSymbol))
        {
            scriptingDefinesStringList.Add(defineSymbol);
            string defines = string.Join(";", scriptingDefinesStringList.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
        }
    }
}
#endif
