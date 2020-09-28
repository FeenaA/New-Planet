using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

#if UNITY_EDITOR 
using UnityEditor; 
#endif


public class XMLOperations : MonoBehaviour
{
    public void GetAll()
    {
        // download settings (isFirstGame, language, achievements, amountOfStars, unlockedLevels,
        // if saved session exists, settingsOfSavedSession () )
        //Preprocessing.sPrepSettings = GetPersonalSettings(PathPersonalSettings);

        GetTextHelp("Help");

        getItems.sGreekAlph = GetTextFromXML("GreekAlph");
        getItems.sIntroduction = GetTextFromXML1("Introduction");

        getItems.ResourceAdd = GetTextFromXML3("ExtraResources");
        getItems.ResNess = GetTextFromXML3("NesessResources");

        
    }

    private List<string> GetTextFromXML(string path)
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
    private Dictionary<int, string> GetTextFromXML1(string path)
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

                if (attr.Value == language)
                {
                    // обходим все дочерние узлы элемента language
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        result[n++] = childnode.InnerText;
                    }
                }
            }
        }
        return result;
    }

    // with detector of languages and keys (numbers)
    private Dictionary<int, getItems.ResourceInformation> GetTextFromXML3(string path)
    {
        Dictionary<int, getItems.ResourceInformation> result = 
            new Dictionary<int, getItems.ResourceInformation>();
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
                        XmlNode attrCost = childnode.Attributes.GetNamedItem("cost");
                        result[System.Convert.ToInt32(attrNum.Value)] = 
                            new getItems.ResourceInformation(attrName.Value, System.Convert.ToInt32(attrCost.Value)) ;
                    }
                }
            }
        }
        return result;
    }
     
    private void GetTextHelp(string path)
    {
        Preprocessing.strHelp = new string[4];
        Preprocessing.strHelpTitle = new string[4];

        string Language = "English";
        if (PersonalSettings.language == LanguageSettings.Language.English)
        { settings.nLanguage = 1; Language = "English"; }
        else
        {
            if (PersonalSettings.language == LanguageSettings.Language.Russian)
            { settings.nLanguage = 0; Language = "Russian"; }
        }
         
        TextAsset xmlData = Resources.Load(path) as TextAsset;
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xmlData.text);

        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == Language)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    int n = 5;
                            if (childnode.Name == "General")    { n = 0; }
                    else {  if (childnode.Name == "Buildings")  { n = 1; }
                    else {  if (childnode.Name == "Researches") { n = 2; }
                    else {  if (childnode.Name == "Resources")  { n = 3; }}}}

                    if (childnode.Attributes.Count > 0) { Preprocessing.strHelpTitle[n] = childnode.Attributes.GetNamedItem("name").Value; }
                    Preprocessing.strHelp[n] = childnode.InnerText;
                }
            }
        }
    }


}
