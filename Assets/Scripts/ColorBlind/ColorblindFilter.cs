using UnityEngine;
using UnityEngine.UI;

public class ColorblindFilter : MonoBehaviour
{
    public Material protanopiaMaterial;
    public Material deuteranopiaMaterial;
    public Material tritanopiaMaterial;
    private Material currentMaterial;
    private Camera cam;

    private enum ColorblindMode { Normal, Protanopia, Deuteranopia, Tritanopia }
    private ColorblindMode currentMode = ColorblindMode.Normal;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("ColorblindFilter script must be attached to a Camera.");
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Press 'C' to toggle modes
        {
            ToggleColorblindMode();
        }
    }

    void ToggleColorblindMode()
    {
        currentMode = (ColorblindMode)(((int)currentMode + 1) % 4);
        switch (currentMode)
        {
            case ColorblindMode.Normal:
                currentMaterial = null;
                break;
            case ColorblindMode.Protanopia:
                currentMaterial = protanopiaMaterial;
                break;
            case ColorblindMode.Deuteranopia:
                currentMaterial = deuteranopiaMaterial;
                break;
            case ColorblindMode.Tritanopia:
                currentMaterial = tritanopiaMaterial;
                break;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (currentMaterial != null)
        {
            Graphics.Blit(source, destination, currentMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
