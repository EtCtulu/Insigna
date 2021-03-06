using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyWoder : MonoBehaviour
{
    public GameObject woder;
    public GameObject foliewoder;
    public GameObject foliemontechargewoder;
    public GameObject ladder1;
    public GameObject ladder2;
    public EnvrioManager em;

    private void Start()
    {
        Invoke("SetActive", 0.01f);
    }

    private void OnDestroy()
    {
        em.woderoff = true;
        woder.SetActive(false);
        if (foliewoder != null)
        {
            foliewoder.SetActive(false);
        }
        if (foliemontechargewoder != null)
        {
            foliemontechargewoder.SetActive(false);
        }
        if (ladder1 != null)
        {
            ladder1.SetActive(false);
        }
        if (ladder2 != null)
        {
            ladder2.SetActive(true);
        }
    }

    private void SetActive()
    {
        this.gameObject.SetActive(false);
        this.gameObject.GetComponent<PopupInterraction>().QuitInterraction();
    }
}
