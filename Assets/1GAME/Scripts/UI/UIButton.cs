////////////////////////////////////////////////
//    Author: https://github.com/dashhoff
////////////////////////////////////////////////
 
using System;
using TMPro;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;

    private string startText;
    private string currentText;

    private void Start()
    {
        startText = _buttonText.text;
    }

    public void OnPointerEnter()
    {
        currentText = _buttonText.text;
        
        if (currentText == "" || currentText == " ") return;
        
        _buttonText.text = "> " + currentText;
    }
    
    public void OnPointerExit()
    {
        _buttonText.text = currentText;
    }
}
