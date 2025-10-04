using UnityEngine;
using UnityEngine.Events;

public class EventosInsanos : MonoBehaviour
{
    public UnityEvent evento;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            evento.Invoke();
        }
    }

}
