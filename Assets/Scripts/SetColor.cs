using UnityEngine;
using System.Collections;

public class SetColor : MonoBehaviour
{
    public Light luz;
    public Color ColorNuevo;
    public GameObject ObjetoAMover;
    public float velocidadMovimiento; 
    public float limiteMovimiento;  

    private bool yaMoviendose = false;     
    private Vector3 posicionInicial;

    private void Start()
    {
        if (ObjetoAMover != null)
            posicionInicial = ObjetoAMover.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            luz.color = ColorNuevo;

            if (!yaMoviendose && ObjetoAMover != null)
                StartCoroutine(MoverHaciaArriba());
        }
    }

    private IEnumerator MoverHaciaArriba()
    {
        yaMoviendose = true;
        Vector3 destino = posicionInicial + Vector3.up * limiteMovimiento;

        while (ObjetoAMover.transform.position.y < destino.y)
        {
            ObjetoAMover.transform.position += Vector3.up * velocidadMovimiento * Time.deltaTime;

            if (ObjetoAMover.transform.position.y > destino.y)
                ObjetoAMover.transform.position = destino;

            yield return null;
        }

        yaMoviendose = false;
    }
}
