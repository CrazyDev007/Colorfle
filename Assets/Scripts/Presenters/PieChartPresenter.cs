using UnityEngine;

public class PieChartPresenter
{
    private int _currentIndex = 0;

    private readonly PieChartView _view;

    public PieChartPresenter(PieChartView view)
    {
        _view = view;
    }

    public void SetGuessColors(Color color)
    {
        color.a = 1;
        if (_currentIndex < _view.GuessImagesLength)
        {
            _view.SetGuessColors(color, _currentIndex++);
        }
    }

    public void ResetGuessIndex()
    {
        _currentIndex = 0;
    }

    public void SetTargetAndResetGuess(Color targetColor)
    {
        //
        var opaqueTarget = targetColor;
        opaqueTarget.a = 1f;
        _view.ResetPieChart(opaqueTarget);
        //
        ResetGuessIndex();
    }
}