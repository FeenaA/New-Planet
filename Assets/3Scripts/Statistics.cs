using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public static int sBestPerSent;
    public static int sBestNResUnits;
    public static int sWins;
    public static int sLoses;

    private int WinPerSent;
    private int LosPerSent;
     
    public Text TextStatistics;
    public Text TextTitle; 
    public Text TextWellDone;

    /// <summary>
    /// Use "The start" instead of "Start" to call prefab with parameter
    /// </summary>
    /// <param name="flagBut0Fin1"></param> // if the mission is completed, true
    void TheStart(bool flagBut0Fin1)
    {
        // to operate with ending of the finished mission
        if ( flagBut0Fin1 )
        {
            // flag: if the best result was improved
            bool flagImprovement = false;

            // change Statistics using information about the finished mission
            if (settings.flagIsWin) { sWins++; }
            else { sLoses++; }
            if (sBestPerSent < settings.gameSettings.CurrentPerSent)
            {
                sBestPerSent = settings.gameSettings.CurrentPerSent;
                flagImprovement = true;
            }
            if (sBestNResUnits < settings.gameSettings.CurrentNResUnits)
            {
                sBestNResUnits = settings.gameSettings.CurrentNResUnits;
                flagImprovement = true;
            }

            // to show TextWellDone
            if (flagImprovement)
            {
                StartCoroutine(WellDoneShow(TextWellDone));
            }
        }

        // count per sent
        if (sWins + sLoses == 0) // to prevent dividing by zero
        {
            WinPerSent = 0;
            LosPerSent = 0;
        }
        else
        {
            WinPerSent = sWins * 100 / (sWins + sLoses);
            LosPerSent = 100 - WinPerSent;
        }

        CorrectText(flagBut0Fin1);
    }

/// <summary>
/// Show "Well Done" string
/// </summary>
/// <param name="TXT">Text to be manipulated</param>
/// <returns></returns>
    private IEnumerator WellDoneShow(Text TXT)
    {
        // periodical spiking of the title
        float timePulse = 0.5f;
        TXT.fontStyle = FontStyle.Normal;
        TXT.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePulse);
        TXT.fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(2*timePulse);
        TXT.fontStyle = FontStyle.Normal;
        yield return new WaitForSeconds(timePulse);
        TXT.fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(2*timePulse);
        TXT.gameObject.SetActive(false);
    }

    private void CorrectText(bool flagBut0Fin1)
    {
        // correct text on the ButtonLanguage
        if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            string str = "НАИЛУЧШИЙ РЕЗУЛЬТАТ:\n\n" + sBestPerSent + "% НАСЕЛЕНИЯ СПАСЕНО ";
            // current data
            if (flagBut0Fin1) { str += "(СЕЙЧАС " + settings.gameSettings.CurrentPerSent + "%)\n"; }
            else { str += "\n"; }

            str += sBestNResUnits + " ЕДИНИЦ РЕСУРСОВ СОБРАНО ";
            // current data
            if (flagBut0Fin1) { str += "(СЕЙЧАС " + settings.gameSettings.CurrentNResUnits + ")\n\n"; }
            else { str += "\n\n"; }

            str += "ПОБЕД: " + sWins + " (" + WinPerSent + "%)\nПОРАЖЕНИЙ: " + sLoses + " (" + LosPerSent + "%)";

            TextStatistics.text = str;
            TextTitle.text = "СТАТИСТИКА";
            TextWellDone.text = "ТЕБЕ УДАЛОСЬ ПОБИТЬ СВОЙ РЕКОРД!";
        }
        else
        {
            string str = "Your best result:\n\n" + sBestPerSent + "% of saved people ";
            // current data
            if (flagBut0Fin1) { str += "(now " + settings.gameSettings.CurrentPerSent + "%)\n"; }
            else { str += "\n"; }

            str += sBestNResUnits + " units of collected resources ";
            // current data
            if (flagBut0Fin1) { str += "(now " + settings.gameSettings.CurrentNResUnits + ")\n\n"; }
            else { str += "\n\n"; }

            str += "Wins: " + sWins + " (" + WinPerSent + "%)\nLoses: " + sLoses + " (" + LosPerSent + "%)";

            TextStatistics.text = str;
            TextTitle.text = "Statistics";
            TextWellDone.text = "You've improved your best result!";
        }
    }

    public void ClosePressed()
    {
        Destroy(gameObject);
    }
}
