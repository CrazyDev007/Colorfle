using UnityEngine;
using UnityEngine.UI;

public class GuessGridView : MonoBehaviour
{
    [SerializeField] private ColorSelector mColorSelector;
    public Image[] guessGridSlots; // Assign 3 UI Image components for guess grid slots in the Inspector

    private GuessGridPresenter _presenter;

    public ColorSelector ColorSelector => mColorSelector;

    private void Start()
    {
        _presenter = new GuessGridPresenter(this);
    }

    public void SetSlotColor(Color color, int index)
    {
        guessGridSlots[index].color = color;
    }

    public void UpdateGuessGridUI()
    {
        _presenter.UpdateGuessGridUI();
    }

    public void OnDeleteLastGuessColor()
    {
        _presenter.OnDeleteLastGuessColor();
    }
}