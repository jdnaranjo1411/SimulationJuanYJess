using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    [SerializeField] float h;
    [SerializeField] float friction;
    [SerializeField] float gravity;

    //Variables tela

    public int iRow;
    public int iCol;
    public int iSpace;
    public GameObject spherePrefab;

    //Variables resorte

    public float k;
    public float restLength;
    public Vector3 velocity;

    //Variables Imanes

    public float fuerzaImanes;

    //Materiales Imanes
    public Material Positivo;
    public Material Negativo;
    public GameObject imanPrefab;

    public List<Iman> imanes = new List<Iman>();
    public GameObject caja;


    //Generaciones

    public GameObject[] GobjCaidaLibre;
    bool CaidaLibre = false;

    public GameObject[] GobjParabolico;
    bool Parabolico = false;

    public GameObject[] GobjChoque;
    bool choque = false;

    public List<Tela> Telas = new List<Tela>();
    private Tela[,] TelaGrid;


    // Start is called before the first frame update
    void Start()
    {
        GobjCaidaLibre = GameObject.FindGameObjectsWithTag("CaidaLibre");
        GobjParabolico = GameObject.FindGameObjectsWithTag("Parabolico");
        GobjChoque = GameObject.FindGameObjectsWithTag("Choque");
        Tela[] TelasEnEscena = FindObjectsOfType<Tela>();
        Telas.AddRange(TelasEnEscena);

        Iman[] imanesEnEscena = FindObjectsOfType<Iman>();
        imanes.AddRange(imanesEnEscena);

        foreach (Iman iman in imanes)
        {
            AsignarMaterial(iman);
        }

    }

    private void AsignarMaterial(Iman iman)
    {
        Renderer rend = iman.GetComponent<Renderer>();
        if (iman.polaridad == Iman.Polaridad.Positivo && Positivo != null)
        {
            rend.material = Positivo;
        }
        else if (iman.polaridad == Iman.Polaridad.Negativo && Negativo != null)
        {
            rend.material = Negativo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Iman iman in imanes)
        {
            foreach (Iman otroIman in imanes)
            {
                if (iman != otroIman)
                {
                    // Calcular la fuerza entre el imán actual y otro imán
                    Vector3 fuerza = iman.CalcularFuerza(otroIman.transform.position, otroIman.polaridad, h, friction, fuerzaImanes);

                    // Aplicar la fuerza al imán actual
                    iman.AplicarFuerza(fuerza, gravity);
                }
            }
        }
        for (int i = 0; i < GobjCaidaLibre.Length; i++)
            {
                GobjCaidaLibre[i].GetComponent<ParticleMovement>().Shoot(h, friction, gravity);
            }

            for (int i = 0; i < GobjParabolico.Length; i++)
            {
                GobjParabolico[i].GetComponent<ParticleMovement>().Shoot(h, friction, gravity);
            }

        for (int i = 0; i < GobjChoque.Length - 1; i++)
        {
            Bolita bolita1 = GobjChoque[i].GetComponent<Bolita>();

            // Iterar sobre las bolitas restantes
            for (int j = i + 1; j < GobjChoque.Length; j++)
            {
                Bolita bolita2 = GobjChoque[j].GetComponent<Bolita>();

                // Verificar si ambas bolitas existen
                if (bolita1 != null && bolita2 != null)
                {
                    // Disparar cada bolita y verificar colisiones con la otra
                    bolita1.Shoot(h, friction, gravity, bolita2);
                    bolita2.Shoot(h, friction, gravity, bolita1);
                    bolita2.Shoot(h, friction, gravity, bolita1);
                }
            }
        }

        foreach (Tela Tela1 in Telas)
        {
            Tela1.Simulate(h, friction, gravity);
        }

        void createMesh()
        {
            TelaGrid = new Tela[iRow, iCol];
            for (int i = 0; i < iRow; i++)
            {
                for (int j = 0; j < iCol; j++)
                {
                    GameObject TelaGO = Instantiate(spherePrefab);
                    TelaGO.transform.position = new Vector3(i * iSpace, j * iSpace, 0);

                    Tela Tela1 = TelaGO.AddComponent<Tela>();
                    Telas.Add(Tela1);

                    TelaGrid[i, j] = Tela1;

                    if (i > 0)
                        Connect(Tela1, TelaGrid[i - 1, j]);

                    if (j > 0)
                        Connect(Tela1, TelaGrid[i, j - 1]);
                }
            }
        }
        void Connect(Tela TelaA, Tela TelaB)
        {
            // Conectar dos resortes agregándolos a las listas de resortes conectados entre sí
            TelaA.TelaConectada.Add(TelaB);
            TelaB.TelaConectada.Add(TelaA);
        }
    }
}
