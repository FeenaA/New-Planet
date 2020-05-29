using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;


public class EarthOnClick : MonoBehaviour
{
    public GameObject prefabBuildings;

    // нажата планета Earth
    public static bool flagBuildings = false;
    //private GameObject instance = null;
    public static bool flagPauseBeforePrefab = false;
    void OnMouseDown()
    {
        // the GameObject instance could be destroyed outside this script

        // if prefab exists on the scene
        //if (instance != null) { flagBuildings = true; }
        //else { flagBuildings = false; }

        // установлена ли пауза до вызова данной функции?
        flagPauseBeforePrefab = DateChangeing.pause;

        if (!flagBuildings) 
        {
            // show prefabBuildings 
            flagBuildings = true;
            GameObject instance = Instantiate(prefabBuildings);

            // make a DateCounter to be paused
            if (!flagPauseBeforePrefab)
            {
                buttons.sPausePressed();
            }
        }
    }
}
