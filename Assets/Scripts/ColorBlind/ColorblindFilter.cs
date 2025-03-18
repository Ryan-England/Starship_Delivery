using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class ColorBlindFilter : MonoBehaviour
{
    [Header("External References")]
    [Tooltip("")]
    public Volume postProcessVolume;
    [Tooltip("Images to apply color-filter to.")]
    public Image[] uiImages;
    [Tooltip("Text elements to apply color-filter to.")]
    public TextMeshProUGUI[] uiTexts;

    private static bool detuer;
    private static bool trit;
    private static bool protan;

    private Color[] originalImageColors;
    private Color[] originalTextColors;

    void Start()
    {
        if (postProcessVolume == null) {
            postProcessVolume = GetComponent<Volume>();
        }
        
        StoreOriginalColors();

        LoadColorSettings();
    }

    public void LoadColorSettings()
    {
        detuer = PlayerPrefs.GetInt("detuer", 0) == 1;
        protan = PlayerPrefs.GetInt("protan", 0) == 1;
        trit = PlayerPrefs.GetInt("trit", 0) == 1;

        ApplyColorFilter();
    }

    public void ApplyColorFilter()
    {
        // Apply to game world
        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments ca))
        {
            SetColorFilter(ca);
        }

        // Apply to UI elements
        ApplyUIColorFilter();
    }

    private void ApplyUIColorFilter()
    {
        if (!protan && !detuer && !trit) {
            ApplyOriginalColors();
        } else {
            Color adjustmentColor = GetUIColor();

            // Adjust UI images
            foreach (Image img in uiImages)
            {
                if (img != null)
                {
                    img.color = adjustmentColor;
                }
            }

            // Adjust UI text elements
            foreach (TextMeshProUGUI text in uiTexts)
            {
                if (text != null)
                {
                    text.color = adjustmentColor;
                }
            }
        }
    }

    private Color GetUIColor()
    {
        if (protan)
            return new Color(1.0f, 0.8f, 0.8f); // Slight red tint
        else if (detuer)
            return new Color(0.8f, 1.0f, 0.8f); // Slight green tint
        else if (trit)
            return new Color(0.8f, 0.8f, 1.0f); // Slight blue tint
        else
            return Color.white; // Default
    }

    private void StoreOriginalColors() {
        originalImageColors = new Color[uiImages.Length];
        for (int i = 0; i < uiImages.Length; i++)
        {
            if (uiImages[i] != null)
                originalImageColors[i] = uiImages[i].color;
        }

        originalTextColors = new Color[uiTexts.Length];
        for (int i = 0; i < uiTexts.Length; i++)
        {
            if (uiTexts[i] != null)
                originalTextColors[i] = uiTexts[i].color;
        }
    }

    private void ApplyOriginalColors() {
        for (int i = 0; i < uiImages.Length; i++) {
            if (uiImages[i] != null) {
                uiImages[i].color = originalImageColors[i];
            }
        }

        for (int i = 0; i < uiTexts.Length; i++) {
            if (uiTexts[i] != null) {
                uiTexts[i].color = originalTextColors[i];
            }
        }
    }

    private void SetColorFilter(UnityEngine.Rendering.Universal.ColorAdjustments ca)
    {
        if (protan)
        {
            ca.hueShift.value = 15f;
        }
        else if (detuer)
        {
            ca.hueShift.value = -19f;
            ca.saturation.value = 31f;
        }
        else if (trit)
        {
            ca.hueShift.value = -44f;
            ca.saturation.value = 100f;
        }
        else
        {
            ca.hueShift.value = 0f;
            ca.saturation.value = 0f;
        }
    }
}
