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

    // texts
    private string StrTitleWin = "Mission completed";
    private string StrTitleFail = "Game over";
    private string StrMessageWin = "You've rescued a lot of people from the native planet!";
    private string StrMessageFail = "You've lost all people of the native planet. " +
                  "Try another strategy to rich success.";
    private string StrMM = "Main menu";
    private string StrPriseWin = "";
    private string StrPriseFail = "";



    void Start()
    {
        CorrectLanguage();

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

    /// <summary>
    /// count and get prise
    /// </summary>
    /// <returns></returns>
    private int GetPrise()
    {
        int res = 1;

        if (settings.flagIsWin)
        {
            
        }

        print(settings.gameSettings.CurrentNResUnits);
        print(settings.gameSettings.CurrentPerSent);
        print("Was: " + BlueCoin.sNBlueCoin);

        BlueCoin BC = BlueCoin.sTextBC.GetComponent<BlueCoin>();
        BC.AddBlueCoins(1);

        print("Become: " + BlueCoin.sNBlueCoin);

        return res;
    }

    private void CorrectLanguage()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            StrTitleWin = "Mission completed";
            StrMessageWin = "You've rescued " + settings.gameSettings.CurrentPerSent +
                    "% of your population and  collected " + settings.gameSettings.CurrentNResUnits +
                    " units of resources.";
            StrPriseWin = "Prise: " + GetPrise() + " blue tokens";
            StrTitleFail = "Game over";
            StrMessageFail = "You've lost all people of the native planet. " +
                    "Try another strategy to rich success.";
            StrPriseFail = "Сonsolation prize: " + GetPrise() + " blue token";
            StrMM = "Main menu";
        }
        else if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            StrTitleWin = "МИССИЯ ВЫПОЛНЕНА";
            StrMessageWin = "ТЫ СПАС " + settings.gameSettings.CurrentPerSent + 
                    "% СВОЕГО НАРОДА И СОБРАЛ " + settings.gameSettings.CurrentNResUnits + 
                    " ЕДИНИЦ РЕСУРСОВ!";
            StrPriseWin = "НАГРАДА: " + GetPrise() + " СИНИХ ЖЕТОНОВ";
            StrTitleFail = "ПОРАЖЕНИЕ";
            StrMessageFail = "ВИРУС ПОГЛОТИЛ ВЕСЬ ТВОЙ НАРОД. ПОПРОБУЙ ДРУГУЮ СТРАТЕГИЮ.";
            StrPriseFail = "УТЕШИТЕЛЬНЫЙ ПРИЗ: 1 СИНИЙ ЖЕТОН";
            StrMM = "ГЛАВНОЕ МЕНЮ";
        }
    }

    public void ClosePressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Begin");
    }
}
