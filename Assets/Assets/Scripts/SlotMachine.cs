using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    [Header("Reels")]
    public RectTransform[] reels;

    [Header("Symbols")]
    public Sprite[] symbols;

    [Header("Settings")]
    public float spinDuration = 2f;
    public float symbolChangeSpeed = 0.05f;
    public float spinMoveAmount = 40f;

    [HideInInspector]
    public bool isSpinning = false;

    private Vector2[] startPositions;
    private Image[] reelImages;

    void Start()
    {
        startPositions = new Vector2[reels.Length];
        reelImages = new Image[reels.Length];

        for (int i = 0; i < reels.Length; i++)
        {
            startPositions[i] = reels[i].anchoredPosition;
            reelImages[i] = reels[i].GetComponent<Image>();
        }
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(Spin());
        }
    }

    IEnumerator Spin()
{
    isSpinning = true;

    float timer = 0;

    while (timer < spinDuration)
    {
        timer += Time.deltaTime;

        for (int i = 0; i < reels.Length; i++)
        {
            reelImages[i].sprite = symbols[Random.Range(0, symbols.Length)];

            reels[i].anchoredPosition =
                startPositions[i] + Vector2.down * Random.Range(0, spinMoveAmount);
        }

        yield return null;
    }

    // volver
    for (int i = 0; i < reels.Length; i++)
    {
        reels[i].anchoredPosition = startPositions[i];
    }

    isSpinning = false;
}
}