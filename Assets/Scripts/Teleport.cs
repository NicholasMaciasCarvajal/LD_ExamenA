using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    [Tooltip("Destino del teletransporte")]
    public Vector3 TpTransform;

    private void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (!hit.CompareTag("Player") && other.transform.root != null)
            hit = other.transform.root.gameObject;

        if (!hit.CompareTag("Player"))
        {
            return;
        }

        if (TpTransform == null)
        {
            return;
        }


        CharacterController cc = hit.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Rigidbody rb = hit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
        }

        hit.transform.position = TpTransform;

        if (cc != null) cc.enabled = true;
    }
}
