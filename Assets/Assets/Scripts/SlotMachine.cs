using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SlotSymbol
{
    public Sprite sprite;
    [Range(1, 100)]
    public int weight = 10;
}

public class SlotMachine : MonoBehaviour
{
    [Header("Reels")]
    public SlotReelUI[] reels;

    [Header("Symbols")]
    public SlotSymbol[] symbols;

    [Header("Settings")]
    public float spinDuration = 2f;
    public float reelStopDelay = 0.5f;

    [Header("Economy")]
    public MoneyTracker moneyTracker;

    [Header("Events")]
    public UnityEvent onWin;
    public UnityEvent onLose;

    [HideInInspector]
    public bool isSpinning = false;

    public void StartSpin()
    {
        if (!isSpinning && (moneyTracker == null || moneyTracker.CanSpin()))
            StartCoroutine(SpinSequence());
    }

    IEnumerator SpinSequence()
    {
        isSpinning = true;
        moneyTracker?.PlaceBet();

        Sprite[] available = GetAllSprites();

        Sprite[] results = new Sprite[reels.Length];
        for (int i = 0; i < reels.Length; i++)
            results[i] = PickWeightedSymbol();

        foreach (var reel in reels)
            reel.StartSpin(available);

        yield return new WaitForSeconds(spinDuration);

        int stoppedCount = 0;
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].Stop(results[i], () => stoppedCount++);

            if (i < reels.Length - 1)
                yield return new WaitForSeconds(reelStopDelay);
        }

        yield return new WaitUntil(() => stoppedCount >= reels.Length);

        CheckWin(results);
        isSpinning = false;
    }

    Sprite PickWeightedSymbol()
    {
        if (symbols == null || symbols.Length == 0) return null;

        int totalWeight = 0;
        foreach (var s in symbols) totalWeight += s.weight;

        int roll = Random.Range(0, totalWeight);
        int accumulated = 0;

        foreach (var s in symbols)
        {
            accumulated += s.weight;
            if (roll < accumulated) return s.sprite;
        }

        return symbols[symbols.Length - 1].sprite;
    }

    Sprite[] GetAllSprites()
    {
        if (symbols == null || symbols.Length == 0) return new Sprite[0];

        Sprite[] result = new Sprite[symbols.Length];
        for (int i = 0; i < symbols.Length; i++)
            result[i] = symbols[i].sprite;
        return result;
    }

    void CheckWin(Sprite[] results)
    {
        for (int i = 1; i < results.Length; i++)
        {
            if (results[i] != results[0])
            {
                onLose.Invoke();
                return;
            }
        }

        moneyTracker?.Win();
        onWin.Invoke();
    }
}
