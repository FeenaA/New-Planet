﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShowProgress : MonoBehaviour
{
    public string Show(Dictionary<int, int> reqRes)
    {
        string s = "";
        foreach (var resource in reqRes)
        {
            if (resource.Key>0)
            {
                // extraordinary resources
                s += getItems.ResourceAdd[resource.Key].name + ": " + resource.Value + "/10\n";
            }
            else
            {
                // nessessary resources
                s += getItems.ResNess[resource.Key].name + ": " + resource.Value + "/10\n";
            }
        }
        return(s);
    }
}
