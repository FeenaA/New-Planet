using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public Button[] ButtonsHelp;
    public Text TextHelp;
    public Text TextTitle; 
    private Button ButtonHelp;
    private Color DeSelectedColor;
     
    void Start()
    {
        if (PersonalSettings.language == LanguageSettings.Language.Russian)
        { TextTitle.text = "СПРАВКА"; }
        else
        { TextTitle.text = "Help"; }

        DeSelectedColor = ButtonsHelp[0].GetComponent<Graphic>().color;

        for (int n = 0; n < 4; n++)
        { ButtonsHelp[n].GetComponentInChildren<Text>().text = Preprocessing.strHelpTitle[n]; }

        // the first Button is selected by default
        ButtonHelp = ButtonsHelp[0];
        TextHelp.text = Preprocessing.strHelp[0];
        Select();
    }

    /// <summary>
    /// Select or Deselect all buttons accordingly to their numbers
    /// </summary>
    /// <param name="n">number of the pressed button</param>
    public void ButtonHelpPressed(int n)
    {
        for (int i = 0; i<4; i++)
        {
            ButtonHelp = ButtonsHelp[i];
            TextHelp.text = Preprocessing.strHelp[n];

            if (i == n) { Select(); }
            else        { DeSelect(); }
        }
    }

    /// <summary>
    /// Make the selected button black
    /// </summary>
    private void Select() 
    {
        ButtonHelp.GetComponent<Graphic>().color = Color.black;
        ButtonHelp.GetComponent<Outline>().enabled = true;
    } 

    /// <summary>
    /// Make previous selected button gray
    /// </summary>
    private void DeSelect()
    {
        ButtonHelp.GetComponent<Graphic>().color = DeSelectedColor;
        ButtonHelp.GetComponent<Outline>().enabled = false;
    }

    /// <summary>
    /// Destroy gameObject and Show CanvasMain
    /// </summary>
    public void BackPressed() 
    {
        Destroy(gameObject); 
        Preprocessing.sCanvasMain.SetActive(true);
    }
}
