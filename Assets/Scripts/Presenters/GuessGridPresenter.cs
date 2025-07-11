using UnityEngine;

public class GuessGridPresenter
{
    private readonly GuessGridView _guessGridView;
    private readonly IGameplayMediator _gameplayScreen;

    public GuessGridPresenter(GuessGridView guessGridView, IGameplayMediator gameplayScreen)
    {
        _guessGridView = guessGridView;
        _gameplayScreen = gameplayScreen;
    }

    public void UpdateGuessGridUI()
    {
        // Update the guess grid UI to show selected colors
        for (int i = 0; i < 3; i++)
        {
            if (i < _guessGridView.GetGuessGridSlot(GameManager.instance.Attempts).Length)
            {
                if (GameManager.instance.SelectedColorIndices[i] >= 0)
                {
                    var color = _guessGridView.PaletteGridView.paletteColors[
                        GameManager.instance.SelectedColorIndices[i]];
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
}