using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject freeCamPrefab;
    private Camera originalCamera;
    private GameObject freeCamInstance;
    private GameObject player;
    private GameObject ui;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No GameObject with the tag 'Player' found!");
        }

        ui = GameObject.Find("Overlay UI");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !GameController.isMinigameActive && !TabManager.journalActive)
        {
            ToggleFreeCam();
            GameController.isMinigameActive = true;
            SetPlayerVisibility(false);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ReturnToOriginalCamera();
            GameController.isMinigameActive = false;
            SetPlayerVisibility(true);
        }
    }

    private void ToggleFreeCam()
    {
        if (freeCamInstance == null)
        {
            GameObject camHolder = GameObject.Find("Cameraholder");
            if (camHolder == null)
            {
                camHolder = new GameObject("Cameraholder");
            }

            originalCamera = Camera.main;
            if (originalCamera == null)
            {
                Debug.LogWarning("No Main Camera found!");
                return;
            }

            freeCamInstance = Instantiate(freeCamPrefab, originalCamera.transform.position, originalCamera.transform.rotation);
            freeCamInstance.transform.SetParent(camHolder.transform);

            originalCamera.gameObject.SetActive(false);
            freeCamInstance.GetComponent<Camera>().tag = "MainCamera";
        }
    }

    private void ReturnToOriginalCamera()
    {
        if (freeCamInstance != null)
        {
            Destroy(freeCamInstance);
            originalCamera.gameObject.SetActive(true);
            originalCamera.tag = "MainCamera";
        }
    }

    private void SetPlayerVisibility(bool isVisible)
    {
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = isVisible;
        }

        if (ui != null) {
            ui.SetActive(isVisible);
        }
    }
}
