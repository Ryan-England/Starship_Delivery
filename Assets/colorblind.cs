using UnityEngine;
using UnityEngine.UI;

public class ColorBlindMode : MonoBehaviour
{
    public Button protan;
    public Button tritan;
    public Button deuter;

    private bool isProtanActive = false;
    private bool isTritanActive = false;
    private bool isDeuterActive = false;

    void Start()
    {
        protan.onClick.AddListener(() => SetMode("Protan"));
        tritan.onClick.AddListener(() => SetMode("Tritan"));
        deuter.onClick.AddListener(() => SetMode("Deuter"));
    }

    void SetMode(string mode)
    {
        isProtanActive = (mode == "Protan");
        isTritanActive = (mode == "Tritan");
        isDeuterActive = (mode == "Deuter");

        Debug.Log($"Protan: {isProtanActive}, Tritan: {isTritanActive}, Deuter: {isDeuterActive}");
    }
}
