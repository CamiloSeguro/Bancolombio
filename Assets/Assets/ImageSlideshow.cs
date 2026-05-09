using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cambia la imagen de un Canvas UI cada vez que el jugador hace click.
/// Compatible con Meta XR SDK (OVRInput).
/// Adjunta este script al GameObject que contenga el componente Image.
/// </summary>
public class ImageSlideshow : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("El componente Image del Canvas que mostrará las imágenes")]
    public Image displayImage;

    [Header("Imágenes (arrastra las 4 sprites aquí)")]
    [Tooltip("Lista de sprites a mostrar en orden. Se recomienda exactamente 4.")]
    public Sprite[] images;

    [Header("Configuración")]
    [Tooltip("Botón del controlador derecho para avanzar (por defecto: gatillo índice derecho)")]
    public OVRInput.Button advanceButton = OVRInput.Button.PrimaryIndexTrigger;

    [Tooltip("¿Volver a la primera imagen al llegar al final?")]
    public bool loop = false;

    // ─── Estado interno ───────────────────────────────────────────────
    private int _currentIndex = 0;
    private bool _buttonWasPressed = false;   // para detectar flanco ascendente

    // ─────────────────────────────────────────────────────────────────

    void Start()
    {
        // Validaciones básicas
        if (displayImage == null)
        {
            Debug.LogError("[ImageSlideshow] No se asignó el componente Image. " +
                           "Asigna el Image del Canvas en el Inspector.");
            enabled = false;
            return;
        }

        if (images == null || images.Length == 0)
        {
            Debug.LogError("[ImageSlideshow] El array de imágenes está vacío. " +
                           "Arrastra los Sprites al Inspector.");
            enabled = false;
            return;
        }

        // Mostrar la primera imagen al iniciar
        ShowImage(0);
    }

    void Update()
    {
        bool buttonIsPressed = OVRInput.Get(advanceButton);

        // Detecta el flanco ascendente (solo actúa en el momento del click)
        if (buttonIsPressed && !_buttonWasPressed)
        {
            OnClick();
        }

        _buttonWasPressed = buttonIsPressed;
    }

    /// <summary>
    /// Llamado una sola vez por cada click detectado.
    /// </summary>
    private void OnClick()
    {
        int nextIndex = _currentIndex + 1;

        if (nextIndex >= images.Length)
        {
            if (loop)
            {
                nextIndex = 0;
            }
            else
            {
                Debug.Log("[ImageSlideshow] Ya se mostraron todas las imágenes.");
                return; // No hay más imágenes
            }
        }

        ShowImage(nextIndex);
    }

    /// <summary>
    /// Muestra el sprite en el índice dado y actualiza el estado.
    /// </summary>
    private void ShowImage(int index)
    {
        if (index < 0 || index >= images.Length)
        {
            Debug.LogWarning($"[ImageSlideshow] Índice {index} fuera de rango.");
            return;
        }

        _currentIndex = index;
        displayImage.sprite = images[index];

        Debug.Log($"[ImageSlideshow] Mostrando imagen {index + 1} / {images.Length}");
    }

    // ─── API pública (opcional) ───────────────────────────────────────

    /// <summary>Avanza manualmente a la siguiente imagen (útil para botones UI).</summary>
    public void Next() => OnClick();

    /// <summary>Retrocede a la imagen anterior.</summary>
    public void Previous()
    {
        int prevIndex = _currentIndex - 1;
        if (prevIndex < 0) prevIndex = loop ? images.Length - 1 : 0;
        ShowImage(prevIndex);
    }

    /// <summary>Regresa a la primera imagen.</summary>
    public void ResetSlideshow() => ShowImage(0);
}