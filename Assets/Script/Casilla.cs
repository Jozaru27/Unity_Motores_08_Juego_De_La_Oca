using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{

    public int numeroCasilla;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        // RELLENAMOS EL NUMERO DE LA CASILLA CON EL NOMBRE
        string casillaString = this.gameObject.name.Substring(7);
        numeroCasilla = int.Parse(casillaString);
    }
}
