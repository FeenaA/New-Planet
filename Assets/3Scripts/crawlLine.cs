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
    // text title
    public GameObject TextTitile; 

    public static bool flagCrawlBusy = false;

    private void Start()
    { 
        string strTitle = "";
        if (PersonalSettings.language == LanguageSettings.Language.English) { strTitle = "Spot news"; }
        else { if ((PersonalSettings.language == LanguageSettings.Language.Russian)) { strTitle = "НОВОСТИ"; } }
        TextTitile.GetComponent<Text>().text = strTitle;
    }

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
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Normal;
        yield return new WaitForSeconds(timePulse);
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        settings.sTitleCrawlLine.SetActive(false);
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Normal;

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
        settings.sTitleCrawlLine.SetActive(true);

        moveingGameObject.gameObject.SetActive(false);
        moveingGameObject.transform.position = initPos;
    }
}
