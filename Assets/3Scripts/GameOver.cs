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
    private string StrTitleWin;
    private string StrTitleFail;
    private string StrMessageWin;
    private string StrMessageFail;
    private string StrMessageTry;

    private string StrMM;
    private string StrPriseWin;
    private string StrPriseFail;

    /// <summary>
    /// Correct language and show text
    /// </summary>
    void Start()
    {
        GetFinished();

        settings.flagIsFinished = true;
        if (settings.flagIsWin)
        {
            TextMessage.text = StrMessageWin;
            TextTitle.text = StrTitleWin;
            TextPrise.text = StrPriseWin;
        }
        else
        {
            TextTitle.text = StrTitleFail;
            TextPrise.text = StrPriseFail;

            if (settings.gameSettings.NPeopleOnNew > 0) // you tried
            { TextMessage.text = StrMessageTry; } 
            else { TextMessage.text = StrMessageFail; } // nobody is on new planet
        }
        TextMainMenu.text = StrMM;

        print("PS=" + settings.gameSettings.CurrentPerSent);
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

        if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            StrTitleWin = "МИССИЯ ВЫПОЛНЕНА";
            StrMessageWin = "ТЫ СПАС " + settings.gameSettings.CurrentPerSent + 
                    "% СВОЕГО НАРОДА И СОБРАЛ " + settings.gameSettings.CurrentNResUnits + 
                    " ЕДИНИЦ РЕСУРСОВ!";
            StrPriseWin = "НАГРАДА: " + Prise + " СИНИХ ЖЕТОНОВ";
            StrTitleFail = "ПОРАЖЕНИЕ";
            StrMessageFail = "ВИРУС ПОГЛОТИЛ ВЕСЬ ТВОЙ НАРОД. ПОПРОБУЙ ДРУГУЮ СТРАТЕГИЮ.";
            StrMessageTry = "ТЫ СПАС " + settings.gameSettings.CurrentPerSent + "% ЖИТЕЛЕЙ, НО ИХ НЕДОСТАТОЧНО, ЧТОБЫ ВЫЖИТЬ НА НОВОЙ ПЛАНЕТЕ";
            StrPriseFail = "УТЕШИТЕЛЬНЫЙ ПРИЗ: " + Prise + " СИНИЙ ЖЕТОН";
            StrMM = "ГЛАВНОЕ МЕНЮ";
        }
        else
        {
            StrTitleWin = "Mission completed";
            StrMessageWin = "You've rescued " + settings.gameSettings.CurrentPerSent +
                    "% of your population and  collected " + settings.gameSettings.CurrentNResUnits +
                    " units of resources.";
            StrPriseWin = "Prise: " + Prise + " blue tokens";
            StrTitleFail = "Failure";
            StrMessageFail = "You've lost all people of the native planet. " +
                    "Try another strategy to rich success.";
            StrMessageTry = "YOU SAVED " + settings.gameSettings.CurrentPerSent + "% OF PEOPLE, BUT THEY ARE NOT ENOUGH TO SURVIVE ON A NEW PLANET.";
            StrPriseFail = "Сonsolation prize: " + Prise + " blue token";
            StrMM = "Main menu";
        }
    }

    public void ClosePressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Begin");
    } 
}
