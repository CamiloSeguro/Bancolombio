using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SlotMachine : MonoBehaviour
{
    public Image[] reels;   // 👈 ESTO es la clave
    public Sprite[] symbols;

    public float spinDuration = 2f;
    public float spinSpeed = 0.1f;

    public void StartSpin()
    {
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        float time = 0;

        while (time < spinDuration)
        {
            time += Time.deltaTime;

            foreach (var reel in reels)
            {
                reel.sprite = symbols[Random.Range(0, symbols.Length)];
            }

            yield return new WaitForSeconds(spinSpeed);
        }
    }
}