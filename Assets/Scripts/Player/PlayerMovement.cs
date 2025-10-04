using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad;
    public float gravedad;
    public float fuerzaSalto;

    [Header("Escalada")]
    public float climbSpeed;
    public float staminaMax;
    public float staminaRecoveryRate;

    [Header("UI")]
    public Image barraEstamina; 

    [Header("Estado (solo lectura)")]
    public float staminaActual;
    public bool puedeEscalar = false;
    public bool estaEscalando = false;

    private CharacterController controlador;
    private PlayerInputAction inputActions;

    private Vector2 inputMovimiento;
    private bool saltoPresionado;
    private bool escalandoPresionado;

    private Vector3 velocidadJugador;

    void Awake()
    {
        inputActions = new PlayerInputAction();

        inputActions.Player.Move.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => inputMovimiento = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => saltoPresionado = true;
        inputActions.Player.Climb.performed += ctx => escalandoPresionado = true;
        inputActions.Player.Climb.canceled += ctx => escalandoPresionado = false;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Start()
    {
        controlador = GetComponent<CharacterController>();
        staminaActual = staminaMax;
        ActualizarBarraEstamina();
    }

    void Update()
    {
        bool estaEnElSuelo = controlador.isGrounded;

        Vector3 movimiento = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;

        if (puedeEscalar && escalandoPresionado && staminaActual > 0f)
        {
            estaEscalando = true;

            velocidadJugador.y = climbSpeed;

            staminaActual -= Time.deltaTime;
            if (staminaActual <= 0f)
            {
                staminaActual = 0f;
                estaEscalando = false;
            }
        }
        else
        {
            estaEscalando = false;

            if (estaEnElSuelo && velocidadJugador.y < 0)
                velocidadJugador.y = -1f;
            else
                velocidadJugador.y += gravedad * Time.deltaTime;

            if (staminaActual < staminaMax)
                staminaActual += staminaRecoveryRate * Time.deltaTime;

            staminaActual = Mathf.Clamp(staminaActual, 0f, staminaMax);
        }

        if (saltoPresionado && estaEnElSuelo && !estaEscalando)
        {
            velocidadJugador.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            saltoPresionado = false;
        }

        Vector3 movimientoFinal = movimiento * velocidad + new Vector3(0, velocidadJugador.y, 0);
        controlador.Move(movimientoFinal * Time.deltaTime);

        ActualizarBarraEstamina();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
            puedeEscalar = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
            puedeEscalar = false;
    }

    private void ActualizarBarraEstamina()
    {
        if (barraEstamina != null)
        {
            barraEstamina.fillAmount = staminaActual / staminaMax;
        }
    }
}
