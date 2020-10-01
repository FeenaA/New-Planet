using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// class to report user about possibility to transport people
/// </summary>
public class StartTransportPeople : MonoBehaviour
{
    public Button ButtonTitle;
    public Text TextMessageBox;
    public Text TextPrise;

    private string strResearch = "You've filled the selected planet with vital resources!";
    private string strGame = "Use 1 spacecraft to transport 1000 people.";
    private string strMessage = "Now you can start to transport people to the new planet.";
    private string strTitle = "Congratulations!";
    private string strPrise = "Prise: 1"; 
     
    void Start()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        CorrectText();

        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Research") 
        { 
            strMessage = strResearch;
            TextPrise.GetComponent<Text>().text = strPrise;
        }
        else if (SceneName == "Game") 
        { 
            strMessage = strGame;
            Destroy(TextPrise.transform.GetChild(0).gameObject);
            Object.Destroy(TextPrise); 
        } 
        TextMessageBox.GetComponent<Text>().text = strMessage;
        ButtonTitle.GetComponentInChildren<Text>().text = strTitle;

        print(settings.gameSettings.NSpasecraft);
    }

    /// <summary>
    /// Correct the title and the text accordingly to the language
    /// </summary>
    private void CorrectText()
    {
        string strNewName = settings.gameSettings.NameNew;
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            strResearch = "You've filled the planet " + strNewName + " with vital resources!";
            strGame = "Use 1 spacecraft to transport 1000 people.";
            strMessage = "Now you can start to transport people to the planet " + strNewName + ".";
            strTitle = "Congratulations!";
            strPrise = "Prise: 1";
        }
        else if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            strResearch = "ТЫ ЗАПОЛНИЛ ПЛАНЕТУ " + strNewName + " ЖИЗНЕННОВАЖНЫМИ РЕСУРСАМИ!";
            strGame = "ИСПОЛЬЗУЙ 1 КОСМОЛЁТ, ЧТОБЫ ПЕРЕВЕЗТИ 1000 ЖИТЕЛЕЙ.";
            strMessage = "ТЕПЕРЬ ТЫ МОЖЕШЬ НАЧАТЬ ПЕРЕВОЗИТЬ ЖИТЕЛЕЙ НА ПЛАНЕТУ " + strNewName + ".";
            strTitle = "ПОЗДРАВЛЯЕМ!";
            strPrise = "НАГРАДА: 1";
        }
    }

    /// <summary>
    /// close the MessageBox 
    /// </summary>
    public void ClosePressed()
    {
        // NSpasecraft increment
        settings.gameSettings.NSpasecraft++;

        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Research") 
        {
            // Save flag people and NSpasecraft
            LoadGame.SetfPeopleNSC();
            SceneManager.LoadScene("Game"); 
        }
        else if (SceneName == "Game") 
        { 
            Object.Destroy(gameObject); 
        }
    }


}
