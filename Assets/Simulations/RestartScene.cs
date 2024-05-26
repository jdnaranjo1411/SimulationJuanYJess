using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private GameObject player;
    public float activationRange = 2f;
    private string interactMessage = "Presiona 'E' para reiniciar las simulaciones";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                Restart();
            }
        }
    }

    public void Restart()
    {
        // Carga la escena actual de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
