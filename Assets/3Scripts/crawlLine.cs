using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crawlLine : MonoBehaviour
{
    // time for pause
    public float movementSpeed = 0.02f;
    // step on x for new position
    public float stepSize = 0.05f;
    // moving gameObject
    public RectTransform moveingGameObject;
    // stable gameObject
    public RectTransform stableGameObject;
    // text massage
    public GameObject TextMessage;

    public static bool flagCrawlBusy = false; 

    public void Show(string message)
    {
        if (!flagCrawlBusy)
        {
            flagCrawlBusy = true;
            StartCoroutine(CrawlLine(message));
            flagCrawlBusy = false;
        }
    }

    IEnumerator CrawlLine(string message)
    {
        // periodical spiking of the title
        float timePulse = 0.5f;
        yield return new WaitForSeconds(timePulse);
        settingsResearches.sTitle.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        settingsResearches.sTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
        yield return new WaitForSeconds(timePulse);
        settingsResearches.sTitle.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        settingsResearches.sTitle.SetActive(false);
        settingsResearches.sTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;

        // crawl line moving
        TextMessage.transform.GetComponent<Text>().text = message;
        moveingGameObject.gameObject.SetActive(true);
        Vector3 initPos = moveingGameObject.transform.position;
        float initPosX = initPos.x;

        while ((-moveingGameObject.transform.position.x) < initPosX)
        {
            yield return new WaitForSeconds(movementSpeed);
            moveingGameObject.transform.position = moveingGameObject.transform.position + new Vector3(-stepSize, 0, 0);
        }
        settingsResearches.sTitle.SetActive(true);

        moveingGameObject.gameObject.SetActive(false);
        moveingGameObject.transform.position = initPos;
    }
}
