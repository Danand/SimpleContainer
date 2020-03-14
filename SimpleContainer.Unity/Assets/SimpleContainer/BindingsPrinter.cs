using System;
using System.Collections.Generic;

using SimpleContainer.Extensions;

namespace SimpleContainer
{
    internal static class BindingsPrinter
    {
        private const string INDENTATION = "----";
        private const string CIRCULAR = " # CIRCULAR";

        public static string GetBindingsString(IList<DependencyNode> rootNodes, DependencyNode circularNode = null)
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

                result = PrintLinks(node, result, level, circularNode);

                result += Environment.NewLine;
            }

            return result;
        }

        private static string PrintLinks(DependencyNode node, string result, int level, DependencyNode circularNode)
        {
            level++;

            var indentation = INDENTATION.Repeat(level);

            foreach (var link in node.GetAllDependencies())
            {
                result += $"{indentation} ({link.KeyType.Name})";

                if (link.Node == null)
                {
                    result += " # NOT REGISTERED";
                }
                else
                {
                    result += $" {link.Node.ResultType.Name}";
                }

                if (link.Node != null && link.Node == circularNode)
                {
                    result += CIRCULAR;
                    result += Environment.NewLine;

                    break;
                }

                result += Environment.NewLine;

                if (link.Node != null)
                {
                    result = PrintLinks(link.Node, result, level, circularNode);
                }
            }

            return result;
        }
    }
}