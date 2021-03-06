using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public PlayerData playerData;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.tag != "RangeNear")
        {
            playerData.ladderGO = transform.gameObject;
            playerData.ladderTaken = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.tag != "RangeNear")
        {
            playerData.ladderTaken = false;
        }
    }



}
