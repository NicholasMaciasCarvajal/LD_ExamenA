using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private PlayerInputAction inputActions;

    private void Awake()
    {
        // Inicializa el esquema de controles
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Asignar callbacks a las acciones
        inputActions.Player.Start.performed += OnStartPressed;
        inputActions.Player.Exit.performed += OnExitPressed;
    }

    private void OnDisable()
    {
        // Limpia las suscripciones
        inputActions.Player.Start.performed -= OnStartPressed;
        inputActions.Player.Exit.performed -= OnExitPressed;

        inputActions.Disable();
    }

    private void OnStartPressed(InputAction.CallbackContext ctx)
    {
        Comenzar();
    }

    private void OnExitPressed(InputAction.CallbackContext ctx)
    {
        Salir();
    }

    public void Comenzar()
    {
        SceneManager.LoadScene("Main");
    }

    public void Salir()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
