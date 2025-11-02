using System;
using System.Collections.Generic;
using System.IO;
using Inventory.PropertyData;
using UnityEngine;

namespace Inventory.PropertyLoader
{
    public class EatablePropertyLoader : IPropertyLoader
    {
        public string PropertyFileName => "EatablePropertyData.csv";

        public Dictionary<int, ItemPropertyData> LoadProperties(string propertyFolderPath)
        {
            var dic = new Dictionary<int, ItemPropertyData>();
            string filePath = Path.Combine(propertyFolderPath, PropertyFileName);
            if (!File.Exists(filePath))
            {
                Debug.LogError($"{filePath} not found");
                return dic;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    Debug.LogWarning(lines[i]);
                    continue;
                }

                string[] values = lines[i].Split(',');
                try
                {
                    int id = int.Parse(values[0]);
                    EatablePropertyData propertyData = new EatablePropertyData()
                    {
                        Cab = float.Parse(values[2]),
                        Fat = float.Parse(values[3]),
                        Protein = float.Parse(values[4]),
                        Water = float.Parse(values[5]),
                        Energy = float.Parse(values[6]),
                    };
                    dic.TryAdd(id, propertyData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            Debug.Log($"Loaded + {dic.Count} + from + {PropertyFileName}");
            return dic;
        }
    }
}