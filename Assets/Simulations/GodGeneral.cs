using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodGeneral : MonoBehaviour
{
    [SerializeField] float h;
    [SerializeField] float friction;
    [SerializeField] float gravity;

    // Generaciones
    public GameObject[] GobjCaidaLibre;
    public GameObject[] GobjParabolico;
    public GameObject[] GobjChoque;
    public GameObject[] GobjResorte;
    public GameObject[] GobjIman;

    // Componentes de las simulaciones
    private GodCaidaLibre caidaLibreController;
    private GodParabolico parabolicoController;
    private GodChoque choqueController;
    private GodImanes imanesController;
    private GodResorte resorteController;

    void Start()
    {
        // Asignar referencias a los controladores específicos
        caidaLibreController = GetComponent<GodCaidaLibre>();
        parabolicoController = GetComponent<GodParabolico>();
        choqueController = GetComponent<GodChoque>();
        imanesController = GetComponent<GodImanes>();

        resorteController = GetComponent<GodResorte>();

        // Inicializar simulaciones
        caidaLibreController?.Initialize(h, friction, gravity);
        parabolicoController?.Initialize(h, friction, gravity);
        choqueController?.Initialize(h, friction, gravity);
        imanesController?.Initialize(h, friction, gravity);

        resorteController?.Initialize(h, friction, gravity);
    }

    void Update()
    {
        // Actualizar simulaciones
        caidaLibreController?.Simulate();
        parabolicoController?.Simulate();
        choqueController?.Simulate();
        imanesController?.Simulate();
        //telaController?.Simulate();
        resorteController?.Simulate();
    }
}
