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

        getItems.sGreekAlph = GetTextFromXML("Assets/5xml/GreekAlph.xml");
        getItems.sIntroduction = GetTextFromXML("Assets/5xml/Introduction.xml");

        getItems.ResourceAdd = GetTextFromXML1("Assets/5xml/Resources.xml");
    }

    public static List<string> GetTextFromXML(string path)
    {
        List<string> result = new List<string>() { };

        XmlTextReader Reader = new XmlTextReader(path);
        while (Reader.Read())
        {
            if (Reader.NodeType == XmlNodeType.Text)
            {
                result.Add(Reader.Value);
            }
        }
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


        // XmlTextReader Reader = new XmlTextReader();
        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);
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
                        // если узел - company
                        //if (childnode.Name == "resource")
                        //{
                            result[n++] = childnode.InnerText;
                        //}
                    }
                }
            }
        }
        return result;
    }
}
