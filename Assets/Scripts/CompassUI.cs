using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    [Tooltip("List of GameObjects with the QuestMarker script representing active objectives in the world.")]
    [SerializeField] private List<QuestMarker> questMarkers = new List<QuestMarker>();
    [Tooltip("The UI element that represents the compass bar.")]
    [SerializeField] private RectTransform compassBarTransform;
    [Tooltip("Prefab used to create objective markers objects nested under compassBar.")]
    [SerializeField] private RectTransform objectiveMarkerPrefab;
    [Tooltip("The UI marker representing the north direction on the compass.")]
    [SerializeField] private RectTransform northMarkerTransform;
    [Tooltip("The UI marker representing the south direction on the compass.")]
    [SerializeField] private RectTransform southMarkerTransform;
    [Tooltip("Reference to the player's camera transform.")]
    [SerializeField] private Transform cameraObjectTransform;
   
    // List of currently active objective markers displayed on the compass
    private List<RectTransform> activeObjectiveMarkers = new List<RectTransform>();

    // Compass tutorial for starting inspiration: https://www.youtube.com/watch?v=XcpTC1VYVNE
    private void Start()
    {
        // Iterate through all QuestMarkers and add them to the compass
        for (int i = 0; i < questMarkers.Count; i++)
        {
            // Calls AddQuestMarker for each marker in the questMarkers list
            AddQuestMarker(questMarkers[i]);

            if (questMarkers[i].IsQuest) {
                HideQuestMarker(questMarkers[i]);
            }
        }
    }

    private void Update()
    {
        // Set position for the North and South markers
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 10000);
        SetMarkerPosition(southMarkerTransform, Vector3.back * 10000);

        // Loop through all active objective markers and update their positions
        int index = 0;
        foreach (RectTransform markerTransform in activeObjectiveMarkers)
        {
            if (index < questMarkers.Count)
            {
                // Get the corresponding QuestMarker from index
                QuestMarker questMarker = questMarkers[index];

                // Update the marker's position based on the quest marker's world position
                SetMarkerPosition(markerTransform, questMarker.transform.position);

                // Get the Image component attached to the marker and set its icon
                Image markerImage = markerTransform.GetComponent<Image>();
                questMarker.SetIcon(markerImage);
            }
            index++; // Move to the next quest marker
        }
    }

    // Adds a new QuestMarker to the compass
    public void AddQuestMarker(QuestMarker questMarker)
    {
        // If the questMarker is empty or the prefab is empty, STOP.
        if (questMarker == null || objectiveMarkerPrefab == null) {
            return;
        }

        // Instantiate a new objective marker and place it under the compass UI
        RectTransform newMarker = Instantiate(objectiveMarkerPrefab, compassBarTransform);
        activeObjectiveMarkers.Add(newMarker); // Add the marker to the active list
        //Debug.LogWarning("QuestMarker added. Total questMarkers count: " + activeObjectiveMarkers.Count);
    }

    // Sets the position of a marker on the compass based on its world position
    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition) {
        // Calculate the direction from the camera to the target world position
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;

        // Project the direction to 2D (ignore y-axis height) for easier angle calculation
        Vector2 directionToTarget2D = new Vector2(directionToTarget.x, directionToTarget.z);
        Vector2 cameraForward2D = new Vector2(cameraObjectTransform.forward.x, cameraObjectTransform.forward.z);

        // Calculate the signed angle between the camera's forward direction and the direction to the marker
        float angle = Vector2.SignedAngle(cameraForward2D, directionToTarget2D);

        // Normalize the angle to fit the compass position range (-1 to 1)
        float compassPositionX = Mathf.Clamp(angle / 180f, -1, 1);  // -1 is left, 1 is right

        // Smoothly transition the marker's position using Lerp
        float smoothPositionX = Mathf.Lerp(markerTransform.anchoredPosition.x, compassBarTransform.rect.width / 2 * compassPositionX, Time.deltaTime * 10f);

        // Set the marker's position on the compass based on the calculated X position
        markerTransform.anchoredPosition = new Vector2(smoothPositionX, 0);
    }

    // Shows a specific quest marker on the compass
    public void ShowQuestMarker(QuestMarker questMarker)
    {
        int index = questMarkers.IndexOf(questMarker);
        if (index >= 0 && index < activeObjectiveMarkers.Count)
        {
            // Set the corresponding marker visible
            activeObjectiveMarkers[index].gameObject.SetActive(true);
        }
    }

    // Hides a specific quest marker from the compass
    public void HideQuestMarker(QuestMarker questMarker)
    {
        int index = questMarkers.IndexOf(questMarker);
        if (index >= 0 && index < activeObjectiveMarkers.Count)
        {
            // Set the corresponding marker invisible
            activeObjectiveMarkers[index].gameObject.SetActive(false);
        }
    }
}
