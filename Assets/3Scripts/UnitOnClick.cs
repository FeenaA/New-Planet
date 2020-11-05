using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitOnClick : MonoBehaviour
{
    public GameObject TimerUnit;

    /// <summary>
    /// OnMouseDown -> to show timer
    /// </summary>
    public void UnitPressed()
    { 
        if (!TimerUnit.activeSelf)
        {
            // make TimerUnit active
            TimerUnit.SetActive(true);
            // make TimerUnit not active in 3 second
            StartCoroutine(MakeSleepObject());
        }
    }

    IEnumerator MakeSleepObject()
    {
        yield return new WaitForSeconds(4.0f);
        if (TimerUnit.activeSelf)
        {
            TimerUnit.SetActive(false);
        }
    }
}


