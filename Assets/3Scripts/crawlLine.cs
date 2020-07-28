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

    public void Show(string message)
    {
        //GameObject instance = Instantiate(settingsResearches.sPrefabCrawnLine);
        //instance.transform.SetParent(settingsResearches.sPanelCrawlLine.transform);
        //instance.transform.position = settingsResearches.sAnchor.transform.position;


        StartCoroutine(CrawlLine(message));


        //imageCrawnLine.transform.position = new Vector3(imageCrawnLine.rect.width/2,0,0);
        //print(imageCrawnLine.rect.width / 2);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //StartCoroutine(StepCrawlLine());

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

        // crawl line moveing
        moveingGameObject.transform.GetComponent<Text>().text = message;
        float width = moveingGameObject.rect.width;
        float initPosX = moveingGameObject.transform.position.x;
        print(initPosX);
        print(width);
        while ((-moveingGameObject.transform.position.x) < (width + initPosX))
        {
            yield return new WaitForSeconds(movementSpeed);
            moveingGameObject.transform.position = moveingGameObject.transform.position + new Vector3(-stepSize, 0, 0);
            print(-moveingGameObject.transform.position.x);
        }
    }
}
