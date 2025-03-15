using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalManager : MonoBehaviour
{
    [SerializeField] private GameObject journalEntryPrefab;
    [SerializeField] private Transform journalContainer;
    private List<JournalEntry> journalEntries = new List<JournalEntry>();

    void Start()
    {
        AddPreWrittenEntry("Tutorial", "Welcome to the journal! Click an entry to expand or collapse it.");
    }

    public void CreateNewEntry()
    {
        AddPreWrittenEntry("New Entry", "");
    }

    public void AddPreWrittenEntry(string title, string content)
    {
        GameObject newEntryObj = Instantiate(journalEntryPrefab, journalContainer);
        JournalEntry newEntry = newEntryObj.GetComponent<JournalEntry>();
        journalEntries.Add(newEntry);
        CollapseAllExcept(newEntry);
        newEntry.Initialize(this, title, content);
    }

    public void CollapseAllExcept(JournalEntry entry)
    {
        foreach (var e in journalEntries)
        {
            if (e != entry)
                e.CollapseEntry();
        }
    }
}
