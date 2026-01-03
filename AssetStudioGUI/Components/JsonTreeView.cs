using AssetStudio;
using System.Text.Json;
using System.Windows.Forms;

namespace AssetStudioGUI
{
    public static class JsonTreeView
    {
        public static void LoadFromJson(this TreeView treeView, JsonDocument jsonDoc, string rootName)
        {
            if (jsonDoc == null)
            {
                Logger.Info("Unable to build tree view of current object");
                return;
            }

            try
            {
                treeView.BeginUpdate();
                treeView.Nodes.Clear();
                var rootNode = treeView.Nodes[treeView.Nodes.Add(new TreeNode(rootName))];

                foreach (var property in jsonDoc.RootElement.EnumerateObject())
                {
                    var childNode = rootNode.Nodes[rootNode.Nodes.Add(new TreeNode(property.Name))];
                    AddNodes(property.Value, childNode);
                }

                rootNode.Expand();
                rootNode.EnsureVisible();
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        private static void AddNodes(JsonElement jsonElement, TreeNode treeNode)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in jsonElement.EnumerateObject())
                    {
                        var childNode = treeNode.Nodes[treeNode.Nodes.Add(new TreeNode(property.Name))];
                        AddNodes(property.Value, childNode);
                    }
                    break;
                case JsonValueKind.Array:
                    const int arrayLenLimit = 500;
                    var arrayLen = jsonElement.GetArrayLength();
                    if (arrayLen == 0)
                    {
                        SetValue(jsonElement, treeNode);
                    }
                    else
                    {
                        var i = 0;
                        foreach (var jsonArrayElem in jsonElement.EnumerateArray())
                        {
                            if (jsonArrayElem.ValueKind == JsonValueKind.Number) //number array
                            {
                                SetValue(jsonElement, treeNode);
                                break;
                            }

                            if (i > arrayLenLimit)
                            {
                                treeNode.Nodes.Add(new TreeNode($"[{i++}-{arrayLen - 1}] Skipped. Too many elements to display"));
                                break;
                            }

                            var childNode = treeNode.Nodes[treeNode.Nodes.Add(new TreeNode($"[{i++}]"))];
                            AddNodes(jsonArrayElem, childNode);
                        }
                    }
                    break;
                default:
                    SetValue(jsonElement, treeNode);
                    break;
            }
        }

        private static void SetValue(JsonElement jsonElem, TreeNode node)
        {
            const int maxStrLen = 128;
            var endStr = "...";
            var strValue = jsonElem.ToString();
            if (jsonElem.ValueKind == JsonValueKind.Array)
            {
                strValue = strValue.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace(",", ", ");
                endStr += "]";
            }
            if (jsonElem.ValueKind == JsonValueKind.Null)
            {
                strValue = jsonElem.GetRawText();
            }
            node.Name = node.Text;
            node.Text += strValue?.Length > maxStrLen
                ? $": {strValue.Substring(0, maxStrLen)}{endStr}"
                : $": {strValue}";
            node.Tag = strValue;
        }
    }
}
