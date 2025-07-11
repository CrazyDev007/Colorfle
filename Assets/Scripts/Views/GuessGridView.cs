using UnityEngine;
using UnityEngine.Serialization;

public class GuessGridView : MonoBehaviour
{
    [FormerlySerializedAs("mColorSelector")] [SerializeField]
    private PaletteGridView mPaletteGridView;

    public GuessGridSlotView[] guessGridSlots0;
    public GuessGridSlotView[] guessGridSlots1;
    public GuessGridSlotView[] guessGridSlots2;
    public GuessGridSlotView[] guessGridSlots3;
    public GuessGridSlotView[] guessGridSlots4;
    public GuessGridSlotView[] guessGridSlots5;

    public GuessGridSlotView[] guessGridSlotsMix;

    private GuessGridPresenter _presenter;

    [SerializeField] private GameplayScreen gameplayScreen;

    public GuessGridSlotView[] GetGuessGridSlot(int attempt)
    {
        return attempt switch
        {
            0 => guessGridSlots0,
            1 => guessGridSlots1,
            2 => guessGridSlots2,
            3 => guessGridSlots3,
            4 => guessGridSlots4,
            _ => guessGridSlots5
        };
    }

    public PaletteGridView PaletteGridView => mPaletteGridView;

    private void Start()
    {
        _presenter = new GuessGridPresenter(this, gameplayScreen);
    }

    public void SetSlotColor(Color color, int index)
    {
        GetGuessGridSlot(GameManager.instance.Attempts)[index].SetSlotColor(color);
    }

    public void UpdateGuessGridUI()
    {
        _presenter.UpdateGuessGridUI();
    }

    public void OnWrongGuess()
    {
        var guessGridSlots = GetGuessGridSlot(GameManager.instance.Attempts);
        var targetColorIndices = GameManager.instance.TargetColorIndices;
        var selectedColorIndices = GameManager.instance.SelectedColorIndices;
        for (var i = 0; i < selectedColorIndices.Length; i++)
        {
            var colorIndex = -1;
            for (var j = 0; j < targetColorIndices.Length; j++)
            {
                if (selectedColorIndices[i] == targetColorIndices[j])
                {
                    colorIndex = j;
                }
            }

            var guessGridSlot = guessGridSlots[i];
            if (colorIndex == -1)
            {
                guessGridSlot.SetBorderImageActive(false);
            }
            else if (i == colorIndex)
            {
                guessGridSlot.SetBorderImageActive(true);
                guessGridSlot.SetBorderColor(Color.green);
            }
            else
            {
                guessGridSlot.SetBorderImageActive(true);
                guessGridSlot.SetBorderColor(Color.yellow);
            }
        }
    }
}