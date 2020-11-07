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
        settings.gameSettings.NameNative = inputField.text;

        // go to the scene "Game"
        SceneManager.LoadScene("Game");
    }
}
