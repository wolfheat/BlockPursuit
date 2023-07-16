using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI infoText;
    private Queue<string> textList = new Queue<string>();

    private void Start()
    {
        UpdateVisability();
    }

    public void DisplayText(string text)
    {
        if(textList.Count > 5) { textList.Dequeue(); }
        textList.Enqueue("*"+text+"\r\n");
        DisplayQueue();
    }

    public void UpdateVisability()
    {
        infoPanel.SetActive(toggle.isOn);
    }

    private void DisplayQueue()
    {
        StringBuilder sb = new StringBuilder();
        foreach(string s in textList)
        {
            sb.Append(s);
        }
        infoText.text = sb.ToString();
    }
}
