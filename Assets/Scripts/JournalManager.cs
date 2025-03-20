using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class JournalManager : MonoBehaviour
{
    [SerializeField] private GameObject journalEntryPrefab;
    [SerializeField] private Transform journalContainer;
    private List<JournalEntry> journalEntries = new List<JournalEntry>();

    void Start()
    {
        if (LocalizationSettings.SelectedLocale.name == "French (fr)")
        {
            AddPreWrittenEntry("Tutoriel", "Bienvenue dans le journal ! Cliquez sur un article pour le développer ou le réduire. " +
            "Dans ce jeu, vous aurez pour objectif de rassembler des ingrédients et de réaliser une recette. " +
            "Cliquez sur les options pour voir les commandes du jeu. Appuyer sur E pour interagir avec les personnages peut vous donner des quêtes ou des informations. " +
            "Terminez ce que vous pouvez avant 19h30, heure militaire. Je vous souhaite bonne chance.");
        }
        else if (LocalizationSettings.SelectedLocale.name == "Hebrew (he)")
        {
            AddPreWrittenEntry("שֶׁל מוֹרֶה", "ברוכים הבאים ליומן! לחץ על ערך כדי להרחיב או לכווץ אותו. " +
            "במשחק הזה, תוטל עליך המטרה לאסוף מרכיבים ולהכין מתכון. " +
            "לחץ על אפשרויות כדי לראות את הפקדים של המשחק. לחיצה על E כדי ליצור אינטראקציה עם דמויות עשויה לתת לך משימות או מידע. " +
            "סיים מה שאתה יכול לפני שהשעה תהפוך לשעה 19:30 או 19:30 בערב. אני מאחל לך בהצלחה.");
        }
        else
        {
            AddPreWrittenEntry("Tutorial", "Welcome to the journal! Click an entry to expand or collapse it. " +
            "In this game, you will be tasked with the goal to gather ingredients and make a recipe. " +
            "Click on options to see the controls for the game. Pressing E to interact with NPCs may give you quests or info. " +
            "Finish what you can before it becomes 7:30pm or 19:30pm military time. I wish you the best of luck.");
        }
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
