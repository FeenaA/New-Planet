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
    public Text TextTitle;

    // flag to mean that crawlline is bisy now
    private static bool flagCrawlBusy = false;
    // flag to prevent new messages
    public static bool BlockCrawlLine = false;
     
    // timer to show smth if there wasn't any actions for a long time
    public static int TimerCrawlLine = 0;
    private int nSecondsToWait = 10;

    // crawl line content
    public static Dictionary<int, string> sCrawlContent;

    private int amountOfMessages;
    //private static int currentString = 0;

    private static System.Random random = new System.Random();
    private static string MessageToShow = "";

   private void Start()
    {
        string strTitle = "Spot news";
        if ((PersonalSettings.language == LanguageSettings.Language.Russian)) { strTitle = "НОВОСТИ"; }
        TextTitle.text = strTitle;

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

    /// <summary>
    /// to manage all processes with CrawlLine
    /// </summary>
    /// <returns></returns>
    private void ManagerCrawlLine()
    {
        print(flagCrawlBusy + " " + TimerCrawlLine);

        if (!(BlockCrawlLine || flagCrawlBusy || DateChangeing.pause))
        {
            // if it is time to show message
            if (TimerCrawlLine == nSecondsToWait)
            {
                if (System.String.IsNullOrEmpty(MessageToShow) )
                {
                    // Show the string of an entertaining content
                    int index = random.Next(1, amountOfMessages);
                    MessageToShow = sCrawlContent[index];
                }

                Show(MessageToShow);

                MessageToShow = "";
                TimerCrawlLine = 0;
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
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Normal;
        yield return new WaitForSeconds(timePulse);
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        settings.sTitleCrawlLine.SetActive(false);
        settings.sTitleCrawlLine.GetComponent<Text>().fontStyle = FontStyle.Normal;

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
        settings.sTitleCrawlLine.SetActive(true);

        moveingGameObject.gameObject.SetActive(false);
        moveingGameObject.position = initPos;

        flagCrawlBusy = false;
        // restart TimerCrawlLine
        TimerCrawlLine = 0;
    }
}
