using UnityEngine;
using TMPro;

public class MoneyTracker : MonoBehaviour
{
    [Header("Balance")]
    public int startingBalance = 100;
    public int winMultiplier = 5;


    [Header("Bet")]
    public int betStep = 5;
    public int minBet = 5;
    public int maxBet = 100;

    [Header("UI")]
    public TextMeshProUGUI balanceText;
    public TextMeshProUGUI betText;

    public int Balance { get; private set; }
    public int CurrentBet { get; private set; }
    private int activeBet;

    void Start()
    {
        Balance = startingBalance;
        CurrentBet = minBet;
        UpdateUI();
    }

    public void AddBet()
    {
        CurrentBet = Mathf.Clamp(CurrentBet + betStep, minBet, Mathf.Min(maxBet, Balance));
        UpdateUI();
    }

    public void SubtractBet()
    {
        CurrentBet = Mathf.Max(CurrentBet - betStep, minBet);
        UpdateUI();
    }

    public bool CanSpin() => Balance >= CurrentBet && CurrentBet >= minBet;

    public void PlaceBet()
    {
        activeBet = CurrentBet;
        Balance -= activeBet;
        UpdateUI();
    }

    public void Win()
    {
        Balance += activeBet * winMultiplier;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (balanceText != null)
            balanceText.text = "$" + Balance;
        if (betText != null)
            betText.text = "Apuesta: $" + CurrentBet;
    }
}
