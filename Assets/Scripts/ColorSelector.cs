using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorPercentage
{
    public Color color;
    [Range(0, 100)]
    public float percentage;
}

public class ColorSelector : MonoBehaviour
{
    public List<ColorPercentage> colorPercentages;

    public Color GetRandomColor()
    {
        float total = 0f;
        foreach (var cp in colorPercentages)
            total += cp.percentage;

        float randomPoint = UnityEngine.Random.value * total;
        float cumulative = 0f;

        foreach (var cp in colorPercentages)
        {
            cumulative += cp.percentage;
            if (randomPoint <= cumulative)
                return cp.color;
        }
        return Color.white; // fallback
    }
}