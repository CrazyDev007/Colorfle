using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the Colorfle game logic: generates a target color/percent combination,
/// processes guesses, and fires events for UI feedback.
/// </summary>
public class ColorfleGameManager : MonoBehaviour
{
    /// <summary>
    /// The palette of available colors to choose from.
    /// </summary>
    [Tooltip("Available colors to choose from.")]
    public Color[] palette;

    /// <summary>
    /// The maximum number of guesses allowed.
    /// </summary>
    [Tooltip("Number of guesses allowed.")]
    public int maxAttempts = 6;

    /// <summary>
    /// Invoked after each guess is evaluated, passing feedback for each slot.
    /// </summary>
    [Tooltip("Event fired after each guess is evaluated.")]
    public UnityEvent<FeedbackType[]> onGuessEvaluated;

    /// <summary>
    /// Invoked when the game ends. Passes true if player won, false otherwise.
    /// </summary>
    [Tooltip("Event fired when the game ends.")]
    public UnityEvent<bool> onGameOver;

    /// <summary>
    /// Feedback for each guessed slot.
    /// </summary>
    public enum FeedbackType { Correct, Misplaced, Absent }

    private Color[] targetColors = new Color[3];
    private int[] targetPercents = new int[3];
    private int attempts;

    /// <summary>
    /// Initializes the game by selecting a random target.
    /// </summary>
    private void Start()
    {
        GenerateTarget();
        attempts = 0;
    }

    /// <summary>
    /// Submits a guess and evaluates it against the target.
    /// </summary>
    /// <param name="guessColors">Array of 3 guessed colors.</param>
    /// <param name="guessPercents">Array of 3 guessed percentages (sum = 100).</param>
    /// <returns>FeedbackType array for each slot.</returns>
    public FeedbackType[] SubmitGuess(Color[] guessColors, int[] guessPercents)
    {
        if (guessColors == null || guessPercents == null || guessColors.Length != 3 || guessPercents.Length != 3)
            throw new ArgumentException("Guess must have exactly 3 colors and 3 percents.");
        if (guessPercents.Sum() != 100)
            throw new ArgumentException("Guess percents must sum to 100.");

        var feedback = EvaluateGuess(guessColors, guessPercents);
        attempts++;
        onGuessEvaluated?.Invoke(feedback);

        bool won = feedback.All(f => f == FeedbackType.Correct);
        if (won || attempts >= maxAttempts)
        {
            onGameOver?.Invoke(won);
        }

        return feedback;
    }

    /// <summary>
    /// Evaluates the guess against the target and returns feedback for each slot.
    /// </summary>
    /// <param name="guessColors">Guessed colors.</param>
    /// <param name="guessPercents">Guessed percents.</param>
    /// <returns>FeedbackType array for each slot.</returns>
    private FeedbackType[] EvaluateGuess(Color[] guessColors, int[] guessPercents)
    {
        FeedbackType[] feedback = new FeedbackType[3];
        bool[] colorMatched = new bool[3];
        bool[] guessMatched = new bool[3];

        // First pass: check for correct (color and percent in correct slot)
        for (int i = 0; i < 3; i++)
        {
            if (ColorsEqual(guessColors[i], targetColors[i]) && guessPercents[i] == targetPercents[i])
            {
                feedback[i] = FeedbackType.Correct;
                colorMatched[i] = true;
                guessMatched[i] = true;
            }
        }

        // Second pass: check for misplaced (color exists elsewhere)
        for (int i = 0; i < 3; i++)
        {
            if (feedback[i] == FeedbackType.Correct)
                continue;

            bool found = false;
            for (int j = 0; j < 3; j++)
            {
                if (!colorMatched[j] && ColorsEqual(guessColors[i], targetColors[j]))
                {
                    found = true;
                    colorMatched[j] = true;
                    break;
                }
            }
            feedback[i] = found ? FeedbackType.Misplaced : FeedbackType.Absent;
        }

        return feedback;
    }

    /// <summary>
    /// Generates a random target of 3 unique colors and 3 percents summing to 100.
    /// </summary>
    private void GenerateTarget()
    {
        if (palette == null || palette.Length < 3)
            throw new InvalidOperationException("Palette must have at least 3 colors.");

        // Pick 3 unique indices
        int[] indices = Enumerable.Range(0, palette.Length).OrderBy(_ => UnityEngine.Random.value).Take(3).ToArray();
        for (int i = 0; i < 3; i++)
            targetColors[i] = palette[indices[i]];

        // Generate 3 random integer percents that sum to 100
        int a = UnityEngine.Random.Range(1, 100 - 1);
        int b = UnityEngine.Random.Range(1, 100 - a);
        int c = 100 - a - b;
        int[] percents = new int[] { a, b, c };
        percents = percents.OrderBy(_ => UnityEngine.Random.value).ToArray();
        for (int i = 0; i < 3; i++)
            targetPercents[i] = percents[i];
    }

    /// <summary>
    /// Compares two colors for equality (ignores alpha).
    /// </summary>
    private bool ColorsEqual(Color a, Color b)
    {
        return Mathf.Approximately(a.r, b.r) &&
               Mathf.Approximately(a.g, b.g) &&
               Mathf.Approximately(a.b, b.b);
    }
}