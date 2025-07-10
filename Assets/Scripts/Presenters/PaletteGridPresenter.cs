using System.Collections.Generic;
using UnityEngine;

public class PaletteGridPresenter
{
    private PaletteGridView _paletteGridView;

    public PaletteGridPresenter(PaletteGridView paletteGridView)
    {
        _paletteGridView = paletteGridView;
    }

    // Call this from the palette color button's OnClick event, passing the color index
    public void OnPaletteColorClicked(int colorIndex)
    {
        if (_paletteGridView == null || colorIndex < 0 || colorIndex >= _paletteGridView.paletteColors.Count)
        {
            Debug.Log("Invalid color selection.");
            return;
        }

        // Prevent duplicate selection
        for (int i = 0; i < GameplayScreen.instance.selectionCount; i++)
        {
            if (ColorfleGameManager.instance.selectedColorIndices[i] == colorIndex)
            {
                Debug.Log("Color already selected.");
                return;
            }
        }

        if (GameplayScreen.instance.selectionCount >= 3)
        {
            Debug.Log("You can only select 3 colors.");
            return;
        }

        ColorfleGameManager.instance.selectedColorIndices[GameplayScreen.instance.selectionCount] = colorIndex;
        GameplayScreen.instance.selectionCount++;
        // Call PieChart.SetGuessColors with the selected color
        if (GameplayScreen.instance.pieChartView != null)
        {
            var color = _paletteGridView.paletteColors[colorIndex];
            GameplayScreen.instance.pieChartView.SetGuessColor(color);
        }

        GameplayScreen.instance.guessGridView.UpdateGuessGridUI();
        var colorInfo = _paletteGridView.paletteColors[colorIndex];
        var c = colorInfo;
        Debug.Log($"Selected Color {colorIndex + 1}: RGB({(int)(c.r * 255)}, {(int)(c.g * 255)}, {(int)(c.b * 255)})");
    }

    public Color GetTargetColor()
    {
        var guessIndex = new List<int>();
        var guessColors = new List<Color>();
        while (guessColors.Count < 3)
        {
            var index = Random.Range(0, _paletteGridView.paletteColors.Count);
            var newColor = _paletteGridView.paletteColors[index];
            if (!guessColors.Contains(newColor))
            {
                guessIndex.Add(index);
                guessColors.Add(newColor);
            }
        }

        foreach (var guessColor in guessIndex)
            Debug.Log(guessColor);
        _paletteGridView.targetColor = GetMixColor(guessColors.ToArray());
        return _paletteGridView.targetColor;
    }

    public Color GetMixColor(Color[] guessedColors)
    {
        var r = 0f;
        var g = 0f;
        var b = 0f;
        var a = 0f;
        for (var i = 0; i < guessedColors.Length; i++)
        {
            var weight = _paletteGridView.weights[i];
            r += guessedColors[i].r * weight;
            g += guessedColors[i].g * weight;
            b += guessedColors[i].b * weight;
            a += guessedColors[i].a * weight;
        }

        var result = new Color(r, g, b, a);

        return result;
    }
}