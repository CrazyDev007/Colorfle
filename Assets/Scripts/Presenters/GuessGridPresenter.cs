using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GuessGridPresenter
{
    private GuessGridView _guessGridView;

    public GuessGridPresenter(GuessGridView guessGridView)
    {
        _guessGridView = guessGridView;
    }

    public void UpdateGuessGridUI()
    {
        // Update the guess grid UI to show selected colors
        for (int i = 0; i < 3; i++)
        {
            if (i < _guessGridView.guessGridSlots.Length)
            {
                if (ColorfleGameManager.instance.selectedColorIndices[i] >= 0)
                {
                    var color = _guessGridView.PaletteGridView.paletteColors[
                        ColorfleGameManager.instance.selectedColorIndices[i]];
                    color.a = 1;
                    _guessGridView.SetSlotColor(color, i);
                }
                else
                {
                    _guessGridView.SetSlotColor(Color.white, i);
                }
            }
        }
    }


    public void OnDeleteLastGuessColor()
    {
        if (GameplayScreen.instance.selectionCount <= 0)
            return;
        GameplayScreen.instance.selectionCount--;
        ColorfleGameManager.instance.selectedColorIndices[GameplayScreen.instance.selectionCount] = -1;
        // Reset the corresponding guess grid slot
        if (_guessGridView.guessGridSlots != null &&
            GameplayScreen.instance.selectionCount < _guessGridView.guessGridSlots.Length)
            _guessGridView.guessGridSlots[GameplayScreen.instance.selectionCount].color = Color.white;
        // Reset the pie chart guess index and re-apply remaining colors
        if (GameplayScreen.instance.pieChartView != null && GameplayScreen.instance.paletteGridView != null)
        {
            GameplayScreen.instance.pieChartView.ResetGuessIndex();
            // Set all guessImage slots to gray
            foreach (var img in GameplayScreen.instance.pieChartView.GetType().GetField("guessImage",
                             BindingFlags.NonPublic | BindingFlags.Instance)
                         .GetValue(GameplayScreen.instance.pieChartView) as Image[])
            {
                if (img != null)
                    img.color = Color.gray;
            }

            for (int i = 0; i < GameplayScreen.instance.selectionCount; i++)
            {
                if (ColorfleGameManager.instance.selectedColorIndices[i] >= 0)
                {
                    var color = GameplayScreen.instance.paletteGridView
                        .paletteColors[ColorfleGameManager.instance.selectedColorIndices[i]];
                    GameplayScreen.instance.pieChartView.SetGuessColor(color);
                }
            }
        }
    }
}