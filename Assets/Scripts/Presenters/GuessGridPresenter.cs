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
                if (ColorfleUIManager.instance.selectedColorIndices[i] >= 0)
                {
                    _guessGridView.SetSlotColor(_guessGridView.ColorSelector
                        .colorPercentages[ColorfleUIManager.instance.selectedColorIndices[i]]
                        .color, i);
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
        if (ColorfleUIManager.instance.selectionCount <= 0)
            return;
        ColorfleUIManager.instance.selectionCount--;
        ColorfleUIManager.instance.selectedColorIndices[ColorfleUIManager.instance.selectionCount] = -1;
        // Reset the corresponding guess grid slot
        if (_guessGridView.guessGridSlots != null &&
            ColorfleUIManager.instance.selectionCount < _guessGridView.guessGridSlots.Length)
            _guessGridView.guessGridSlots[ColorfleUIManager.instance.selectionCount].color = Color.white;
        // Reset the pie chart guess index and re-apply remaining colors
        if (ColorfleUIManager.instance.pieChartView != null && ColorfleUIManager.instance.colorSelector != null)
        {
            ColorfleUIManager.instance.pieChartView.ResetGuessIndex();
            // Set all guessImage slots to gray
            foreach (var img in ColorfleUIManager.instance.pieChartView.GetType().GetField("guessImage",
                             BindingFlags.NonPublic | BindingFlags.Instance)
                         .GetValue(ColorfleUIManager.instance.pieChartView) as Image[])
            {
                if (img != null)
                    img.color = Color.gray;
            }

            for (int i = 0; i < ColorfleUIManager.instance.selectionCount; i++)
            {
                if (ColorfleUIManager.instance.selectedColorIndices[i] >= 0)
                {
                    var color = ColorfleUIManager.instance.colorSelector
                        .colorPercentages[ColorfleUIManager.instance.selectedColorIndices[i]].color;
                    ColorfleUIManager.instance.pieChartView.SetGuessColor(color);
                }
            }
        }
    }
}