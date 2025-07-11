using System.Collections.Generic;
using UnityEngine;

public class PaletteGridPresenter
{
    private PaletteGridView _paletteGridView;
    private IGameplayMediator _gameplayScreen;

    public PaletteGridPresenter(PaletteGridView paletteGridView, IGameplayMediator gameplayScreen)
    {
        _paletteGridView = paletteGridView;
        _gameplayScreen = gameplayScreen;
    }

    // Call this from the palette color button's OnClick event, passing the color index
    public void OnPaletteColorClicked(int colorIndex)
    {
        var gameManger = GameManager.instance;
        if (gameManger.CurrentIndex >= 3)
        {
            Debug.Log("You can only select 3 colors.");
            return;
        }

        // Prevent duplicate selection
        for (int i = 0; i <= gameManger.CurrentIndex; i++)
        {
            if (GameManager.instance.SelectedColorIndices[i] == colorIndex)
            {
                Debug.Log("Color already selected.");
                return;
            }
        }


        GameManager.instance.SelectedColorIndices[gameManger.CurrentIndex] = colorIndex;
        // Call PieChart.SetGuessColors with the selected color
        var color = _paletteGridView.paletteColors[colorIndex];
        _gameplayScreen.Notify(_paletteGridView, GameplayEvent.PaletteColorClicked, color);
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