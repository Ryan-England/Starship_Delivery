using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlindFilter : MonoBehaviour
{
    public Volume postProcessVolume;

    private static bool detuer;
    private static bool trit;
    private static bool protan;

    void Start()
    {
        // Get the PostProcessVolume attached to the GameObject
        postProcessVolume = GetComponent<Volume>();

        LoadColorSettings();

        // Check if the volume has the ColorGrading effect
    }

    public void LoadColorSettings()
    {
        detuer = PlayerPrefs.GetInt("detuer", 0) == 1;
        protan = PlayerPrefs.GetInt("protan", 0) == 1;
        trit = PlayerPrefs.GetInt("trit", 0) == 1;

        if (postProcessVolume.profile.TryGet(out ColorAdjustments ca))
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
}
