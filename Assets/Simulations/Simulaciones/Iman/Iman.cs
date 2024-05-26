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
        // Aquí puedes agregar cualquier lógica adicional necesaria para iniciar la simulación del imán
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

    // Método para calcular la fuerza entre dos imanes
    public Vector3 CalcularFuerza(Vector3 posiciónOtroImán, Polaridad polaridadOtroImán, float h, float friccion, float fuerzaImanes)
    {
        //Se calcula la distancia entre los imanes
        float distancia = Vector3.Distance(transform.position, posiciónOtroImán);


        //Se asigna el atraer para las polaridades opuestas
        bool atraer = (polaridad == Polaridad.Positivo && polaridadOtroImán == Polaridad.Negativo) || (polaridad == Polaridad.Negativo && polaridadOtroImán == Polaridad.Positivo);

        //Ley de coulomb F= (k*q1*q1)/r*r
        float fuerza = fuerzaImanes / (distancia * distancia);

        //Si se repelen, la fuerza toma el lado contrario
        if (!atraer)
        {
            fuerza *= -1;
        }

        //Se calculan las direcciones a tomar
        Vector3 dirección = posiciónOtroImán - transform.position;
        dirección.Normalize();


        Vector3 fuerzaFinal = dirección * fuerza;


        // Aplicar efectos adicionales como h y fricción
        fuerzaFinal *= h;
        fuerzaFinal -= friccion * fuerzaFinal;

        // Si los imanes están muy cerca, evitan clippeos
        if (distancia < distanciaMinima)
        {
            Vector3 ajuste = dirección * (distanciaMinima - distancia) / 2f;
            transform.position -= ajuste;
        }

        return fuerzaFinal;
    }

    // Método para aplicar fuerza al imán
    public void AplicarFuerza(Vector3 fuerza, float gravedad)
    {
        // Aplica fuerza
        transform.position += fuerza * Time.deltaTime;

        // Detecta colisión con el plano y ajusta la posición si es necesario
        if (transform.position.y != transform.position.y) // Si la posición Y del imán está por debajo del plano
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); // Ajusta la posición Y del imán para que la esfera quede completamente sobre el plano
        }
    }

}
