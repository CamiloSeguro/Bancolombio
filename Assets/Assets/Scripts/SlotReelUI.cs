using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotReelUI : MonoBehaviour
{
    public RectTransform[] icons;
    public float speed = 800f;
    public float iconSpacing = 200f;
    public float decelerationDuration = 0.8f;

    private Image[] iconImages;
    private bool spinning;
    private bool stopping;
    private Sprite resultSprite;
    private System.Action onStopped;

    void Start()
    {
        iconImages = new Image[icons.Length];
        for (int i = 0; i < icons.Length; i++)
            iconImages[i] = icons[i].GetComponent<Image>();
    }

    public void StartSpin(Sprite[] available)
    {
        if (!spinning)
            StartCoroutine(SpinCoroutine(available));
    }

    public void Stop(Sprite result, System.Action callback)
    {
        if (!spinning)
        {
            callback?.Invoke();
            return;
        }

        resultSprite = result;
        onStopped = callback;
        stopping = true;
    }

    IEnumerator SpinCoroutine(Sprite[] available)
    {
        spinning = true;
        stopping = false;
        float currentSpeed = speed;
        float decelTimer = 0f;
        bool resultAssigned = false;

        while (spinning)
        {
            if (stopping)
            {
                decelTimer += Time.deltaTime;
                currentSpeed = Mathf.Lerp(speed, 0f, decelTimer / decelerationDuration);
            }

            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].anchoredPosition -= Vector2.up * currentSpeed * Time.deltaTime;

                if (icons[i].anchoredPosition.y < -iconSpacing)
                {
                    icons[i].anchoredPosition = new Vector2(
                        icons[i].anchoredPosition.x,
                        GetHighestY() + iconSpacing
                    );

                    if (stopping && !resultAssigned)
                    {
                        iconImages[i].sprite = resultSprite;
                        resultAssigned = true;
                    }
                    else
                    {
                        iconImages[i].sprite = available[Random.Range(0, available.Length)];
                    }
                }
            }

            if (stopping && currentSpeed < 5f)
            {
                SnapToGrid();
                spinning = false;
            }

            yield return null;
        }

        onStopped?.Invoke();
    }

    void SnapToGrid()
    {
        int centerIdx = 0;
        float minDist = float.MaxValue;
        for (int i = 0; i < icons.Length; i++)
        {
            float dist = Mathf.Abs(icons[i].anchoredPosition.y);
            if (dist < minDist) { minDist = dist; centerIdx = i; }
        }

        iconImages[centerIdx].sprite = resultSprite;
        icons[centerIdx].anchoredPosition = new Vector2(icons[centerIdx].anchoredPosition.x, 0);

        int slot = 0;
        for (int i = 0; i < icons.Length; i++)
        {
            if (i == centerIdx) continue;
            icons[i].anchoredPosition = new Vector2(
                icons[i].anchoredPosition.x,
                slot == 0 ? iconSpacing : -iconSpacing
            );
            slot++;
        }
    }

    float GetHighestY()
    {
        if (icons == null || icons.Length == 0) return 0f;
        float highest = icons[0].anchoredPosition.y;
        for (int i = 1; i < icons.Length; i++)
            if (icons[i].anchoredPosition.y > highest)
                highest = icons[i].anchoredPosition.y;
        return highest;
    }
}
