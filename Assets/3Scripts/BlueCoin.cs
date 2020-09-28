using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text
using System.Xml; // XML
using System.IO;

public class BlueCoin : MonoBehaviour
{
    public static GameObject sTextBC;
    public static int sNBlueCoin = 2; 

    void Start()
    {
        sTextBC = gameObject;
        gameObject.GetComponent<Text>().text = System.Convert.ToString(sNBlueCoin);
    }

    /// <summary>
    /// save amount of BlueCoins
    /// </summary>
    public void SetBlueCoins()
    {
        // create or read file with personal settings
        string fullPath = PersonalSettings.PathPersonalSettings;
        string str = File.ReadAllText(fullPath);
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(str);
        XmlElement xRoot = xDoc.DocumentElement;
        foreach (XmlNode xnode in xRoot)
        {
            if (xnode.Name == "NBlue")
            {
                xnode.InnerText = System.Convert.ToString(sNBlueCoin);
            }
        }

        StringWriter sw = new StringWriter();
        XmlTextWriter xw = new XmlTextWriter(sw);
        xDoc.WriteTo(xw);

        File.WriteAllText(fullPath, sw.ToString());
    }

    /// <summary>
    /// clever blueCoins increment and saveing to the file
    /// </summary>
    /// <param name="N">amount of additional blueCoins</param>
    /// <returns></returns>
    public void AddBlueCoins(int N)
    {
        if (sNBlueCoin + N > DateChangeing.MaxCoins) { sNBlueCoin = DateChangeing.MaxCoins; }
        else { sNBlueCoin += N; }

        SetBlueCoins();
    }

    /// <summary>
    /// clever blueCoins decrement and saveing to the file
    /// </summary>
    /// <param name="N">amount of substracted blueCoins</param>
    /// <returns></returns>
    public void SubstractBlueCoins(int N)
    {
        if (sNBlueCoin - N < 0) { sNBlueCoin = 0; }
        else { sNBlueCoin -= N; }

        SetBlueCoins();
    }
}
