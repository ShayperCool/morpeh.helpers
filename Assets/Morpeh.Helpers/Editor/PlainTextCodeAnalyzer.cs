using System.IO;
using System.Text.RegularExpressions;

namespace Editor
{
    public class PlainTextCodeAnalyzer
    {
        
        private const string ECS_NAMESPACE = "using Scellecs.Morpeh;";
        private const string DECLARTION_REGEX = @"^.*\s*struct\s+(\w+)\s*:\s*IComponent\s*";
        private const string NAMESPACE_REGEX = @"^\s*namespace\s+(\w.+)\s*";
        
        public bool TryGetComponentDescription(string code, out ComponentDescription componentDescription)
        {
            componentDescription = default;
            using var stringReader = new StringReader(code);
            var isEcsUsageFound = false;
            var namespaceFound = false;
            var namespaceName = "";
            var declarationFound = false;
            var typeName = "";
            while (true)
            {
                var line = stringReader.ReadLine();
                if (line is null)
                {
                    break;
                }
                    
                if (isEcsUsageFound == false && line.Contains(ECS_NAMESPACE))
                {
                    isEcsUsageFound = true;
                    continue;
                }
                    
                if (!namespaceFound)
                {
                    var match = Regex.Match(line, NAMESPACE_REGEX);
                    if(match.Success)
                    {
                        namespaceFound = true;
                        namespaceName = match.Groups[1].Value;
                        continue;
                    }
                }

                if (isEcsUsageFound && !declarationFound)
                {
                    var match = Regex.Match(line, DECLARTION_REGEX);
                    if(match.Success)
                    {
                        declarationFound = true;
                        typeName = match.Groups[1].Value;
                        continue;
                    }
                }
                    
                if(isEcsUsageFound && declarationFound && namespaceFound)
                {
                    componentDescription = new ComponentDescription
                    {
                        TypeName = typeName,
                        Namespace = namespaceName
                    };
                    return true;
                }
            }

            return false;
        }
    }
}