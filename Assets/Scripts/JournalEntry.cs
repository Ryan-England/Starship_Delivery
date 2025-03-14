using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JournalEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text previewText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject inputArea;

    private JournalManager journalManager;
    private bool isExpanded = false;

    public void Initialize(JournalManager manager, string title = "New Entry", string content = "")
    {
        journalManager = manager;
        inputField.text = content;
        previewText.text = title;
    }

    public void ToggleEntry()
    {
        if (isExpanded)
        {
            CollapseEntry();
        }
        else
        {
            journalManager.CollapseAllExcept(this);
            ExpandEntry();
        }
    }

    public void ExpandEntry()
    {
        isExpanded = true;
        inputArea.SetActive(true);
        previewText.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.ActivateInputField();
    }

    public void CollapseEntry()
    {
        if (!isExpanded) return;

        isExpanded = false;
        inputArea.SetActive(false);
        previewText.text = GetFirstLine(inputField.text);
        previewText.gameObject.SetActive(true);
    }

    string GetFirstLine(string text)
    {
        if (string.IsNullOrEmpty(text)) return "New Entry...";
        string[] lines = text.Split('\n');
        return lines.Length > 0 ? lines[0] : text;
    }
}
