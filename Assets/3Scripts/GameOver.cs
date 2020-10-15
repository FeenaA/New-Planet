using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Message about finish status (win or fail)
    public Text TextMessage;
    // Prise
    public Text TextPrise;
    // Title 
    public Text TextTitle;
    // Go to the main menu 
    public Text TextMainMenu;
    // prefab cointaining this script
    public GameOver prefabGameOver;

    // texts
    private string StrTitleWin = "Mission completed";
    private string StrTitleFail = "Game over";
    private string StrMessageWin = "You've rescued a lot of people from the native planet!";
    private string StrMessageFail = "You've lost all people of the native planet. " +
                  "Try another strategy to rich success.";
    private string StrMM = "Main menu";
    private string StrPriseWin = "";
    private string StrPriseFail = "";

    /// <summary>
    /// Correct language and show text
    /// </summary>
    void Start()
    {
        // the name of the current scene
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (SceneName != "Begin")
        {
            GetFinished();

            settings.flagIsFinished = true;
            if (settings.flagIsWin)
            {
                TextMessage.transform.GetComponent<Text>().text = StrMessageWin;
                TextTitle.transform.GetComponent<Text>().text = StrTitleWin;
                TextPrise.transform.GetComponent<Text>().text = StrPriseWin;
            }
            else
            {
                TextMessage.transform.GetComponent<Text>().text = StrMessageFail;
                TextTitle.transform.GetComponent<Text>().text = StrTitleFail;
                TextPrise.transform.GetComponent<Text>().text = StrPriseFail;
            }
            TextMainMenu.transform.GetComponent<Text>().text = StrMM;
        }
        else
        {
            if (settings.flagIsWin)
            { TextTitle.transform.GetComponent<Text>().text = StrTitleWin; }
            else { TextTitle.transform.GetComponent<Text>().text = StrTitleFail; }
            Destroy(TextPrise);
            TextMessage.transform.GetComponent<Text>().text = TextStatistics();
            TextMainMenu.transform.GetComponent<Text>().text = "OK";
        }

    }

    /// <summary>
    /// get the information about general amount of winnings and failures
    /// </summary>
    private string TextStatistics()
    {
        string res = "Statistics";
        
        return res;
    }


    /// <summary>
    /// count and get prise
    /// </summary>
    /// <returns></returns>
    private int GetPrise()
    {
        int res = 1;

        // count prise
        if (settings.flagIsWin)
        { res += (settings.gameSettings.CurrentNResUnits + settings.gameSettings.CurrentPerSent) / 25; }

        BlueCoin BC = BlueCoin.sTextBC.GetComponent<BlueCoin>();
        BC.AddBlueCoins(res);

        return res;
    }

    private void GetFinished()
    {
        int Prise = GetPrise();

        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            StrTitleWin = "Mission completed";
            StrMessageWin = "You've rescued " + settings.gameSettings.CurrentPerSent +
                    "% of your population and  collected " + settings.gameSettings.CurrentNResUnits +
                    " units of resources.";
            StrPriseWin = "Prise: " + Prise + " blue tokens";
            StrTitleFail = "Failure";
            StrMessageFail = "You've lost all people of the native planet. " +
                    "Try another strategy to rich success.";
            StrPriseFail = "Сonsolation prize: " + Prise + " blue token";
            StrMM = "Main menu";
        }
        else if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            StrTitleWin = "МИССИЯ ВЫПОЛНЕНА";
            StrMessageWin = "ТЫ СПАС " + settings.gameSettings.CurrentPerSent + 
                    "% СВОЕГО НАРОДА И СОБРАЛ " + settings.gameSettings.CurrentNResUnits + 
                    " ЕДИНИЦ РЕСУРСОВ!";
            StrPriseWin = "НАГРАДА: " + Prise + " СИНИХ ЖЕТОНОВ";
            StrTitleFail = "ПОРАЖЕНИЕ";
            StrMessageFail = "ВИРУС ПОГЛОТИЛ ВЕСЬ ТВОЙ НАРОД. ПОПРОБУЙ ДРУГУЮ СТРАТЕГИЮ.";
            StrPriseFail = "УТЕШИТЕЛЬНЫЙ ПРИЗ: " + Prise + " СИНИЙ ЖЕТОН";
            StrMM = "ГЛАВНОЕ МЕНЮ";
        }
    }

    public void ClosePressed()
    {
        // the name of the current scene
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (SceneName != "Begin")
        { UnityEngine.SceneManagement.SceneManager.LoadScene("Begin"); }
        else { Destroy(gameObject); }
    } 
}
