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
        // flag - was the game paused before OnMouseDown?
        settings.flagPauseBeforePrefab = DateChangeing.pause;

        if (!flagBuildings) 
        {
            // show prefabBuildings 
            flagBuildings = true;
            //GameObject instance = 
                Instantiate(prefabBuildings);

            // make a DateCounter to be paused
            if (!settings.flagPauseBeforePrefab)
            {
                buttons.sPausePressed();
            }
        }
    }
}
