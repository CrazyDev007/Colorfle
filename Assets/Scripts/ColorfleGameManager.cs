using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ColorfleGameManager : MonoBehaviour
{
    public static ColorfleGameManager instance;

    [Tooltip("Number of guesses allowed.")]
    public int maxAttempts = 6;

    [Tooltip("Event fired when the game ends.")]
    public UnityEvent<bool> onGameOver;

    public int CurrentIndex { get; set; }
    public int Attempts { get; set; }
    public int[] selectedColorIndices = new int[3] { -1, -1, -1 };


    [FormerlySerializedAs("colorSelector")]
    public PaletteGridView paletteGridView; // Reference to ColorSelector


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        Attempts = 0;
    }

    private bool ColorsEqual(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }


    public void OnSubmitGuess()
    {
        // Collect selected colors
        Color[] guessColors = new Color[3];
        int selected = 0;
        for (int i = 0; i < 3; i++)
        {
            if (selectedColorIndices[i] >= 0 && paletteGridView != null)
            {
                guessColors[i] = paletteGridView.paletteColors[selectedColorIndices[i]];
                selected++;
            }
            else
            {
                guessColors[i] = Color.white;
            }
        }

        if (selected < 3)
        {
            Debug.Log("Please select 3 colors before submitting your guess.");
            return;
        }

        // Calculate total percentage and compare with target
        var targetColor = paletteGridView.targetColor;
        var targetMixColor = paletteGridView.GetMixColor(guessColors);
        if (targetColor != targetMixColor)
        {
            Debug.Log($"guess color ({targetMixColor}%) does not match target color ({targetColor}%).");
            return;
        }

        onGameOver?.Invoke(true);
        Debug.Log($"Guess submitted. Resulting color: {targetMixColor}");
    }
}