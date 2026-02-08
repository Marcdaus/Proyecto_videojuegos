using UnityEngine;

public class AgarrarObjeto : MonoBehaviour
{
    [Header("Configuración de agarre")]
    public Transform hand;                // Punto donde se sostiene el objeto
    public float radioDeteccion = 1.5f;   // Distancia para agarrar
    public LayerMask capaObjeto;          // Layer de objetos agarrables

    private GameObject objetoEnMano;
    private int layerOriginal;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetoEnMano == null)
                IntentarAgarrar();
            else
                Soltar();
        }

        // Opcional: mantener objeto pegado a la mano
        if (objetoEnMano != null)
        {
            objetoEnMano.transform.position = hand.position;
            objetoEnMano.transform.rotation = hand.rotation;
        }
    }

    void IntentarAgarrar()
    {
        // Detectar objetos cercanos en la layer Agarrable
        Collider[] objetos = Physics.OverlapSphere(
            transform.position,
            radioDeteccion,
            capaObjeto
        );

        if (objetos.Length == 0) return;

        GameObject obj = objetos[0].gameObject;

        // Evitar agarrar al personaje o objetos sin Rigidbody
        if (obj == gameObject) return;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("El objeto no tiene Rigidbody: " + obj.name);
            return;
        }

        // Guardamos layer original
        layerOriginal = obj.layer;

        // Agarrar
        objetoEnMano = obj;
        rb.isKinematic = true;                               // desactiva física
        obj.layer = LayerMask.NameToLayer("ObjetoEnMano");   // ignora colisiones con personaje
        obj.transform.SetParent(hand);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    void Soltar()
    {
        if (objetoEnMano == null) return;

        Rigidbody rb = objetoEnMano.GetComponent<Rigidbody>();
        rb.isKinematic = false;                             // reactiva física
        objetoEnMano.layer = layerOriginal;                 // vuelve a layer original
        objetoEnMano.transform.SetParent(null);
        objetoEnMano = null;
    }

    // Visualizar el radio de detección en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }
}
