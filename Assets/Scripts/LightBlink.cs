using UnityEngine;

public class LightBlink : MonoBehaviour
{
    private new Light light;
    [SerializeField] private float blinkIntensity = 1f;
    [SerializeField] private float blinkSmooth = 10f;

    private float initialIntensity;

    private void Start()
    {
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
    }

    private void Update()
    {
        Blink();
    }

    private void Blink()
    {
        float random = Random.Range(initialIntensity - blinkIntensity, initialIntensity + blinkIntensity);
        light.intensity = Mathf.Lerp(light.intensity, random, blinkSmooth * Time.deltaTime);
    }
}
