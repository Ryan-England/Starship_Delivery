using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlindFilter : MonoBehaviour
{
    public Volume postProcessVolume;

    void Start()
    {
        // Get the PostProcessVolume attached to the GameObject
        postProcessVolume = GetComponent<Volume>();

        // Check if the volume has the ColorGrading effect
    }

    void Update()
    {
        if(Menu1.protan){
            if (postProcessVolume.profile.TryGet(out ColorAdjustments ca))
            {
                // Successfully got the ColorGrading settings
                ca.hueShift.value = 15f;
            }
            else
            {
                Debug.LogError("ColorGrading effect not found in PostProcessProfile.");
            }
        }
        else if(Menu1.detuer){
            if (postProcessVolume.profile.TryGet(out ColorAdjustments ca))
            {
                // Successfully got the ColorGrading settings
                ca.hueShift.value = -19f;
                ca.saturation.value = 31f;
            }
            else
            {
                Debug.LogError("ColorGrading effect not found in PostProcessProfile.");
            }
        }
        else if(Menu1.trit){
            if (postProcessVolume.profile.TryGet(out ColorAdjustments ca))
            {
                // Successfully got the ColorGrading settings
                ca.hueShift.value = -44f;
                // ca.saturation.value = 31f;
            }
            else
            {
                Debug.LogError("ColorGrading effect not found in PostProcessProfile.");
            }
        }
    }
}
