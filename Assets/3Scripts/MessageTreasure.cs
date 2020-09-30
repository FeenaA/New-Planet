using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageTreasure : MonoBehaviour 
{
    public Button ButtonTitle;

    private string TextCoins = "You've found some Coins on this planet!\nUse Coins to build new biuldings.";
    private string TextEther = "You've found 1 Ether on this planet!\nUse Ether to get any resource.";
    private string TextTitle = "Congratulations!"; 
     
    /// <summary>
    /// Use "The start" instead of "Start" to call prefab with parameter
    /// </summary>
    /// <param name="flag"></param>
    void TheStart(bool flag)
    {
        CorrectText();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        var textMessageBox = gameObject.transform.Find("TextMessageBox");
        if (flag)   {textMessageBox.GetComponent<Text>().text = TextCoins;}
        else        {textMessageBox.GetComponent<Text>().text = TextEther;}
        ButtonTitle.GetComponentInChildren<Text>().text = TextTitle; 
    }

    /// <summary>
    /// Correct the title and the text accordingly to the language
    /// </summary>
    private void CorrectText()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            TextCoins = "You've found some Coins on this planet!\nUse Coins to build new biuldings.";
            TextEther = "You've found 1 Ether on this planet!\nUse Ether to get any resource.";
            TextTitle = "Congratulations!";
        }
        else if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            TextCoins = "НА ЭТОЙ ПЛАНЕТЕ ОБНАРУЖЕНО СОКРОВИЩЕ!\nИСПОЛЬЗУЙ МОНЕТЫ ДЛЯ ПОСТРОЙКИ НОВЫХ ЗДАНИЙ.";
            TextEther = "НА ЭТОЙ ПЛАНЕТЕ ОБНАРУЖЕНО СОКРОВИЩЕ!\nИСПОЛЬЗУЙ ЭФИР ВМЕСТО НЕДОСТАЮЩЕГО РЕСУРСА.";
            TextTitle = "ПОЗДРАВЛЯЕМ!";
        }
    }

    // close the MessageBox 
    public void ClosePressed()
    {
        Object.Destroy(gameObject);
    }

}
