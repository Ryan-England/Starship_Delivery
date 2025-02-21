using UnityEngine;
using UnityEngine.UI;

public class MixingMinigame : MonoBehaviour
{
    public Slider gaugeBar;               // The gauge bar UI element
    public float fillAmountPerPress = 0.1f;
    public float decayRate = 0.02f;
    public GameObject keyIndicator;       // Key Indicator GameObject to activate/deactivate
    public Text keyIndicatorText;         // UI text to show which keys to press
    public Text successMessage;           // Message displayed when gauge is full

    //public Transform interactableObject;  // Object to trigger the minigame
    public KeyCode interactionKey = KeyCode.E; // Key to start the minigame
    public float interactionRange = 2f;   // Range to interact with the object

    private bool hasWon = false;                 // Tracks if the minigame was completed
    private KeyCode lastKeyPressed;

    private Transform player; // Reference to the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Ensure player has "Player" tag
        gaugeBar.value = 0;
        keyIndicator.SetActive(false);     // Ensure the key indicator is initially hidden
        successMessage.gameObject.SetActive(false);
        gaugeBar.gameObject.SetActive(false); // Hide the minigame UI initially
    }

    void Update()
    {
        // Interaction to start the minigame
        // if (!GameController.isMinigameActive && !hasWon && Vector3.Distance(player.position, interactableObject.position) <= interactionRange)
        // {
        //     keyIndicator.SetActive(true);
        //     keyIndicatorText.text = "Press E to Start Mixing";
        //     if (Input.GetKeyDown(interactionKey))
        //     {
        //         StartMinigame();
        //     }
        // }
        // else if (!GameController.isMinigameActive && !hasWon)
        // {
        //     keyIndicator.SetActive(false); // Hide indicator if not near the object
        // }

        // Minigame logic
        if (GameController.isMinigameActive)
        {
            HandleKeyPresses();
            DecayGauge();
            CheckForSuccess();
        }

        // Dismiss the success message with Spacebar
        if (hasWon && Input.GetKeyDown(KeyCode.Space))
        {
            HideSuccessMessage();
        }
    }

    private void HandleKeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.A) && lastKeyPressed != KeyCode.A)
        {
            IncreaseGauge();
            lastKeyPressed = KeyCode.A;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastKeyPressed != KeyCode.D)
        {
            IncreaseGauge();
            lastKeyPressed = KeyCode.D;
        }
    }

    private void IncreaseGauge()
    {
        gaugeBar.value += fillAmountPerPress;
        gaugeBar.value = Mathf.Clamp01(gaugeBar.value);
    }

    private void DecayGauge()
    {
        gaugeBar.value -= decayRate * Time.deltaTime;
        gaugeBar.value = Mathf.Clamp01(gaugeBar.value);
    }

    private void CheckForSuccess()
    {
        if (gaugeBar.value >= 0.99f)  // Added buffer for floating-point precision
        {
            successMessage.gameObject.SetActive(true);
            successMessage.text = "Mixing Complete!";
            GameController.isMinigameActive = true;       // Keep active to dismiss the message
            hasWon = true;                                // Set minigame completion flag
            gaugeBar.gameObject.SetActive(false);         // Hide the gauge after completion
            keyIndicator.SetActive(false);                // Hide key indicator after success
        }
    }

    public void StartMinigame()
    {
        GameController.isMinigameActive = true;          // Set the global flag
        hasWon = false;
        gaugeBar.value = 0;
        successMessage.gameObject.SetActive(false);
        gaugeBar.gameObject.SetActive(true);             // Show the gauge when minigame starts
        keyIndicator.SetActive(true);                    // Activate the key indicator
        keyIndicatorText.text = "Press A and D to Mix!"; // Set the key indicator text
    }

    private void HideSuccessMessage()
    {
        successMessage.gameObject.SetActive(false);
        hasWon = false;
        keyIndicator.SetActive(false);                   // Ensure key indicator is hidden after dismissing
        GameController.isMinigameActive = false;         // Disable minigame after dismissing
    }
}
