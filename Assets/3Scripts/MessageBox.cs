using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    /// <summary>
    /// text to be filled by a meessage
    /// </summary>
    public Text TextMessage;
    /// <summary>
    /// GO to have an access to crawl line
    /// </summary> 
    public GameObject GO_CL;
    /// <summary>
    /// to check if buildings are switched on
    /// </summary>
    public GameObject CanvasBuilding;

    /// <summary>
    /// to fill TextMessage by the message
    /// </summary>
    /// <param name="Message">message is assumed to be in a correct language</param>
    void TheStart(string Message)
    {
        //to block showing any new messages while the messagebox exists
        crawlLine.BlockCrawlLine = true;
        EarthOnClick.flagBuildings = true;
        TextMessage.text = Message;
    }

    /// <summary>
    /// Close the MessageBox
    /// </summary>
    public void ClosePressed()
    {
        // to operate with CrawlLine
        crawlLine.BlockCrawlLine = false;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game")
        { EarthOnClick.flagBuildings = EarthOnClick.sCanvasBuildings.activeSelf; }

        if (!crawlLine.flagShowNow) 
        {
            // restart TimerCrawlLine
            crawlLine.RestartTimer();
        }
        else // there is a message to be shown right now
        {
            crawlLine.TimerCrawlLine = crawlLine.nSecondsToWait;
            crawlLine.flagShowNow = false;
        }

        Destroy(gameObject);
    }
}
