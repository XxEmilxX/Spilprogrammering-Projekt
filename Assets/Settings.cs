using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI qualityText;

    public void SetQuality(float quality)
    {
        QualitySettings.SetQualityLevel((int)quality);
        qualityText.text = "Quality: " + QualitySettings.names[(int)quality];
    }
}
