using System;
using System.Collections.Generic;

using SimpleContainer.Extensions;

namespace SimpleContainer
{
    internal static class BindingsPrinter
    {
        private const string INDENTATION = "----";
        private const string CIRCULAR = " # CIRCULAR";
        private const string NOT_REGISTERED = " # NOT REGISTERED";
        private const int CIRCULAR_LEVEL_MAX = 10;

        public static string GetBindingsString(
            IList<DependencyNode>   rootNodes,
            DependencyNode          circularNode = null,
            Type                    notRegisteredType = null)
        {
            var result = string.Empty;

            foreach (var node in rootNodes)
            {
                result += $"{INDENTATION} ({node.ContractType.Name}) {node.ResultType.Name}";

                if (node == circularNode)
                {
                    result += CIRCULAR;
                }

                result += Environment.NewLine;

                int level = 1;

                result = PrintLinks(node, result, level, circularNode, notRegisteredType);

                result += Environment.NewLine;
            }

            return result;
        }

        private static string PrintLinks(
            DependencyNode  node,
            string          result,
            int             level,
            DependencyNode  circularNode,
            Type            notRegisteredType)
        {
            level++;

            if (circularNode != null && level > CIRCULAR_LEVEL_MAX)
                return result;

            var indentation = INDENTATION.Repeat(level);

            foreach (var link in node.GetAllDependencies())
            {
                result += $"{indentation} ({link.KeyType.Name})";

                if (link.ContractType == notRegisteredType)
                {
                    result += NOT_REGISTERED;
                    result += Environment.NewLine;

                    break;
                }

                if (link.Node == null)
                {
                    result += Environment.NewLine;
                    break;
                }

                result += $" {link.Node.ResultType.Name}";

                if (link.Node == circularNode)
                {
                    result += CIRCULAR;
                    result += Environment.NewLine;

                    break;
                }

                result += Environment.NewLine;

                if (link.Node != null)
                {
                    result = PrintLinks(link.Node, result, level, circularNode, notRegisteredType);
                }
            }

            return result;
        }
    }
}