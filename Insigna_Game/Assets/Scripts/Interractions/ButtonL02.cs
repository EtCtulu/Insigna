using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonL02 : MonoBehaviour
{
    private Interractable parent;

    public int lever1 = 0;
    public int lever2 = 0;
    public int lever3 = 0;
    public int lever4 = 0;

    private void Start()
    {
        parent = transform.parent.GetComponent<Interractable>();

    }

    void Update()
    {
        if (parent.interractionSecurity == false)
        {
            if (lever1 == 2 && lever2 == 1 && lever3 == 2 && lever4 == 0)
            {
                Debug.Log("Ta grosse m�re vincent");
            }
        }
    }
}