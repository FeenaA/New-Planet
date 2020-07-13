using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class showProgress : MonoBehaviour
{
    public static string Show( Dictionary<int, int> reqRes)
    {
        string s = "";
        foreach (var resource in reqRes)
        {
            if (resource.Key>0)
            {
                // extraordinary resources
                s += getItems.ResourceAdd[resource.Key] + ": " + resource.Value + "/10\n";
            }
            else
            {
                // nessessary resources
                s += getItems.ResNess[resource.Key] + ": " + resource.Value + "/10\n";
            }
        }
        return(s);
    }
}
