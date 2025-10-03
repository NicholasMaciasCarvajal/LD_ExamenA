using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float gravedad = -9.81f;
    public float fuerzaSalto = 5f;

    private CharacterController controlador;
    private Vector3 velocidadJugador;
    private bool estaEnElSuelo;

    private PlayerInputAction inputActions;
    private Vector2 inputMovimiento;
    private bool saltoPresionado;

    void Awake()
    {
        inputActions = new PlayerInputAction();

        inputActions.Player.Move.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => inputMovimiento = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => saltoPresionado = true;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        controlador = GetComponent<CharacterController>();

        Debug.Log("Input registrado: " + inputActions.Player.Move);
    }


    void Update()
    {
        estaEnElSuelo = controlador.isGrounded;

        if (estaEnElSuelo && velocidadJugador.y < 0)
        {
            velocidadJugador.y = -1f;
        }

        Vector3 movimiento = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;
        controlador.Move(movimiento * velocidad * Time.deltaTime);

        if (saltoPresionado && estaEnElSuelo)
        {
            velocidadJugador.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            saltoPresionado = false;
        }

        velocidadJugador.y += gravedad * Time.deltaTime;
        controlador.Move(velocidadJugador * Time.deltaTime);

        Debug.Log("Movimiento: " + inputMovimiento); // DEBUG
    }
}
