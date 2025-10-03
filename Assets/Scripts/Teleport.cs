using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    public Transform TpTransform;
    public UnityEvent evento;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = TpTransform.position;
        evento.Invoke();
    }
}
