using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public RectTransform movingLine;
    public RectTransform orangeSection;
    public RectTransform redSection;
    public Text winText;
    public GameObject keyIndicator;        // UI element to show the key prompt
    public Text keyIndicatorText;          // Text inside the keyIndicator

    public Transform interactableObject;   // The GameObject the player interacts with
    public KeyCode interactionKey = KeyCode.E; // Key to start the minigame
    public float interactionRange = 2f;    // Distance within which the player can interact

    private int successCount = 0;
    public int requiredSuccesses = 3;
    private bool hasWon = false;

    public static bool isMinigameActive = false; // Minigame starts inactive

    private Transform player; // Reference to the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure your player has the "Player" tag
        redSection.gameObject.SetActive(false); // Hide minigame until started
        keyIndicator.SetActive(false);          // Hide key indicator initially
    }

    void Update()
    {
        // Check for interaction to start the minigame
        // if (!isMinigameActive && Vector3.Distance(player.position, interactableObject.position) <= interactionRange)
        // {
        //     keyIndicator.SetActive(true);
        //     keyIndicatorText.text = "Press E to Start Chopping";

        //     if (Input.GetKeyDown(interactionKey))
        //     {
        //         StartMinigame();
        //     }
        // }
        // else if (!isMinigameActive)
        // {
        //     keyIndicator.SetActive(false); // Hide indicator when out of range
        // }

        // Minigame logic
        if (isMinigameActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (hasWon)
            {
                HideWinMessage();
                isMinigameActive = false;
                redSection.gameObject.SetActive(false); // Hide minigame UI when done
                return;
            }

            if (IsLineInOrangeSection())
            {
                successCount++;
                Debug.Log("Success! The line is in the orange section!");

                if (successCount >= requiredSuccesses)
                {
                    DisplayWinMessage();
                }
                else
                {
                    MoveOrangeSection();
                }
            }
            else
            {
                Debug.Log("Missed! The line is outside the orange section.");
            }
        }
    }

    private bool IsLineInOrangeSection()
    {
        Rect movingRect = new Rect(
            movingLine.anchoredPosition - (movingLine.sizeDelta / 2),
            movingLine.sizeDelta
        );

        Rect orangeRect = new Rect(
            orangeSection.anchoredPosition - (orangeSection.sizeDelta / 2),
            orangeSection.sizeDelta
        );

        return movingRect.Overlaps(orangeRect);
    }

    private void MoveOrangeSection()
    {
        float redWidth = redSection.rect.width;
        float redHeight = redSection.rect.height;

        float orangeWidth = orangeSection.rect.width;
        float orangeHeight = orangeSection.rect.height;

        float maxX = (redWidth - orangeWidth) / 2;
        float maxY = (redHeight - orangeHeight) / 2;

        float newX = Random.Range(-maxX, maxX);
        float newY = Random.Range(-maxY, maxY);

        orangeSection.anchoredPosition = new Vector2(newX, newY);
    }

    private void DisplayWinMessage()
    {
        winText.gameObject.SetActive(true);
        redSection.gameObject.SetActive(false);
        hasWon = true;
        isMinigameActive = true; // Stay active to wait for win message dismissal
    }

    private void HideWinMessage()
    {
        winText.gameObject.SetActive(false);
        hasWon = false;
        isMinigameActive = false; // Fully deactivate minigame after win message
    }

    public void StartMinigame()
    {
        Debug.Log("Minigame Started!");
        redSection.gameObject.SetActive(true);  // Show minigame UI
        keyIndicator.SetActive(false);          // Hide the key indicator when minigame starts
        isMinigameActive = true;
        successCount = 0;                       // Reset progress
        MoveOrangeSection();                   // Start with a random orange section position
    }
}
