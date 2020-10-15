using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class PersonalSettings : MonoBehaviour
{
    // sttings to be readable from the other scenes of the project
    public static LanguageSettings.Language language;
    public static bool flagSavedGame;
    public static bool flagMusic;
    public static bool flagFirstGame;

    private static StreamWriter file;
    public static string PathPersonalSettings;
    private static XmlDocument xDoc = new XmlDocument();

    public void GetPersonalSettings()
    {
        // create or read file with personal settings
        PathPersonalSettings = Application.persistentDataPath + "/PersonalSettings.xml";
        //"C:/Users/Feena/AppData/LocalLow/Feena/New Planet/test.txt"
        string stringPersonalSettings =
        "<?xml version=\"1.0\"?><PersonalSettings><firstGame>true</firstGame><language>Russian</language><savedGame>false</savedGame><music>True</music><NBlue>2</NBlue></PersonalSettings>";

        // if file doesn't exist, create it and fill 
        if (!File.Exists(PathPersonalSettings))
        {
            file = File.CreateText(PathPersonalSettings);
            file.WriteLine(stringPersonalSettings);
            file.Close();

            flagFirstGame = true;
        }
        // file exists -> just read it
        else
        {
            stringPersonalSettings = File.ReadAllText(PathPersonalSettings);
            flagFirstGame = false;
        }

        Parse(stringPersonalSettings);
    }

    /// <summary>
    /// fill all settings
    /// </summary>
    /// <param name="str">the contents of file PersonalSettings</param>
    private void Parse(string str)
    {
        // Create the XmlDocument.
        xDoc.LoadXml(str);

        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == "savedGame")
            { flagSavedGame = (System.Convert.ToBoolean(xnode.InnerText) == true); }

            else if (xnode.Name == "language")
            {
                if (xnode.InnerText == "English") { language = LanguageSettings.Language.English; }
                else { if (xnode.InnerText == "Russian") { language = LanguageSettings.Language.Russian; } }
            }

            else if (xnode.Name == "music")
            { flagMusic = (System.Convert.ToBoolean(xnode.InnerText) == true); }

            else if (xnode.Name == "NBlue")
            { BlueCoin.sNBlueCoin = System.Convert.ToInt32(xnode.InnerText); }
        }
    }

    public void SetLanguage()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == "language")
            {
                if (language == LanguageSettings.Language.English)
                { xnode.InnerText = "English"; }
                else { if (language == LanguageSettings.Language.Russian)
                    { xnode.InnerText = "Russian"; }
                }
            }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        File.WriteAllText(PathPersonalSettings, sw.ToString());
    }

    public void SetFlagMusic()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == "music"){   xnode.InnerText = System.Convert.ToString(flagMusic); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        File.WriteAllText(PathPersonalSettings, sw.ToString());
    }

    public void SetFlagSavedGame(bool FlagSavedGame) 
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == "savedGame") { xnode.InnerText = System.Convert.ToString(FlagSavedGame); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        File.WriteAllText(PathPersonalSettings, sw.ToString());

        flagSavedGame = FlagSavedGame;
    }

    public void SetAchievements()
    {
         
    }
}
