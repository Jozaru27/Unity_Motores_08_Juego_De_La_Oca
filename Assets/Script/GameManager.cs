using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    int[] vectorCasillas;
    int[] infoCasillas;
    GameObject[] vectorObjetos;

    public GameObject fichaJugador;
    public GameObject fichaIA;

    public TMP_Text textoRonda;
    public TMP_Text textoJugador;
    public Button botonAdelante;
    public Button botonAtras;

    private int posicionJugador = 0;
    private int posicionIA = 0;
    private bool turnoJugador = true;
    private int ronda = 1;


    private void Awake()
    {

        vectorCasillas = new int[21];
        infoCasillas   = new int[21];

        // RELLENAMOS EL VECTOR DE CASILLAS
        for (int i = 0; i < vectorCasillas.Length; i++)
            vectorCasillas[i] = 0;

        // RELLENAMOS EL VECTOR DE INFO CASILLAS
        for (int i = 0; i < infoCasillas.Length; i++)
            infoCasillas[i] = 0;

        // teleports, destino siempre +6
        infoCasillas[0] = 1;
        infoCasillas[5] = 1;

        // volver a tirar
        infoCasillas[12] = 2;
        infoCasillas[17] = 2;

        // retroceder 3 casillas
        infoCasillas[4] = -1;
        infoCasillas[9] = -1;
        infoCasillas[13] = -1;
        infoCasillas[18] = -1;
        infoCasillas[19] = -1;

        // victoria
        infoCasillas[20] = 99;

        // RELLENAMOS EL VECTOR DE GAMEOBJECTS
        //vectorObjetos = GameObject.FindGameObjectsWithTag("casilla");

        // METODO 1: OBTENER LOS HIJOS DE UN PARENT VAC�O

        // METODO 2: RELLENAR CON UN FOR Y UN FIND
        vectorObjetos = new GameObject[21];

        for (int i = 0; i < vectorObjetos.Length; i++)
            vectorObjetos[i] = GameObject.Find("casilla" + i);


        // METODO 3: ORDENAR LA LISTA A PARTIR DE LA LISTA DE TAGS
        // LA MAS COMPLICADA PERO LA MAS EFICIENTE

        // 21 CASILLAS DESORDENADAS
        botonAdelante.gameObject.SetActive(false);
        botonAtras.gameObject.SetActive(false);

        for (int i = 1; i < vectorObjetos.Length; i++)
        {
            vectorObjetos[i] = GameObject.Find("casilla" + i.ToString("D2")); 

            if (vectorObjetos[i] == null)
            {
                Debug.LogError("No se encontró el GameObject con el nombre casilla" + i.ToString("D2"));
            }
        }

        AsignarColoresCasillas();

    }

    // Start is called before the first frame update
    void Start()
    {
        ActualizarUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnoJugador && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TurnoJugador());
        }
    }

    IEnumerator TurnoJugador()
    {
        int tirada = TirarDado();
        Debug.Log("El jugador ha sacado un " + tirada);

        MoverFicha(ref posicionJugador, tirada, fichaJugador);
        yield return new WaitForSeconds(1f);

        AplicarModificador(ref posicionJugador, fichaJugador);
        yield return new WaitForSeconds(1f);

        if (posicionJugador == posicionIA)
        {
            botonAdelante.gameObject.SetActive(true);
            botonAtras.gameObject.SetActive(true);

            botonAdelante.onClick.AddListener(() => ResolverColision(true));
            botonAtras.onClick.AddListener(() => ResolverColision(false));

            yield break;
        }

        turnoJugador = false;
        StartCoroutine(TurnoIA());
    }

    IEnumerator TurnoIA()
    {
        yield return new WaitForSeconds(1f);

        int tirada = TirarDado();
        Debug.Log("La IA ha sacado un " + tirada);

        MoverFicha(ref posicionIA, tirada, fichaIA);
        yield return new WaitForSeconds(1f);

        AplicarModificador(ref posicionIA, fichaIA);
        yield return new WaitForSeconds(1f);

        if (posicionIA == posicionJugador)
        {
            ResolverColisionIA();
        }

        turnoJugador = true;
        ronda++;
        ActualizarUI();
    }

    int TirarDado()
    {
        return Random.Range(1, 7);
    }

    void MoverFicha(ref int posicion, int tirada, GameObject ficha)
    {
        vectorCasillas[posicion] = 0;
        posicion = Mathf.Min(posicion + tirada, 20);
        ficha.transform.position = vectorObjetos[posicion].transform.position;
        vectorCasillas[posicion] = (ficha == fichaJugador) ? 1 : 2;
    }

    void AplicarModificador(ref int posicion, GameObject ficha)
    {
        if (infoCasillas[posicion] == 1)
        {
            posicion += 6;
        }
        else if (infoCasillas[posicion] == 2)
        {
            return;
        }
        else if (infoCasillas[posicion] == -1)
        {
            posicion = Mathf.Max(0, posicion - 3);
        }

        ficha.transform.position = vectorObjetos[posicion].transform.position;
    }

    void ResolverColision(bool adelante)
    {
        posicionJugador += (adelante ? 1 : -1);
        posicionJugador = Mathf.Clamp(posicionJugador, 0, 20);
        fichaJugador.transform.position = vectorObjetos[posicionJugador].transform.position;

        botonAdelante.gameObject.SetActive(false);
        botonAtras.gameObject.SetActive(false);
        botonAdelante.onClick.RemoveAllListeners();
        botonAtras.onClick.RemoveAllListeners();

        turnoJugador = false;
        StartCoroutine(TurnoIA());
    }

    void ResolverColisionIA()
    {
        if (posicionIA < 20 && infoCasillas[posicionIA + 1] > 0)
        {
            posicionIA++;
        }
        else if (posicionIA > 0 && infoCasillas[posicionIA - 1] >= 0)
        {
            posicionIA--;
        }
        else
        {
            posicionIA += (Random.value > 0.5f) ? 1 : -1;
        }

        posicionIA = Mathf.Clamp(posicionIA, 0, 20);
        fichaIA.transform.position = vectorObjetos[posicionIA].transform.position;
    }

    void ActualizarUI()
    {
        textoRonda.text = "Ronda: " + ronda;
        textoJugador.text = turnoJugador ? "Turno: Jugador" : "Turno: IA";
    }

    void AsignarColoresCasillas()
    {
        // Colores que vamos a usar para las casillas especiales
        Color colorNormal = Color.white;  // Casilla normal
        Color colorTeletransporte = Color.blue;  // Teletransporte
        Color colorVolverATirar = Color.green;  // Volver a tirar
        Color colorRetroceder = Color.red;  // Retroceder 3 casillas
        Color colorFinal = Color.yellow;  // Casilla final

        // Iteramos sobre las casillas para asignar los colores
        for (int i = 0; i < vectorObjetos.Length; i++)
        {
            // Asegurarse de que el objeto no sea null
            if (vectorObjetos[i] != null)
            {
                // Accedemos al componente Renderer de la casilla (puede ser un SpriteRenderer o MeshRenderer)
                Renderer renderer = vectorObjetos[i].GetComponent<Renderer>();

                // Si hay un Renderer, asignamos el color dependiendo del valor en infoCasillas
                if (renderer != null)
                {
                    switch (infoCasillas[i])
                    {
                        case 1:
                            renderer.material.color = colorTeletransporte;  // Teletransporte // LA DE DESTINO EN AZUL TAMBIÉN
                            break;
                        case 2:
                            renderer.material.color = colorVolverATirar;  // Volver a tirar
                            break;
                        case -1:
                            renderer.material.color = colorRetroceder;  // Retroceder 3 casillas
                            break;
                        case 99:
                            renderer.material.color = colorFinal;  // Casilla final
                            break;
                        default:
                            renderer.material.color = colorNormal;  // Casilla normal
                            break;
                    }
                }
            }
        }
    }

}
