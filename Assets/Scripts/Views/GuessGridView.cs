using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GuessGridView : MonoBehaviour
{
    [FormerlySerializedAs("mColorSelector")] [SerializeField] private PaletteGridView mPaletteGridView;
    public Image[] guessGridSlots; // Assign 3 UI Image components for guess grid slots in the Inspector

    private GuessGridPresenter _presenter;

    public PaletteGridView PaletteGridView => mPaletteGridView;

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