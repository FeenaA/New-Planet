using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;


public class EarthOnClick : MonoBehaviour
{
    // CanvasBuildings to be shown on the scene
    public GameObject CanvasBuildings;
    // panels to be unactive when CanvasBuildings is on the scene
    public GameObject PanelPeople;
    public GameObject PanelResources;

    // Earth is clicked
    public static bool flagBuildings = false;
    // Earth is clicked the first time
    private static bool flagFirstTime = false;
    void OnMouseDown()
    {
        // to prevent multi showing of the BuildingPanel
        if (!flagBuildings) 
        {
            // show prefabBuildings 
            flagBuildings = true;
            crawlLine.BlockCrawlLine = true;
            CanvasBuildings.SetActive(true);
            PanelPeople.SetActive(false);
            PanelResources.SetActive(false);

            if (!flagFirstTime)
            {
                flagFirstTime = true;
                BuildingsOperations BO = CanvasBuildings.GetComponent<BuildingsOperations>();
                BO.ShowHelp();
            }
        }
    }
}
