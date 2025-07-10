using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ColorPercentage
{
    public Color color;
    [Range(0, 100)] public float percentage;
}

public class ColorSelector : MonoBehaviour
{
    public Color targetColor;
    public List<ColorPercentage> colorPercentages;

    [SerializeField] private List<float> weights;


    public Color GetTargetColor()
    {
        var guessIndex = new List<int>();
        var guessColors = new List<Color>();
        while (guessColors.Count < 3)
        {
            var index = Random.Range(0, colorPercentages.Count);
            var newColor = colorPercentages[index].color;
            if (!guessColors.Contains(newColor))
            {
                guessIndex.Add(index);
                guessColors.Add(newColor);
            }
        }

        foreach (var guessColor in guessIndex)
            Debug.Log(guessColor);
        targetColor = GetMixColor(guessColors.ToArray());
        return targetColor;
    }

    public Color GetMixColor(Color[] guessedColors)
    {
        var r = 0f;
        var g = 0f;
        var b = 0f;
        var a = 0f;
        for (var i = 0; i < guessedColors.Length; i++)
        {
            var weight = weights[i];
            r += guessedColors[i].r * weight;
            g += guessedColors[i].g * weight;
            b += guessedColors[i].b * weight;
            a += guessedColors[i].a * weight;
        }

        var result = new Color(r, g, b, a);

        return result;
    }
}