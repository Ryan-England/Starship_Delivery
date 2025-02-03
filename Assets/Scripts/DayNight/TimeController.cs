using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    private DateTime currentTime; 
    public Text displayTime;

    public Light sunLight;

    public float sunriseHour; 

    public float sunsetHour;

    public TimeSpan sunriseTime;

    public TimeSpan sunsetTime;

    public Color dayAmbientLight;

    public Color nightAmbientLight;

    public Color cameraDayColor;

    public Color cameraNightColor;

    public AnimationCurve lightChangeCurve;

    public float maxSunLightIntensity;

    public Light moonLight;

    public float maxMoonLightIntensity;

    public Camera c;
    private float timeOfDay = 0f;
    public float transitionSpeed = 0.1f;      // Speed of the transition
    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
    
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        // c.backgroundColor = cameraDayColor;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay(){
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        timeOfDay += Time.deltaTime * transitionSpeed; // Increase time of da
        if (timeOfDay > 1f) timeOfDay = 0f; // Loop back to night
        if(displayTime != null){
            displayTime.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun(){
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime){
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage); 
            // c.backgroundColor = cameraDayColor;
        }
        else{
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
            // c.backgroundColor = cameraNightColor;
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings(){
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime){
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0){
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}
