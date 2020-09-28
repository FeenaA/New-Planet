using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // text

public class advert : MonoBehaviour
{
    public GameObject TextMessageBox;
    public Button ButtonYes; 
    public Button ButtonNo;

    // Start is called before the first frame update
    void Start()
    {
        CorrectLanguages();
    }

    /// <summary>
    ///  to show an advertisement
    /// </summary>
    public void PressedOK()
    {
        Destroy(gameObject);

        // advertisement

        BlueCoin BC = BlueCoin.sTextBC.GetComponent<BlueCoin>();
        BC.AddBlueCoins(1);
        BlueCoin.sTextBC.GetComponent<Text>().text = System.Convert.ToString(BlueCoin.sNBlueCoin);
        buttons.sBlueCoin.interactable = true;
    }

    /// <summary>
    /// to destroy current object
    /// </summary>
    public void PressedNO()
    {
        Destroy(gameObject);
        buttons.sBlueCoin.interactable = true;
    }

    /// <summary>
    /// Set a text in a correct language
    /// </summary>
    private void CorrectLanguages()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            TextMessageBox.GetComponent<Text>().text = "Would you like to watch a short video to get a blue token?";
            ButtonYes.GetComponentInChildren<Text>().text = "Yes";
            ButtonNo.GetComponentInChildren<Text>().text = "No";
        }
        else
        {
            if ((PersonalSettings.language == LanguageSettings.Language.Russian))
            {
                TextMessageBox.GetComponent<Text>().text = "ВЫ ХОТИТЕ ПОСМОТРЕТЬ КОРОТКОЕ ВИДЕО И ПОЛУЧИТЬ СИНИЙ ЖЕТОН?";
                ButtonYes.GetComponentInChildren<Text>().text = "ДА";
                ButtonNo.GetComponentInChildren<Text>().text = "НЕТ";
            }
        }
    }
}
