using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class readAll : MonoBehaviour
{
    public static void GetAll()
    {
        // download settings (isFirstGame, language, achievements, amountOfStars, unlockedLevels,
        // if saved session exists, settingsOfSavedSession () )

        getItems.sGreekAlph = GetTextFromXML("GreekAlph");
        getItems.ResourceAdd = GetTextFromXML1("ExtraResources");
        getItems.sIntroduction = GetTextFromXML1("Introduction");
        getItems.ResNess = GetTextFromXML2("NesessResources");
    }

    public static List<string> GetTextFromXML(string path)
    {
        List<string> result = new List<string>() { };

        #region //Здесь реализация через TextAsset
        TextAsset xmlData = Resources.Load(path) as TextAsset;
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xmlData.text);
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            result.Add(xnode.InnerText);
        }
        #endregion

        return result;
    }

    // with detector of languages 
    public static Dictionary<int, string> GetTextFromXML1(string path)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        int n = 1;
        string language = "";
        if (settings.nLanguage == 0) { language = "Russian"; }
        if (settings.nLanguage == 1) { language = "English"; }


        TextAsset xmlData = Resources.Load(path) as TextAsset;
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xmlData.text);
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            // получаем атрибут name
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("language");

                if (attr.Value== language)
                {
                    // обходим все дочерние узлы элемента user
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        result[n++] = childnode.InnerText;
                    }
                }
            }
        }
        return result;
    }

    // with detector of languages and keys
    public static Dictionary<int, string> GetTextFromXML2(string path)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        string language = "";
        if (settings.nLanguage == 0) { language = "Russian"; }
        if (settings.nLanguage == 1) { language = "English"; }


        TextAsset xmlData = Resources.Load(path) as TextAsset;
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xmlData.text);
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Attributes.Count > 0)
            {
                XmlNode attr = xnode.Attributes.GetNamedItem("language");

                if (attr.Value == language)
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        XmlNode attrNum = childnode.Attributes.GetNamedItem("number");
                        XmlNode attrName = childnode.Attributes.GetNamedItem("name");
                        result[System.Convert.ToInt32( attrNum.Value )] = attrName.Value;
                    }
                }
            }
        }
        return result;
    }
}
