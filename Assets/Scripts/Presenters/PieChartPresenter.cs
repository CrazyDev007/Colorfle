using UnityEngine;

public class PieChartPresenter
{
    private readonly PieChartView _view;

    public PieChartPresenter(PieChartView view)
    {
        _view = view;
    }

    public void SetGuessColors(Color color)
    {
        color.a = 1;
        _view.SetGuessColors(color, ColorfleGameManager.instance.CurrentIndex++);
    }

    public void SetTargetAndResetGuess(Color targetColor)
    {
        //
        var opaqueTarget = targetColor;
        opaqueTarget.a = 1f;
        _view.ResetPieChart(opaqueTarget);
    }
}