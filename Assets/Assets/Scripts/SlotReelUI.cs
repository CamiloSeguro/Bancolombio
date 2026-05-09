using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotReelUI : MonoBehaviour
{
    public RectTransform[] icons;
    public Sprite[] sprites;

    public float speed = 600f;
    public float iconSpacing = 200f;

    private bool spinning = false;
    private Image[] iconImages;

    void Start()
    {
        iconImages = new Image[icons.Length];
        for (int i = 0; i < icons.Length; i++)
            iconImages[i] = icons[i].GetComponent<Image>();
    }

    public void StartSpin(float duration)
    {
        if (!spinning)
            StartCoroutine(Spin(duration));
    }

    IEnumerator Spin(float duration)
    {
        spinning = true;

        float timer = 0f;

        while (timer < duration)
        {
            foreach (RectTransform icon in icons)
            {
                icon.anchoredPosition -= Vector2.up * speed * Time.deltaTime;

                // Si sale por abajo
                if (icon.anchoredPosition.y < -iconSpacing)
                {
                    // mover arriba
                    float highestY = GetHighestY();
                    icon.anchoredPosition = new Vector2(
                        icon.anchoredPosition.x,
                        highestY + iconSpacing
                    );

                    int idx = System.Array.IndexOf(icons, icon);
                    iconImages[idx].sprite = sprites[Random.Range(0, sprites.Length)];
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        spinning = false;
    }

    float GetHighestY()
    {
        if (icons == null || icons.Length == 0) return 0f;

        float highest = icons[0].anchoredPosition.y;

        foreach (RectTransform icon in icons)
        {
            if (icon.anchoredPosition.y > highest)
                highest = icon.anchoredPosition.y;
        }

        return highest;
    }
}