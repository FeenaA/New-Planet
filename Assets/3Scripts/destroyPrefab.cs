using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyPrefab : MonoBehaviour
{
    void Start()
    {
        Transform canvas = transform.Find("Canvas");
        Transform panel = canvas.Find("Panel");
        // give actual information about N coins per day
        //panel.Find("Text").GetComponent<Text>().text = coinsRegister.stepCoins + " per day";
        panel.Find("Text").GetComponent<Text>().text = DateChangeing.stepCoins + " per day";

        // destroy the "N coins per day" object in 3 seconds
        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
