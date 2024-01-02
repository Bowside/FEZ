using System;
using Microsoft.SPOT;
using System.IO;
using System.Xml;
using System.Collections;

namespace HouseWatch
{
public static class ConfigurationManager
{
    private const string APPSETTINGS_SECTION = "appSettings";
    private const string ADD = "add";
    private const string KEY = "key";
    private const string VALUE = "value";
 
    private static Hashtable appSettings;
 
    static ConfigurationManager()
    {
        appSettings = new Hashtable();
    }
 
    public static string GetAppSetting(string key)
    {
        return GetAppSetting(key, null);
    }
 
    public static string GetAppSetting(string key, string defaultValue)
    {
        if (!appSettings.Contains(key))
            return defaultValue;
 
        return (string)appSettings[key];
    }
 
    public static void SetAppSetting(string key, string value)
    {
        appSettings[key] = value;
    }
 
    public static void Load(Stream xmlStream)
    {
        appSettings.Clear();
        using (XmlReader reader = XmlReader.Create(xmlStream))
        {
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case APPSETTINGS_SECTION:
                        while (reader.Read())
                        {
                            if (reader.Name == APPSETTINGS_SECTION)
                                break;
 
                            if (reader.Name == ADD)
                            {
                                var key = reader.GetAttribute(KEY);
                                var value = reader.GetAttribute(VALUE);
 
                                //Debug.Print(key + "=" + value);
                                appSettings.Add(key, value);
                            }
                        }
 
                        break;
                }
            }
        }
    }
 
    public static void Save(Stream xmlStream)
    {
        using (StreamWriter writer = new StreamWriter(xmlStream))
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<configuration>");
            writer.WriteLine("\t<appSettings>");
 
            foreach (DictionaryEntry item in appSettings)
            {
                string add = "\t\t<add key=\"" + item.Key + "\" value=\"" + item.Value + "\" />";
                writer.WriteLine(add);
            }
 
            writer.WriteLine("\t</appSettings>");
            writer.WriteLine("</configuration>");
        }
    }
}

}
