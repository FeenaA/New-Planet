using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;


public class EarthOnClick : MonoBehaviour
{
    public GameObject prefabBuildings;

    // Earth is clicked
    public static bool flagBuildings = false;
    void OnMouseDown()
    {
        // to prevent multi showing of the BuildingPanel
        if (!flagBuildings) 
        {
            // show prefabBuildings 
            flagBuildings = true;
            settings.sCanvasBuildings.SetActive(true);
            settings.sPanelPeople.SetActive(false);
            settings.sPanelResources.SetActive(false);
        }
    }
}
