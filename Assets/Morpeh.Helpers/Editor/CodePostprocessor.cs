using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CodePostprocessor : AssetPostprocessor
    {
        private const string PROVIDER_TEMPLATE = "using Scellecs.Morpeh.Providers;\n\nnamespace $NAMESPACE\n{\n    public class $TYPE_NAMEProvider : MonoProvider<$TYPE_NAME>\n    {\n        \n    }\n}";

        private static readonly PlainTextCodeAnalyzer CodeAnalyzer = new PlainTextCodeAnalyzer();

        private void OnPreprocessAsset()
        {
            if (IsValidCodeAsset())
            {
                var code = File.ReadAllText(assetPath);
                if (CodeAnalyzer.TryGetComponentDescription(code, out var componentDescription))
                {
                    var providerPath = assetPath.Replace(".cs", "Provider.cs");
                    if (!File.Exists(providerPath))
                    {
                        var providerCode = PROVIDER_TEMPLATE.Replace("$NAMESPACE", componentDescription.Namespace)
                            .Replace("$TYPE_NAME", componentDescription.TypeName);
                        File.WriteAllText(providerPath, providerCode);
                    }
                }
            }
        }

        private bool IsValidCodeAsset()
        {
            return assetPath.EndsWith(".cs") && !assetPath.Contains(nameof(CodePostprocessor));
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var deletedAsset in deletedAssets)
            {
                var guid = AssetDatabase.AssetPathToGUID(deletedAsset);
            }
        }
    }
}