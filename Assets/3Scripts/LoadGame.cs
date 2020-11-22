using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class LoadGame : MonoBehaviour
{
    private static string fullPath = "";
    private static XmlDocument xDoc = new XmlDocument();
    private static StreamWriter file;

    /// <summary>
    /// copy all information from NewGameSettins to ContinueGame
    /// </summary>
    public void StartNew()
    {
        // NewGame reading
        string Path = "NewGameSettings";
        TextAsset xmlData = Resources.Load(Path) as TextAsset;
        string strNew = xmlData.text;

        // ContinueGame filling
        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        // the previous game isn't saved
        if (!File.Exists(fullPath))
        {
            file = File.CreateText(fullPath);
            file.WriteLine(strNew);
            file.Close();
        }
        // rewrite all file
        else
        {
            File.WriteAllText(fullPath, strNew);
        }

        // fill gameSettings by information from ContinueGame.xml
        settings.gameSettings = Parse(strNew);
    }

    /// <summary>
    /// fill gameSettings by information from ContinueGame.xml
    /// </summary>
    public void Continue()
    {
        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        string strCont = File.ReadAllText(fullPath);
        settings.gameSettings = Parse(strCont);
    }

    /// <summary>
    /// to fill settings.gameSettings by information from file
    /// </summary>
    /// <param name="str">path to file</param>
    /// <returns></returns>
    private settings.GameSettings Parse(string str)
    {
        settings.GameSettings res = new settings.GameSettings
        {
            RequestedResources = new Dictionary<int, int>(),
            SetPlanets = new Dictionary<int, getItems.PlanetProperty>(),
            Storage = new Dictionary<int, List<settingsResearches.AcceptRes>>()
        };

        // Create the XmlDocument.
        xDoc.LoadXml(str);

        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == "NameNative") { res.NameNative = xnode.InnerText; }
            else if (xnode.Name == "NEarthMaterial") { res.NEarthMaterial = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "NMoonMaterial") { res.NMoonMaterial = System.Convert.ToInt32(xnode.InnerText); }

            else if (xnode.Name == "RequestedResources")
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    XmlNode attrNum = childnode.Attributes.GetNamedItem("number");
                    XmlNode attrAmount = childnode.Attributes.GetNamedItem("amount");
                    res.RequestedResources.Add(System.Convert.ToInt32(attrNum.Value), System.Convert.ToInt32(attrAmount.Value));
                }
            }

            else if (xnode.Name == "SetPlanets")
            {
                foreach (XmlElement xPlanet in xnode.ChildNodes)
                {
                    getItems.PlanetProperty PP = new getItems.PlanetProperty();

                    int Key = System.Convert.ToInt32(xPlanet.Attributes.GetNamedItem("number").Value);

                    PP.textName = xPlanet.Attributes.GetNamedItem("textName").Value;
                    PP.textTI = System.Convert.ToInt32(xPlanet.Attributes.GetNamedItem("textTI").Value);
                    PP.numMaterial = System.Convert.ToInt32(xPlanet.Attributes.GetNamedItem("numMaterial").Value);
                    PP.numIntro = System.Convert.ToInt32(xPlanet.Attributes.GetNamedItem("numIntro").Value);
                    PP.flagIsResearched = System.Convert.ToBoolean(xPlanet.Attributes.GetNamedItem("flagIsResearched").Value);
                    PP.flagIsSelected = System.Convert.ToBoolean(xPlanet.Attributes.GetNamedItem("flagIsSelected").Value);
                    PP.flagCoins = System.Convert.ToBoolean(xPlanet.Attributes.GetNamedItem("flagCoins").Value);
                    PP.flagEther = System.Convert.ToBoolean(xPlanet.Attributes.GetNamedItem("flagEther").Value);
                    PP.amountProbes = System.Convert.ToInt32(xPlanet.Attributes.GetNamedItem("amountProbes").Value);

                    int i = 0;
                    foreach (XmlElement xResources in xPlanet.ChildNodes)
                    {
                        if (xResources.Name == "ResNessAmount")
                        {
                            foreach (XmlNode item in xResources.ChildNodes)
                            { PP.ResNess_Amount[i++] = System.Convert.ToInt32(item.InnerText); }
                            i = 0;
                        }
                        else if (xResources.Name == "ResAdd")
                        {
                            foreach (XmlNode item in xResources.ChildNodes)
                            {
                                PP.ResAdd[i++] = System.Convert.ToInt32(item.InnerText);
                            }
                            i = 0;
                        }
                        else if (xResources.Name == "ResAddAmount")
                        {
                            foreach (XmlNode item in xResources.ChildNodes)
                            { PP.ResAddAmount[i++] = System.Convert.ToInt32(item.InnerText); }
                            i = 0;
                        }
                    }
                    res.SetPlanets[Key] = PP;
                }
            }

            else if (xnode.Name == "NDays") { res.NDays = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "NCoins") { res.NCoins = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "stepCoins") { res.stepCoins = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "flagSelectedPlanet") { res.flagSelectedPlanet = System.Convert.ToBoolean(xnode.InnerText); }
            else if (xnode.Name == "NameNew") { res.NameNew = System.Convert.ToString(xnode.InnerText); }

            else if (xnode.Name == "ProbeFactory")
            {
                res.ProbeFactory = new BuildingsOperations.BuildingTime();
                XmlNode attr;
                attr = xnode.Attributes.GetNamedItem("Cost");
                res.ProbeFactory.Cost = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("N");
                res.ProbeFactory.N = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("Time");
                res.ProbeFactory.Time = System.Convert.ToInt32(attr.Value);
            }

            else if (xnode.Name == "Hospital")
            {
                res.Hospital = new BuildingsOperations.BuildingHospital();
                XmlNode attr;
                attr = xnode.Attributes.GetNamedItem("Cost");
                res.Hospital.Cost = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("N");
                res.Hospital.N = System.Convert.ToInt32(attr.Value);
            }

            else if (xnode.Name == "Mine")
            {
                res.Mine = new BuildingsOperations.BuildingMine();
                XmlNode attr;
                attr = xnode.Attributes.GetNamedItem("Cost");
                res.Mine.Cost = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("N");
                res.Mine.N = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("stepCoins");
                res.Mine.stepCoins = System.Convert.ToInt32(attr.Value);
            }

            else if (xnode.Name == "SCfactory")
            {
                res.SCfactory = new BuildingsOperations.BuildingTime();
                XmlNode attr;
                attr = xnode.Attributes.GetNamedItem("Cost");
                res.SCfactory.Cost = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("N");
                res.SCfactory.N = System.Convert.ToInt32(attr.Value);
                attr = xnode.Attributes.GetNamedItem("Time");
                res.SCfactory.Time = System.Convert.ToInt32(attr.Value);
            }

            else if (xnode.Name == "koefToday") { res.koefToday = float.Parse(xnode.InnerText); }
            else if (xnode.Name == "NEther") { res.NEther = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "NProbe") { res.NProbe = System.Convert.ToInt32(xnode.InnerText); }

            else if (xnode.Name == "Storage")
            {
                foreach (XmlElement resource in xnode.ChildNodes)
                {
                    List<settingsResearches.AcceptRes> Resources = new List<settingsResearches.AcceptRes>();
                    // number of resource
                    int key = System.Convert.ToInt32(resource.Attributes.GetNamedItem("number").Value);
                    // List: name of the planet and amount of the resource 
                    foreach (XmlNode planet in resource.ChildNodes)
                    {
                        settingsResearches.AcceptRes acceptRes = new settingsResearches.AcceptRes
                        {
                            NamePlanet = planet.Attributes.GetNamedItem("NamePlanet").Value,
                            amount = System.Convert.ToInt32(planet.Attributes.GetNamedItem("amount").Value)
                        };
                        //print(acceptRes.NamePlanet + " " + acceptRes.amount);
                        Resources.Add(acceptRes);
                    }
                    res.Storage.Add(key, Resources);
                }
            }

            else if (xnode.Name == "flagPeopleTransport") { res.flagPeopleTransport = System.Convert.ToBoolean(xnode.InnerText); }
            else if (xnode.Name == "flagPeopleVeBeenSent") { res.flagPeopleVeBeenSent = System.Convert.ToBoolean(xnode.InnerText); }
            else if (xnode.Name == "NSpasecraft") { res.NSpasecraft = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "NPeopleOnNative") { res.NPeopleOnNative = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "AllPeople") { res.AllPeople = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "NPeopleOnNew") { res.NPeopleOnNew = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "NPeopleDied") { res.NPeopleDied = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "CurrentPerSent") { res.CurrentPerSent = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "CurrentNResUnits") { res.CurrentNResUnits = System.Convert.ToInt32(xnode.InnerText); }
            else if (xnode.Name == "Level") { res.Level = System.Convert.ToInt32(xnode.InnerText); }
        }

        return res;
    }

    /// <summary>
    /// after scene "Game" beginning
    /// </summary>
    public static void SetAll()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "NameNative") { xelem.InnerText = settings.gameSettings.NameNative; }
            else if (xelem.Name == "NEarthMaterial")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NEarthMaterial); }
            else if (xelem.Name == "NMoonMaterial")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NMoonMaterial); }
            else if (xelem.Name == "Level")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.Level); }
            else if (xelem.Name == "RequestedResources")
            {
                foreach (var item in settings.gameSettings.RequestedResources)
                {
                    XmlElement nameItem = xDoc.CreateElement("item");
                    xelem.AppendChild(nameItem);

                    XmlAttribute attrNum = xDoc.CreateAttribute("number");
                    XmlText TextNum = xDoc.CreateTextNode(System.Convert.ToString(item.Key));
                    attrNum.AppendChild(TextNum);
                    nameItem.Attributes.Append(attrNum);

                    XmlAttribute attrAmount = xDoc.CreateAttribute("amount");
                    XmlText TextAmount = xDoc.CreateTextNode(System.Convert.ToString(item.Value));
                    attrAmount.AppendChild(TextAmount);
                    nameItem.Attributes.Append(attrAmount);
                }
            }
            else if (xelem.Name == "SetPlanets")
            {
                foreach (var planet in settings.gameSettings.SetPlanets)
                {
                    XmlElement xPlanet = xDoc.CreateElement("planet");
                    xelem.AppendChild(xPlanet);

                    XmlAttribute attr;
                    attr = xDoc.CreateAttribute("number");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Key)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("textName");
                    attr.AppendChild(xDoc.CreateTextNode(planet.Value.textName));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("textTI");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.textTI)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("numMaterial");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.numMaterial)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("numIntro");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.numIntro)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("flagIsResearched");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.flagIsResearched)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("flagIsSelected");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.flagIsSelected)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("flagCoins");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.flagCoins)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("flagEther");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.flagEther)));
                    xPlanet.Attributes.Append(attr);

                    attr = xDoc.CreateAttribute("amountProbes");
                    attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.Value.amountProbes)));
                    xPlanet.Attributes.Append(attr);

                    XmlElement nameArray = xDoc.CreateElement("ResNessAmount");
                    xPlanet.AppendChild(nameArray);
                    for (int i = 0; i < 3; i++)
                    {
                        XmlElement nameItem = xDoc.CreateElement("item");
                        nameItem.InnerText = System.Convert.ToString(planet.Value.ResNess_Amount[i]);
                        nameArray.AppendChild(nameItem);
                    }

                    nameArray = xDoc.CreateElement("ResAdd");
                    xPlanet.AppendChild(nameArray);
                    for (int i = 0; i < 3; i++)
                    {
                        XmlElement nameItem = xDoc.CreateElement("item");
                        nameItem.InnerText = System.Convert.ToString(planet.Value.ResAdd[i]);
                        nameArray.AppendChild(nameItem);
                    }

                    nameArray = xDoc.CreateElement("ResAddAmount");
                    xPlanet.AppendChild(nameArray);
                    for (int i = 0; i < 3; i++)
                    {
                        XmlElement nameItem = xDoc.CreateElement("item");
                        nameItem.InnerText = System.Convert.ToString(planet.Value.ResAddAmount[i]);
                        nameArray.AppendChild(nameItem);
                    }
                }
            }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// everyday changeing
    /// </summary>
    public static void SetDailyData()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "NDays")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NDays); }
            else if (xelem.Name == "NCoins")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NCoins); }
            else if (xelem.Name == "NPeopleOnNative")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NPeopleOnNative); }
            else if (xelem.Name == "NPeopleDied")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NPeopleDied); }
            else if (xelem.Name == "NPeopleOnNew")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NPeopleOnNew); }
            else if (xelem.Name == "stepCoins")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.stepCoins); }
            else if (xelem.Name == "koefToday")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.koefToday); }
            else if (xelem.Name == "NProbe")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NProbe); }
            else if (xelem.Name == "NSpasecraft")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NSpasecraft); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save researched status of the planet, probes decrement, reload Storage
    /// </summary>
    /// <param name="namePlanet"></param>
    public static void SetResearched(string namePlanet)
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "SetPlanets")
            {
                foreach (XmlElement xPlanet in xelem.ChildNodes)
                {
                    if (xPlanet.Attributes.GetNamedItem("textName").Value == namePlanet)
                    { xPlanet.Attributes.GetNamedItem("flagIsResearched").Value = "true"; }
                }
            }
            else if (xelem.Name == "Storage")
            {
                ResetStorage(xelem);
            }
            else if (xelem.Name == "NProbe")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NProbe); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save researched status of the planet, probes decrement, reload Storage
    /// </summary>
    /// <param name="namePlanet"></param>
    public static void SetResearchIsFailed(string namePlanet)
    {  
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "SetPlanets")
            {
                int i = 0;
                foreach (XmlElement xPlanet in xelem.ChildNodes)
                {
                    if (xPlanet.Attributes.GetNamedItem("textName").Value == namePlanet)
                    {
                        print("1: " + xPlanet.Attributes.GetNamedItem("amountProbes").Value);
                        print("2: " + settings.gameSettings.SetPlanets[i].amountProbes);

                        xPlanet.Attributes.GetNamedItem("amountProbes").Value = 
                            System.Convert.ToString( settings.gameSettings.SetPlanets[i].amountProbes ); }
                    i++;
                }
            }
            else if (xelem.Name == "NProbe")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NProbe); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save flagSelectedPlanet, NameNew, information in SetPlanets and RequestedResources
    /// </summary>
    public static void SetSelected()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        string namePlanet = System.Convert.ToString(settings.gameSettings.NameNew);
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "flagSelectedPlanet")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.flagSelectedPlanet); }
            else if (xelem.Name == "NameNew")
            { xelem.InnerText = namePlanet; }
            else if (xelem.Name == "SetPlanets")
            {
                // change settings in the set of planet
                foreach (XmlElement xPlanet in xelem.ChildNodes)
                {
                    if (xPlanet.Attributes.GetNamedItem("textName").Value == namePlanet)
                    { xPlanet.Attributes.GetNamedItem("flagIsSelected").Value = "true"; }
                }
            }
            else if (xelem.Name == "RequestedResources") { ResetRequested(xelem); }
            else if (xelem.Name == "Storage") { ResetStorage(xelem); }
            else if (xelem.Name == "CurrentNResUnits") 
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.CurrentNResUnits); }
        }
        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// // Save flag people and NSpasecraft when ResNess is collected
    /// </summary>
    public static void SetfPeopleNSC() 
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "flagPeopleTransport")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.flagPeopleTransport); }
            else if (xelem.Name == "NSpasecraft")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NSpasecraft); }
        }
        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save NSpacecraft and amount of people on native and new planets
    /// </summary>
    public static void SetPeopleTransport()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "NSpasecraft")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NSpasecraft); }
            else if (xelem.Name == "NPeopleOnNative")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NPeopleOnNative); }
            else if (xelem.Name == "NPeopleOnNew")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NPeopleOnNew); }
            else if (xelem.Name == "CurrentPerSent")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.CurrentPerSent); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// the first group of people was sent
    /// </summary>
    public static void SetPeopleVeBeenSent()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "flagPeopleVeBeenSent")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.flagPeopleVeBeenSent); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save all current information about buildings
    /// </summary>
    public static void SetBuildings()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        XmlAttribute attr;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "ProbeFactory")
            {
                attr = xDoc.CreateAttribute("Cost");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.ProbeFactory.Cost)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("N");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.ProbeFactory.N)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("Time");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.ProbeFactory.Time)));
                xelem.Attributes.Append(attr);
            }
            else if (xelem.Name == "Hospital")
            {
                attr = xDoc.CreateAttribute("Cost");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.Hospital.Cost)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("N");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.Hospital.N)));
                xelem.Attributes.Append(attr);
            }
            else if (xelem.Name == "Mine")
            {
                attr = xDoc.CreateAttribute("Cost");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.Mine.Cost)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("N");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.Mine.N)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("stepCoins");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.Mine.stepCoins)));
                xelem.Attributes.Append(attr);
            }
            else if (xelem.Name == "SCfactory")
            {
                attr = xDoc.CreateAttribute("Cost");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.SCfactory.Cost)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("N");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.SCfactory.N)));
                xelem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("Time");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(settings.gameSettings.SCfactory.Time)));
                xelem.Attributes.Append(attr);
            }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save researched planets of the SetPlanets
    /// </summary>
    public static void SetTransport()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "SetPlanets")
            {
                foreach (XmlElement xPlanet in xelem.ChildNodes)
                { if (xPlanet.Attributes.GetNamedItem("flagIsResearched").Value == "true") { ResetPlanet(xPlanet); } }
            }
            else if (xelem.Name == "RequestedResources") { ResetRequested(xelem); }
            else if (xelem.Name == "Storage") { ResetStorage(xelem); }
            else if (xelem.Name == "NSpasecraft")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NSpasecraft); }
            else if (xelem.Name == "CurrentNResUnits")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.CurrentNResUnits); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save NEther, Storage
    /// </summary>
    /// <param name="LoadStorage">flag - to reload storage or not</param>
    public static void SetEther(bool LoadStorage)
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "NEther")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NEther); }
            else if (xelem.Name == "Storage" && LoadStorage) { ResetStorage(xelem); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// Save the selected planet's resources and coins
    /// </summary>
    public static void SetSell()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "SetPlanets")
            {
                foreach (XmlElement xPlanet in xelem.ChildNodes)
                { 
                    if (xPlanet.Attributes.GetNamedItem("flagIsSelected").Value == "true") 
                    { ResetPlanet(xPlanet); } 
                }
            }
            else if (xelem.Name == "Storage") { ResetStorage(xelem); }
            else if (xelem.Name == "NCoins")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.NCoins); }
            else if (xelem.Name == "CurrentNResUnits")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.CurrentNResUnits); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// save an information about current selected planet's resources
    /// </summary>
    public static void SetBuy()
    {
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlElement xelem in xRoot)
        {
            if (xelem.Name == "SetPlanets")
            {
                foreach (XmlElement xPlanet in xelem.ChildNodes)
                { 
                    if (xPlanet.Attributes.GetNamedItem("flagIsSelected").Value == "true") 
                    { ResetPlanet(xPlanet); }
                }
            }
            else if (xelem.Name == "RequestedResources") { ResetRequested(xelem); }
            else if (xelem.Name == "Storage") { ResetStorage(xelem); }
            else if (xelem.Name == "CurrentNResUnits")
            { xelem.InnerText = System.Convert.ToString(settings.gameSettings.CurrentNResUnits); }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        fullPath = Application.persistentDataPath + "/ContinueGame.xml";
        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// copy an information from storage to XmlElement
    /// </summary>
    /// <param name="xelem">XmlElement to be changed</param>
    private static void ResetStorage(XmlElement xelem)
    {
        // remove all resources
        while (xelem.HasChildNodes) { xelem.RemoveChild(xelem.FirstChild); }

        XmlAttribute attr;
        // create all new sections for resources
        foreach (var resource in settings.gameSettings.Storage)
        {
            XmlElement resourceItem = xDoc.CreateElement("resource");
            xelem.AppendChild(resourceItem);

            attr = xDoc.CreateAttribute("number");
            attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(resource.Key)));
            resourceItem.Attributes.Append(attr);

            foreach (var planet in resource.Value)
            {
                XmlElement planetItem = xDoc.CreateElement("planet");
                resourceItem.AppendChild(planetItem);

                attr = xDoc.CreateAttribute("NamePlanet");
                attr.AppendChild(xDoc.CreateTextNode(planet.NamePlanet));
                planetItem.Attributes.Append(attr);

                attr = xDoc.CreateAttribute("amount");
                attr.AppendChild(xDoc.CreateTextNode(System.Convert.ToString(planet.amount)));
                planetItem.Attributes.Append(attr);
            }
        }
    }

    /// <summary>
    /// set information about xPlanet's resources
    /// </summary>
    /// <param name="xPlanet"></param>
    private static void ResetPlanet(XmlElement xPlanet)
    {
        foreach (XmlElement element in xPlanet)
        {
            int Key = System.Convert.ToInt32(xPlanet.Attributes.GetNamedItem("number").Value);

            if (element.Name == "ResNessAmount")
            {
                int i = 0;
                foreach (XmlElement resource in element)
                { resource.InnerText = System.Convert.ToString( settings.gameSettings.SetPlanets[Key].ResNess_Amount[i++] ); }
            }
            else if (element.Name == "ResAdd")
            {
                int i = 0;  
                foreach (XmlElement resNum in element)
                { resNum.InnerText = System.Convert.ToString(settings.gameSettings.SetPlanets[Key].ResAdd[i++]); }
            }
            else if (element.Name == "ResAddAmount")
            {
                int i = 0;
                foreach (XmlElement resource in element)
                { resource.InnerText = System.Convert.ToString(settings.gameSettings.SetPlanets[Key].ResAddAmount[i++]); }
            }
        }
    }

    /// <summary>
    /// set information about requested resources
    /// </summary>
    /// <param name="xelem"></param>
    private static void ResetRequested(XmlElement xelem)
    {
        foreach (XmlNode reqRes in xelem.ChildNodes)
        {
            // number of the resource
            int number = System.Convert.ToInt32(reqRes.Attributes.GetNamedItem("number").Value);
            // reset amount of the resource
            reqRes.Attributes.GetNamedItem("amount").Value =
                System.Convert.ToString(settings.gameSettings.RequestedResources[number]);
        }
    } 
}
