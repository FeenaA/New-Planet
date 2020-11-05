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
    public Transform moveingGameObject;
    // text message
    public Text TextMessage;
    // text title
    public GameObject TextTitle;

    // flag to mean that crawlline is bisy now
    private static bool flagCrawlBusy = false;
    // flag to prevent new messages
    public static bool BlockCrawlLine = false;
    // flag to show message without pause
    public static bool flagShowNow = false;

    // timer to show smth if there wasn't any actions for a long time
    public static int TimerCrawlLine = 0;
    public static int nSecondsToWait = 7;

    // crawl line content
    public static Dictionary<int, string> sCrawlContent;

    private int amountOfMessages;
    //private static int currentString = 0;

    private static System.Random random = new System.Random();
    private static string MessageToShow = "";

   private void Start()
    {
        //print("Message: " + MessageToShow);

        string strTitle = "Spot news";
        if ((PersonalSettings.language == LanguageSettings.Language.Russian)) { strTitle = "НОВОСТИ"; }
        TextTitle.GetComponent<Text>().text = strTitle;

        int nSecondsStep = 1;
        TimerCrawlLine = nSecondsToWait;
        amountOfMessages = sCrawlContent.Count;
        InvokeRepeating(nameof(ManagerCrawlLine), 0, nSecondsStep);
    }

    /// <summary>
    /// Restart time and free status of crawl line
    /// </summary>
    public static void RestartTimer()
    {
        flagCrawlBusy = false;
        TimerCrawlLine = 0;
    }

    
    public void RestartToShow()
    {
        flagCrawlBusy = false;
    }

    /// <summary>
    /// to manage all processes with CrawlLine
    /// </summary>
    /// <returns></returns>
    private void ManagerCrawlLine()
    {
        //print(flagCrawlBusy + " " + TimerCrawlLine);

        if (!(BlockCrawlLine || flagCrawlBusy || DateChangeing.pause))
        {
            // if it is time to show message
            if (TimerCrawlLine == nSecondsToWait)
            {
                string message = MessageToShow;
                MessageToShow = "";

                if (System.String.IsNullOrEmpty(message) )
                {
                    // Show the string of an entertaining content
                    int index = random.Next(1, amountOfMessages);
                    message = sCrawlContent[index];
                }
                Show(message);
            }

            // timer increment
            TimerCrawlLine++;
        }
    }

    /// <summary>
    /// ti show the message after a pause
    /// </summary>
    /// <param name="message">message to be shown</param>
    public void ShowNext(string message)
    {
        MessageToShow = message;
    }

    /// <summary>
    /// ti show the message after a pause
    /// </summary>
    /// <param name="message">message to be shown</param>
    public void ShowWithoutPause(string message)
    {
        flagShowNow = true;
        MessageToShow = message;
    }

    private void Show(string message)
    {
        if (!flagCrawlBusy)
        {
            flagCrawlBusy = true;
            StartCoroutine(CrawlLine(message));
        }
    }

    IEnumerator CrawlLine(string message)
    {
        // periodical spiking of the title
        float timePulse = 0.5f;
        yield return new WaitForSeconds(timePulse);
        TextTitle.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        TextTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
        yield return new WaitForSeconds(timePulse);
        TextTitle.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        TextTitle.SetActive(false);
        TextTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;

        // crawl line moving
        TextMessage.text = message;
        moveingGameObject.gameObject.SetActive(true);
        Vector3 initPos = moveingGameObject.position;
        float initPosX = initPos.x;

        while ((-moveingGameObject.position.x) < initPosX)
        {
            yield return new WaitForSeconds(movementSpeed);
            moveingGameObject.position += new Vector3(-stepSize, 0, 0);
        }
        TextTitle.SetActive(true);

        moveingGameObject.gameObject.SetActive(false);
        moveingGameObject.position = initPos;

        // make crawl line free
        flagCrawlBusy = false;

        if (!flagShowNow) 
        { 
            // restart TimerCrawlLine
            TimerCrawlLine = 0; 
        } 
        else // there is a message to be shown right now
        { 
            TimerCrawlLine = nSecondsToWait; 
            flagShowNow = false; 
        }
    }
}
