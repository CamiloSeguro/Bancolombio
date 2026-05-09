using UnityEngine;

public class HandTutorialMenu : MonoBehaviour
{
    public GameObject tutorialMenu;
    public Transform playerCamera;

    private bool isOpen = false;

    void Start()
    {
        tutorialMenu.SetActive(false);
    }

    void Update()
    {
        // Detecta pinch con mano derecha
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            ToggleMenu();
        }

        // Detecta pinch con mano izquierda
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;

        tutorialMenu.SetActive(isOpen);

        if (isOpen)
        {
            tutorialMenu.transform.position =
                playerCamera.position + playerCamera.forward * 1.5f;

            tutorialMenu.transform.LookAt(playerCamera);

            tutorialMenu.transform.forward *= -1;
        }

        Debug.Log("MENU TOGGLE");
    }
}