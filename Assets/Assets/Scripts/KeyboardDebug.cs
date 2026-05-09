using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardDebug : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Debug.Log("TECLA DETECTADA");
            }

            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                Debug.Log("N DETECTADA");
            }
        }
    }
}