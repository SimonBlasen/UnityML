using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class FileLoader
{
    public static bool SaveToFile(List<PartBuilding> parts, string filename)
    {
        //string fileContent = "";
        //for (int i = 0; i < parts.Count; i++)
        //{
        //    fileContent += (int)parts[i].Type + ":";
        //    fileContent += parts[i].Position.ToString() + ";\n";
        //}
        //
        //File.WriteAllText(filename, fileContent);

        /*if (File.Exists(filename))
        {
            byte[] fileBytes = File.ReadAllBytes(filename);
            bool allOnes = false;
            if (fileBytes.Length >= 32)
            {
                allOnes = true;
                for (int i = 0; i < 32; i++)
                {
                    if (fileBytes[i] != 255)
                    {
                        allOnes = false;
                        break;
                    }
                }
            }

            if (allOnes == false)
            {
                CarPropertiesSetting cps = new CarPropertiesSetting();
                byte[] cpsBytes = cps.ToBytes();
                byte[] newFileBytes = new byte[cpsBytes.Length + 4 + 32 + fileBytes.Length];
                for (int i = 0; i < 32; i++)
                {
                    newFileBytes[i] = 255;
                }
                newFileBytes[32] = (byte)(cpsBytes.Length >> 24);
                newFileBytes[33] = (byte)(cpsBytes.Length >> 16);
                newFileBytes[34] = (byte)(cpsBytes.Length >> 8);
                newFileBytes[35] = (byte)(cpsBytes.Length);
                for (int i = 0; i < cpsBytes.Length; i++)
                {
                    newFileBytes[i + 36] = cpsBytes[i];
                }
                for (int i = 0; i < fileBytes.Length; i++)
                {
                    newFileBytes[i + 36 + cpsBytes.Length] = fileBytes[i];
                }
                File.WriteAllBytes(filename, newFileBytes);
            }
        }*/

        List<byte> bytes = new List<byte>();

        for (int i = 0; i < parts.Count; i++)
        {
            byte[] typeBytes = BitConverter.GetBytes((int)parts[i].Type);
            byte[] directionBytes = BitConverter.GetBytes((int)parts[i].Direction);
            byte[] rotationBytes = BitConverter.GetBytes((int)parts[i].Rotation);
            byte[] positionXBytes = BitConverter.GetBytes(parts[i].Position.x);
            byte[] positionYBytes = BitConverter.GetBytes(parts[i].Position.y);
            byte[] positionZBytes = BitConverter.GetBytes(parts[i].Position.z);

            if (typeBytes.Length == 4 && directionBytes.Length == 4 && rotationBytes.Length == 4 && positionXBytes.Length == 4 && positionYBytes.Length == 4 && positionZBytes.Length == 4)
            {
                bytes.AddRange(typeBytes);
                bytes.AddRange(directionBytes);
                bytes.AddRange(rotationBytes);
                bytes.AddRange(positionXBytes);
                bytes.AddRange(positionYBytes);
                bytes.AddRange(positionZBytes);
            }
            else
            {
                return false;
            }
        }

        File.WriteAllBytes(filename, bytes.ToArray());
        /*
        List<byte> settingsBytes = new List<byte>();
        if (File.Exists(filename))
        {
            byte[] curFileBytes = File.ReadAllBytes(filename);
            for (int i = 0; i < 36; i++)
            {
                settingsBytes.Add(curFileBytes[i]);
            }
            int lengthOfSettings = (curFileBytes[32] << 24) | (curFileBytes[33] << 16) | (curFileBytes[34] << 8) | (curFileBytes[35]);
            for (int i = 0; i < lengthOfSettings; i++)
            {
                settingsBytes.Add(curFileBytes[36 + i]);
            }
        }
        else
        {
            CarPropertiesSetting cps = new CarPropertiesSetting();
            byte[] cpsBytes = cps.ToBytes();
            for (int i = 0; i < 32; i++)
            {
                settingsBytes.Add(255);
            }
            settingsBytes.Add((byte)(cpsBytes.Length >> 24));
            settingsBytes.Add((byte)(cpsBytes.Length >> 16));
            settingsBytes.Add((byte)(cpsBytes.Length >> 8));
            settingsBytes.Add((byte)(cpsBytes.Length));
            for (int i = 0; i < cpsBytes.Length; i++)
            {
                settingsBytes.Add(cpsBytes[i]);
            }
        }

        List<byte> allBytes = new List<byte>();
        allBytes.AddRange(settingsBytes);
        allBytes.AddRange(bytes);

        File.WriteAllBytes(filename, bytes.ToArray());*/

        return true;
    }

    public static bool SavePropsToFile(CarPropertiesSetting setting, string filename)
    {
        List<string> lines = new List<string>();
        PropertyInfo[] props = setting.GetType().GetProperties();
        for (int i = 0; i < props.Length; i++)
        {
            if (props[i].CanRead && props[i].CanWrite)
            {
                lines.Add(props[i].Name + ":" + props[i].PropertyType.Name + "=" + props[i].GetValue(setting, null));
            }
        }

        File.WriteAllLines(filename, lines.ToArray());

        return true;
    }

    public static CarPropertiesSetting LoadSettingFromFile(string filename)
    {
        CarPropertiesSetting cps = new CarPropertiesSetting();

        PropertyInfo[] props = cps.GetType().GetProperties();

        if (File.Exists(filename))
        {
            string[] lines = File.ReadAllLines(filename);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Split(':').Length == 2 && lines[i].Split('=').Length == 2)
                {
                    string propName = lines[i].Split(':')[0];
                    string datatype = lines[i].Split(':')[1].Split('=')[0];
                    string strValue = lines[i].Split('=')[1];

                    for (int j = 0; j < props.Length; j++)
                    {
                        if (props[j].Name == propName)
                        {
                            props[j].SetValue(cps, getValFromStr(datatype, strValue), null);
                            break;
                        }
                    }
                }
            }
        }

        return cps;
    }

    public static PartConfiguration[] LoadFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            byte[] bytes = File.ReadAllBytes(filename);

            return LoadFromBytes(bytes);
        }
        else
        {
            return null;
        }
    }

    public static PartConfiguration[] LoadFromBytes(byte[] bytes)
    {
        if (bytes.Length % 24 == 0)
        {
            PartConfiguration[] configs = new PartConfiguration[bytes.Length / 24];

            for (int i = 0; i < bytes.Length; i += 24)
            {
                configs[i / 24] = new PartConfiguration();
                configs[i / 24].partType = (PartType)BitConverter.ToInt32(bytes, i);
                configs[i / 24].partDirection = (PartDirection)BitConverter.ToInt32(bytes, i + 4);
                configs[i / 24].partRotation = (PartRotation)BitConverter.ToInt32(bytes, i + 8);
                configs[i / 24].partPosition = new Vector3Int();
                configs[i / 24].partPosition.x = BitConverter.ToInt32(bytes, i + 12);
                configs[i / 24].partPosition.y = BitConverter.ToInt32(bytes, i + 16);
                configs[i / 24].partPosition.z = BitConverter.ToInt32(bytes, i + 20);
            }

            return configs;
        }
        else
        {
            return null;
        }
    }

    public static ConnectorFile LoadConnFile(string filename)
    {
        ConnectorFile cf = new ConnectorFile();
        if (! File.Exists(filename))
        {
            string[] newlines = new string[1];
            newlines[0] = "propertiesfile=";
            File.WriteAllLines(filename, newlines);
        }

        string[] lines = File.ReadAllLines(filename);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Split('=').Length == 2)
            {
                if (lines[i].Split('=')[0] == "propertiesfile")
                {
                    cf.PropertiesFile = lines[i].Split('=')[1];
                }
            }
        }

        return cf;
    }

    public static void SaveConnFile(string filename, ConnectorFile connFile)
    {
        string[] lines = new string[1];
        lines[0] = "propertiesfile=" + connFile.PropertiesFile;
        File.WriteAllLines(filename, lines);
    }

    private static object getValFromStr(string strType, string strValue)
    {
        switch (strType)
        {
            case "Single":
                return Convert.ToSingle(strValue);
            case "Boolean":
                return Convert.ToBoolean(strValue);
            case "Int32":
                return Convert.ToInt32(strValue);
            case "String":
                return strValue;
            case "Byte":
                return Convert.ToByte(strValue);
            case "Double":
                return Convert.ToDouble(strValue);
            default:
                return null;
        }
    }
}
