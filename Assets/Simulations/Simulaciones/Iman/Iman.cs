using UnityEngine;

public class Iman : MonoBehaviour
{
    public enum Polaridad
    {
        Positivo,
        Negativo
    }

    public Polaridad polaridad;
    private float distanciaMinima = 1.0f;

    private bool isSimulating = false;
    private GameObject player;
    public float activationRange = 2f;
    private string interactMessage = "Presiona 'E' para interactuar";


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartSimulation()
    {
        isSimulating = true;
        // Aqu� puedes agregar cualquier l�gica adicional necesaria para iniciar la simulaci�n del im�n
    }
    void OnGUI()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 200, 50), interactMessage);
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= activationRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartSimulation();
            }
        }
    }

    // M�todo para calcular la fuerza entre dos imanes
    public Vector3 CalcularFuerza(Vector3 posici�nOtroIm�n, Polaridad polaridadOtroIm�n, float h, float friccion, float fuerzaImanes)
    {
        //Se calcula la distancia entre los imanes
        float distancia = Vector3.Distance(transform.position, posici�nOtroIm�n);


        //Se asigna el atraer para las polaridades opuestas
        bool atraer = (polaridad == Polaridad.Positivo && polaridadOtroIm�n == Polaridad.Negativo) || (polaridad == Polaridad.Negativo && polaridadOtroIm�n == Polaridad.Positivo);

        //Ley de coulomb F= (k*q1*q1)/r*r
        float fuerza = fuerzaImanes / (distancia * distancia);

        //Si se repelen, la fuerza toma el lado contrario
        if (!atraer)
        {
            fuerza *= -1;
        }

        //Se calculan las direcciones a tomar
        Vector3 direcci�n = posici�nOtroIm�n - transform.position;
        direcci�n.Normalize();


        Vector3 fuerzaFinal = direcci�n * fuerza;


        // Aplicar efectos adicionales como h y fricci�n
        fuerzaFinal *= h;
        fuerzaFinal -= friccion * fuerzaFinal;

        // Si los imanes est�n muy cerca, evitan clippeos
        if (distancia < distanciaMinima)
        {
            Vector3 ajuste = direcci�n * (distanciaMinima - distancia) / 2f;
            transform.position -= ajuste;
        }

        return fuerzaFinal;
    }

    // M�todo para aplicar fuerza al im�n
    public void AplicarFuerza(Vector3 fuerza, float gravedad)
    {
        // Aplica fuerza
        transform.position += fuerza * Time.deltaTime;

        // Detecta colisi�n con el plano y ajusta la posici�n si es necesario
        if (transform.position.y != transform.position.y) // Si la posici�n Y del im�n est� por debajo del plano
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Ajusta la posici�n Y del im�n para que la esfera quede completamente sobre el plano
        }
    }

}
