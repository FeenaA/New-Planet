using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameStart : MonoBehaviour
{
    // the field containing NameNative
    public InputField inputField;

    /// <summary>
    /// Close the MessageBox
    /// </summary>
    public void ClosePressed()
    {
        gameObject.SetActive(false);

        // Reset NameNative
        string str = inputField.text;

        // correct register, case RU
        str = RuDetectAndUpper(str);

        settings.gameSettings.NameNative = str;

        // go to the scene "Game"
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// if line includes russian symbols they'll be changed toUpper
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private string RuDetectAndUpper(string line)
    {
        string[] str = new[] { line }; 

        for (int i = 0; i < str.Length; i++)
        {
            char c = line[i];
            if ((c >= 'а') && (c <= 'я'))
            { str[i] = str[i].ToUpper(new System.Globalization.CultureInfo("ru-RU", false)); }
        }
        return System.String.Join(" ", str); ;
    }

}
