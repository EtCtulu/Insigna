using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TriggerEnd : MonoBehaviour
{

    public GameObject player;
    public SpriteRenderer playerSprite;
    public CinemachineVirtualCamera oldCam;
    public CinemachineVirtualCamera newCam;

    public GameObject[] cinematicTexts;

    public GameObject endCinematic;

    public GameObject choix;
    private GameObject journal;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            journal = GameObject.Find("Journal");
            journal.GetComponent<Button>().enabled = false;
            player.GetComponent<PlayerInput>().enabled = false;
            playerSprite.enabled = false;

            CameraManager.Instance.setCameraPrioHigh(newCam);
            CameraManager.Instance.setCameraPrioLow(oldCam);

            endCinematic.GetComponent<Animator>().SetTrigger("End");

            StartCoroutine(AfterEndAnim());
        }
    }

    IEnumerator AfterEndAnim()
    {
        yield return new WaitForSeconds(14f);

        UIManager.Instance.DisplayPortrait(0);
        cinematicTexts[0].SetActive(true);

        yield return new WaitForSeconds(6f);

        cinematicTexts[0].SetActive(false);
        cinematicTexts[1].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(2);
        cinematicTexts[1].SetActive(false);
        cinematicTexts[2].SetActive(true);

        yield return new WaitForSeconds(6f);

        cinematicTexts[2].SetActive(false);
        cinematicTexts[3].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(3);
        cinematicTexts[3].SetActive(false);
        cinematicTexts[4].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(0);
        cinematicTexts[4].SetActive(false);
        cinematicTexts[5].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(2);
        cinematicTexts[5].SetActive(false);
        cinematicTexts[6].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(0);
        cinematicTexts[6].SetActive(false);
        cinematicTexts[7].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(2);
        cinematicTexts[7].SetActive(false);
        cinematicTexts[8].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(0);
        cinematicTexts[8].SetActive(false);
        cinematicTexts[9].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(2);
        cinematicTexts[9].SetActive(false);
        cinematicTexts[10].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(0);
        cinematicTexts[10].SetActive(false);
        cinematicTexts[11].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        UIManager.Instance.DisplayPortrait(2);
        cinematicTexts[11].SetActive(false);
        cinematicTexts[12].SetActive(true);

        yield return new WaitForSeconds(6f);

        UIManager.Instance.HidePortraits();
        choix.SetActive(true);
    }
}
