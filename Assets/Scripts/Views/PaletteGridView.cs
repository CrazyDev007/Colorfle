using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PaletteGridView : MonoBehaviour
{
    [FormerlySerializedAs("gameplayScreenMediatorMediator")] [FormerlySerializedAs("gameplayScreenMediator")] [FormerlySerializedAs("mediator")] [SerializeField] private GameplayScreen gameplayScreen;
    public Image[] paletteButtons; // Assign palette button images in Inspector
    public Color targetColor;

    [FormerlySerializedAs("colorPercentages")]
    public List<Color> paletteColors;

    public List<float> weights;

    private PaletteGridPresenter _paletteGridPresenter;

    private void Awake()
    {
        _paletteGridPresenter = new PaletteGridPresenter(this, gameplayScreen);
        SetPaletteButtonColors();
    }

    public void OnPaletteColorClicked(int colorIndex)
    {
        _paletteGridPresenter.OnPaletteColorClicked(colorIndex);
    }

    public void SetPaletteButtonColors()
    {
        for (var i = 0; i < paletteButtons.Length && i < paletteColors.Count; i++)
        {
            var color = paletteColors[i];
            color.a = 1;
            paletteButtons[i].color = color;
        }
    }

    public Color GetTargetColor()
    {
        return _paletteGridPresenter.GetTargetColor();
    }

    public Color GetMixColor(Color[] guessedColors)
    {
        return _paletteGridPresenter.GetMixColor(guessedColors);
    }
}