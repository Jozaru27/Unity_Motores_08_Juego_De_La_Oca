using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    int[] vectorCasillas;
    int[] infoCasillas;
    GameObject[] vectorObjetos;

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
        GameObject[] vectorGOCasillas = GameObject.FindGameObjectsWithTag("casilla");

        for (int i = 0; i < vectorGOCasillas.Length; i++)
        {
            GameObject casilla = vectorGOCasillas[i];
            // falta terminar ..

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            int tirada = TirarDado();
            Debug.Log("El jugador ha sacado un " + tirada);
        }
    }

    public int TirarDado()
    {
        int resultado = Random.Range(1, 7); // Genera un número aleatorio entre 1 y 6 (incluidos)
        Debug.Log("Tirada del dado: " + resultado);
        return resultado;
    }

}
