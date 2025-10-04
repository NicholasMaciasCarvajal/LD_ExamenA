using UnityEngine;
using UnityEngine.InputSystem; // Necesario para usar InputAction

[RequireComponent(typeof(CharacterController))]
public class PlayerClimb : MonoBehaviour
{
    [Header("Configuración general")]
    public float climbSpeed;
    public float staminaMax;
    public float staminaRecoveryRate;

    [Header("Estado (solo lectura)")]
    public float staminaActual;
    public bool puedeEscalar = false;
    public bool estaEscalando = false;

    private CharacterController controller;
    private Vector3 moveDir;

    private PlayerInputAction inputActions;
    private InputAction climbAction;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        climbAction = inputActions.Player.Climb;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        staminaActual = staminaMax;
    }

    void Update()
    {
        bool presionandoEscalar = climbAction != null && climbAction.ReadValue<float>() > 0f;

        if (puedeEscalar && presionandoEscalar && staminaActual > 0f)
        {
            estaEscalando = true;
            moveDir = Vector3.up * climbSpeed;
            controller.Move(moveDir * Time.deltaTime);

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
            if (staminaActual < staminaMax)
                staminaActual += staminaRecoveryRate * Time.deltaTime;

            staminaActual = Mathf.Clamp(staminaActual, 0f, staminaMax);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            puedeEscalar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            puedeEscalar = false;
        }
    }
}
