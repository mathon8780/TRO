using System.Collections.Generic;
using System.IO;
using ItemInventory;
using ItemInventory.InteractiveBehaviours;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public static class ItemBaseInfoCsvImporter
    {
        [MenuItem("Tools/Import ItemBaseInfo from CSV")]
        public static void ImportCsv()
        {
            // 弹出文件选择窗口，仅限 .csv
            string csvPath = EditorUtility.OpenFilePanel("选择物品数据 CSV 文件", "", "csv");
            if (string.IsNullOrEmpty(csvPath)) return;
            // 读取所有行（使用 UTF-8 编码，避免中文乱码）
            string[] lines = File.ReadAllLines(csvPath, System.Text.Encoding.UTF8);
            if (lines.Length < 2)
            {
                Debug.LogError("CSV 文件至少需要包含表头和一行数据！");
                return;
            }

            // 解析表头
            string[] headers = ParseCsvLine(lines[0]);


            List<Dictionary<string, string>> records = new List<Dictionary<string, string>>();
            // 解析数据行
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue; // 跳过空行
                string[] values = ParseCsvLine(lines[i]);
                if (values.Length != headers.Length)
                {
                    Debug.LogWarning($"第 {i + 1} 行数据列数与表头不匹配，已跳过。");
                    continue;
                }

                Dictionary<string, string> record = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                {
                    record[headers[j]] = values[j];
                }

                records.Add(record);
            }

            // 创建输出文件夹（如果不存在）
            string assetFolderPath = "Assets/ItemData";
            if (!AssetDatabase.IsValidFolder(assetFolderPath))
            {
                AssetDatabase.CreateFolder("Assets", "ItemData");
            }

            int successCount = 0;
            // 遍历每条记录，创建 ScriptableObject
            foreach (var record in records)
            {
                try
                {
                    ItemBaseInfo item = ScriptableObject.CreateInstance<ItemBaseInfo>();
                    // 基本信息
                    item.itemId = int.Parse(record["itemId"]);
                    item.itemName = record["itemName"];
                    item.itemType = ParseEnum<E_ItemType>(record["itemType"]);
                    item.description = record["description"];
                    item.weight = float.Parse(record["weight"]);
                    item.canStack = bool.Parse(record["canStack"]);
                    // 资源加载信息（运行时使用，不加载实际资源）
                    item.iconLoadKey = record["iconLoadKey"];
                    item.resourceLoadKey = record["resourceLoadKey"];
                    // 行为组件（枚举数组）
                    string behaviorStr = record["behaviorTypes"];
                    if (!string.IsNullOrEmpty(behaviorStr))
                    {
                        // 移除可能的双引号（CSV 中用 "Pickable,Eatable" 包裹）
                        behaviorStr = behaviorStr.Trim('"');
                        string[] behaviorNames = behaviorStr.Split(',');
                        item.behaviorTypes = new E_ItemBehaviorType[behaviorNames.Length];
                        for (int i = 0; i < behaviorNames.Length; i++)
                        {
                            item.behaviorTypes[i] = ParseEnum<E_ItemBehaviorType>(behaviorNames[i].Trim());
                        }
                    }
                    else
                    {
                        item.behaviorTypes = new E_ItemBehaviorType[0];
                    }

                    // 生成资产路径
                    string assetPath = $"{assetFolderPath}/Item_{item.itemId:D4}.asset";

                    // 确保路径唯一（避免覆盖）
                    int counter = 1;
                    string originalPath = assetPath;
                    while (AssetDatabase.LoadAssetAtPath<ItemBaseInfo>(assetPath) != null)
                    {
                        assetPath = originalPath.Replace(".asset", $"_{counter}.asset");
                        counter++;
                    }

                    // 创建资产
                    AssetDatabase.CreateAsset(item, assetPath);
                    successCount++;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"导入物品数据时出错（ID: {record.GetValueOrDefault("itemId", "未知")}）: {e.Message}\n{e.StackTrace}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"✅ 成功导入 {successCount} 个物品数据到 {assetFolderPath}/");
        }

        /// <summary>
        /// 安全解析 CSV 行（处理带逗号的字段，如 "a,b",c → ["a,b", "c"]）
        /// 简化版：仅处理被双引号包裹的字段
        /// </summary>
        private static string[] ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            bool inQuotes = false;
            string currentField = "";
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }

            fields.Add(currentField); // 添加最后一个字段
            return fields.ToArray();
        }

        /// <summary>
        /// 安全解析枚举（忽略大小写）
        /// </summary>
        private static T ParseEnum<T>(string value) where T : struct
        {
            if (System.Enum.TryParse(value, true, out T result))
            {
                return result;
            }
            else
            {
                Debug.LogWarning($"无法解析枚举值 '{value}' 为类型 {typeof(T).Name}，使用默认值。");
                return default(T);
            }
        }
    }

    // 扩展方法：安全获取字典值
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.TryGetValue(key, out TValue value) ? value : defaultValue;
        }
    }
}