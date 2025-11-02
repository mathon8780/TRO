using System;
using System.Collections.Generic;
using System.IO;
using Inventory;
using Inventory.PropertyData;
using Inventory.PropertyLoader;
using UnityEditor;
using UnityEngine;


namespace Editor
{
    public class CsvImporter : EditorWindow
    {
        public string mainCsvFilePath = "Assets/Configs/Inventory/ItemsData.csv"; // Main CSV File Path
        public string itemAssetSavePath = "Assets/Configs/Inventory/Items"; // Item Save Directory
        public string propertiesCsvFolder = "Assets/Configs/Inventory/Property"; // Property CSV Folder

        private readonly List<IPropertyLoader> _propertyLoader = new()
        {
            new EatablePropertyLoader(),
            new DurablePropertyLoader(),
            new ItemContainerPropertyLoader()
        };

        [MenuItem("Tools/Import CSV to PropertyDatabase")] // 添加选项 为当前的内容创建窗口
        public static void ShowWindow()
        {
            GetWindow<CsvImporter>("CSV Importer"); // 窗口名称为 CSV Importer
        }

        void OnGUI() // 绘制窗口
        {
            GUILayout.Label("CSV 转 PropertyDatabase 导入器", EditorStyles.boldLabel);

            // 主 CSV 文件选择器
            mainCsvFilePath = EditorGUILayout.TextField("主物品 CSV 路径", mainCsvFilePath);
            if (GUILayout.Button("选择主 CSV 文件"))
            {
                mainCsvFilePath = EditorUtility.OpenFilePanel("选择主 CSV 文件", "Assets", "csv");
            }

            // 属性文件夹选择器
            propertiesCsvFolder = EditorGUILayout.TextField("属性 CSV 文件夹", propertiesCsvFolder);
            if (GUILayout.Button("选择属性文件夹"))
            {
                propertiesCsvFolder = EditorUtility.OpenFolderPanel("选择属性文件夹", "Assets", "");
            }

            // 资源保存文件夹选择器
            itemAssetSavePath = EditorGUILayout.TextField("保存资源文件夹", itemAssetSavePath);
            if (GUILayout.Button("选择保存文件夹"))
            {
                string folder = EditorUtility.OpenFolderPanel("选择保存文件夹", "Assets", "");
                if (!string.IsNullOrEmpty(folder) && folder.StartsWith(Application.dataPath))
                {
                    itemAssetSavePath = "Assets" + folder.Substring(Application.dataPath.Length);
                }
            }

            if (GUILayout.Button("导入 CSV"))
            {
                if (string.IsNullOrEmpty(mainCsvFilePath) || !File.Exists(mainCsvFilePath))
                {
                    Debug.Log(mainCsvFilePath);
                    Debug.LogError("主 CSV 文件路径无效。");
                    return;
                }

                if (string.IsNullOrEmpty(propertiesCsvFolder) || !Directory.Exists(propertiesCsvFolder))
                {
                    Debug.LogError("属性文件夹路径无效。");
                    return;
                }

                ImportCsvToDatabase();
            }
        }

        /// <summary>
        /// 文件解析主逻辑
        /// </summary>
        void ImportCsvToDatabase()
        {
            Dictionary<int, List<ItemPropertyData>> allPropertiesMap = new Dictionary<int, List<ItemPropertyData>>();

            // Load properties
            foreach (var loader in _propertyLoader)
            {
                var loaderProperty = loader.LoadProperties(propertiesCsvFolder);
                foreach (var property in loaderProperty)
                {
                    int id = property.Key;
                    ItemPropertyData propertyData = property.Value;
                    if (!allPropertiesMap.ContainsKey(id))
                        allPropertiesMap[id] = new List<ItemPropertyData>();
                    allPropertiesMap[id].Add(propertyData);
                }
            }

            List<string> allItemPath = new List<string>();
            string[] lines = File.ReadAllLines(mainCsvFilePath); //读取整个mainCSV文件

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue; //跳过空白行

                string[] values = lines[i].Split(','); //按逗号分隔每一列

                try
                {
                    // 创建物品基础数据
                    int itemId = int.Parse(values[0]);
                    ItemData data = CreateInstance<ItemData>();
                    {
                        data.itemID = itemId;
                        data.itemName = string.IsNullOrWhiteSpace(values[1]) ? "null" : values[1].Trim();
                        data.itemWeight = float.TryParse(values[2], out float weight) ? weight : -1f;
                        data.canStack = bool.TryParse(values[3], out bool canStack) && canStack;
                        data.icon = null;
                        data.properties = new List<ItemPropertyData>();
                    }

                    // 获取物品的特定属性
                    if (allPropertiesMap.TryGetValue(itemId, out List<ItemPropertyData> properties))
                    {
                        data.properties.AddRange(properties);
                    }

                    // 保存单个物品文件资产
                    if (!AssetDatabase.IsValidFolder(itemAssetSavePath))
                    {
                        Directory.CreateDirectory(itemAssetSavePath);
                    }

                    string itemPath = $"{itemAssetSavePath}/{data.itemID}_{data.itemName}.asset";
                    AssetDatabase.CreateAsset(data, itemPath);
                    allItemPath.Add(itemPath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing lines[i]: {lines[i]}\n{e}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            List<ItemData> itemList = new List<ItemData>();
            //在磁盘中加载保存的文件 并关联到db数据中保存
            foreach (var assetPath in allItemPath)
            {
                ItemData loadItemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
                if (loadItemData != null)
                {
                    itemList.Add(loadItemData);
                }
                else
                {
                    Debug.LogError($"Failed to load item data from {assetPath}");
                }
            }

            UpdatePropertyDatabase(itemList); //这里传入的内容为内存中的内容 不是硬盘中的内容 因此无法正确的保存下来数据
            Debug.Log($"Successfully imported {itemList.Count} items from CSV to {itemAssetSavePath}");
        }


        void UpdatePropertyDatabase(List<ItemData> items)
        {
            string dbFilePath = "Assets/Configs/Inventory/Items/ItemDataBase.asset";
            ItemDataBase db = AssetDatabase.LoadAssetAtPath<ItemDataBase>(dbFilePath);
            if (db == null)
            {
                db = CreateInstance<ItemDataBase>();
                AssetDatabase.CreateAsset(db, dbFilePath);
                db = AssetDatabase.LoadAssetAtPath<ItemDataBase>(dbFilePath);
            }

            db.items = items;
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}