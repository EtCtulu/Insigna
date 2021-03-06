using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PopupInterraction : MonoBehaviour
{


    // D�claration du joueur et du playerinput pour g�rer l'interraction depuis les inputs du joueur.
    private PlayerInput playerInputs;
    private GameObject player;

    // Bool qui indique si le joueur est proche de l'objet ou non
    public bool isNear = false;

    // Les gameobjects qui d�fissent l'interraction elles sont d�lcarr�es dans un ordre pr�cis
    private GameObject nearInt0;
    private GameObject farInt1;

    // Bool si l'int�raction est possible et une s�curit� pour ne pas afficher deux fois l'interraction.
    private bool security = false;
    private bool isInterractableOn = false;

    // Un bool qui indique si le curseur est devant l'objet.
    private bool cursorOn = false;

    // S�curise les interractions pour qu'elles ne se lancent pas au moment de l'interaction.
    public bool interractionSecurity = true;
    public SpriteRenderer spriteHighlight;

    //public GameObject vfx;

    [Header("Phrase a dire")]
    public string farPhrase;
    public string nearPhrase;

    [Header("Interraction IDX")]
    public int interractionIdx;

    private GameObject popUpList;

    private TextMeshProUGUI observationText;

    public int portraitIdx = 0;
    public bool isInterractionTalkative = false;

    private TextMeshProUGUI[] interactionsTexts;

    private GameObject[] allBoxColliders;
    private BoxCollider2D[] allBoxCollier2DDisabled = new BoxCollider2D[100];
    private int allDisBoxColLength = 0;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RangeNear"))
        {
            isNear = true;
        }
        if (cursorOn == true)
        {
            UIManager.Instance.SetPopUpCursor();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RangeNear"))
        {
            isNear = false;
            if (cursorOn == true)
            {
                UIManager.Instance.SetFarCursor();

            }
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInputs = player.GetComponent<PlayerInput>();
        playerInputs.actions.FindAction("Look").started += OnLook;
        playerInputs.actions.FindAction("Use").started += OnUse;
        popUpList = GameObject.Find("PopUps");
        nearInt0 = popUpList.transform.GetChild(interractionIdx).gameObject;
        QuitInterraction();
        nearInt0.SetActive(false);
        farInt1 = transform.GetChild(0).gameObject;
        observationText = farInt1.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        farInt1.SetActive(false);
        interractionSecurity = false;
        if (spriteHighlight != null)
        {
            spriteHighlight.enabled = false;
        }
        QuitInterraction();
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cursorOn == true && gameObject.activeSelf == true)
            {
                if (GameManager.Instance.hasInteracted)
                {
                    /*interactionsTexts = FindObjectsOfType<TextMeshProUGUI>();
                    for (int i = 0; i < interactionsTexts.Length; i++)
                    {
                        if (interactionsTexts[i] != null)
                        {
                            interactionsTexts[i].text = null;
                        }
                    }*/

                    GameManager.Instance.StopHidePortaitFonction();
                    GameManager.Instance.StopFarTextFonction();
                    GameManager.Instance.StopNearTextFonction();

                    if (GameManager.Instance.far_Text != null)
                    {
                        GameManager.Instance.far_Text.SetActive(false);
                    }
                    if (GameManager.Instance.near_Text != null)
                    {
                        GameManager.Instance.near_Text.SetActive(false);
                    }

                    UIManager.Instance.HidePortraits();
                    GameManager.Instance.hasInteracted = false;
                }

                if (GameManager.Instance.hasInteracted == false)
                {
                    GameManager.Instance.far_Text = farInt1;
                    GameManager.Instance.hasInteracted = true;
                    GameManager.Instance.StartHidePortaitFonction();
                    GameManager.Instance.StartFarTextFonction();
                }

                if (isNear == false)
                {
                    if (GameManager.Instance.globalInterractionSecurity == true)
                    {
                        if (GameManager.Instance.isNear == true)
                        {
                            security = false;
                            GameManager.Instance.globalInterractionSecurity = false;
                            if (GameObject.FindGameObjectWithTag("NearInt") != null)
                            {
                                for (int i = 0; i <= GameObject.FindGameObjectsWithTag("NearInt").Length; i++)
                                {
                                    GameObject.FindGameObjectsWithTag("NearInt")[i].SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            security = false;
                            GameManager.Instance.globalInterractionSecurity = false;
                            if (GameObject.FindGameObjectWithTag("FarInt") != null)
                            {
                                for (int i = 0; i <= GameObject.FindGameObjectsWithTag("FarInt").Length; i++)
                                {
                                    GameObject.FindGameObjectsWithTag("FarInt")[i].SetActive(false);
                                }
                            }
                        }
                    }
                    UIManager.Instance.DisplayPortrait(portraitIdx);
                    StartCoroutine(FarInterraction());
                    security = true;
                    GameManager.Instance.globalInterractionSecurity = true;
                    return;
                }
                if (isNear == true)
                {
                    if (GameManager.Instance.globalInterractionSecurity == true)
                    {
                        if (GameManager.Instance.isNear == true)
                        {
                            security = false;
                            GameManager.Instance.globalInterractionSecurity = false;
                            if (GameObject.FindGameObjectWithTag("NearInt") != null)
                            {
                                for (int i = 0; i <= GameObject.FindGameObjectsWithTag("NearInt").Length; i++)
                                {
                                    GameObject.FindGameObjectsWithTag("NearInt")[i].SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            security = false;
                            GameManager.Instance.globalInterractionSecurity = false;
                            if (GameObject.FindGameObjectWithTag("FarInt") != null)
                            {
                                for (int i = 0; i <= GameObject.FindGameObjectsWithTag("FarInt").Length; i++)
                                {
                                    GameObject.FindGameObjectsWithTag("FarInt")[i].SetActive(false);
                                }
                            }
                        }
                    }
                    UIManager.Instance.DisplayPortrait(portraitIdx);
                    StartCoroutine(FarNearInterraction());
                    security = true;
                    GameManager.Instance.globalInterractionSecurity = true;
                    return;
                }
            }
        }
    }



    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (cursorOn == true && gameObject.activeSelf == true)
            {

                /*interactionsTexts = FindObjectsOfType<TextMeshProUGUI>();
                for (int i = 0; i < interactionsTexts.Length; i++)
                {
                    if (interactionsTexts[i] != null)
                    {
                        interactionsTexts[i].text = null;
                    }
                }*/

                if (isNear == true)
                {
                    if (GameManager.Instance.globalInterractionSecurity == true)
                    {
                        if (GameManager.Instance.isNear == true)
                        {
                            security = false;
                            GameManager.Instance.globalInterractionSecurity = false;
                            if (GameObject.FindGameObjectWithTag("NearInt") != null)
                            {
                                for (int i = 0; i <= GameObject.FindGameObjectsWithTag("NearInt").Length; i++)
                                {
                                    GameObject.FindGameObjectsWithTag("NearInt")[i].SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            security = false;
                            GameManager.Instance.globalInterractionSecurity = false;
                            if (GameObject.FindGameObjectWithTag("FarInt") != null)
                            {
                                for (int i = 0; i <= GameObject.FindGameObjectsWithTag("FarInt").Length; i++)
                                {
                                    GameObject.FindGameObjectsWithTag("FarInt")[i].SetActive(false);
                                }
                            }
                        }
                    }
                    StartCoroutine(NearInterraction());
                    UIManager.Instance.HidePortraits();
                    if (spriteHighlight != null)
                    {
                        spriteHighlight.enabled = false;
                    }
                    FindObjectOfType<AudioManager>().Play("OnClickInventory");
                    playerInputs.currentActionMap.Disable();
                    //GameObject currentVfx = Instantiate(vfx, transform.position, transform.rotation);
                    //currentVfx.transform.parent = null;
                    //Destroy(currentVfx, 3f);
                    security = true;
                    GameManager.Instance.globalInterractionSecurity = true;

                    allBoxColliders = GameObject.FindGameObjectsWithTag("Interractable");
                    for (int i = 0; i < allBoxColliders.Length; i++)
                    {
                        if (allBoxColliders[i] != null && allBoxColliders[i].GetComponent<BoxCollider2D>() != null)
                        {
                            if (allBoxColliders[i].GetComponent<BoxCollider2D>().enabled == false)
                            {
                                allBoxCollier2DDisabled[allDisBoxColLength] = allBoxColliders[i].GetComponent<BoxCollider2D>();
                                allDisBoxColLength += 1;
                            }

                            if (allBoxColliders[i].GetComponent<BoxCollider2D>().enabled)
                            {
                                allBoxColliders[i].GetComponent<BoxCollider2D>().enabled = false;
                            }

                        }
                    }

                }
            }
        }
    }



    private void OnMouseEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Cursor Over");
        if (isNear == true)
        {
            if (spriteHighlight != null)
            {
                spriteHighlight.enabled = true;
            }
            UIManager.Instance.SetPopUpCursor();
            isInterractableOn = true;
            cursorOn = true;
            return;
        }
        if (isNear == false)
        {
            if (spriteHighlight != null)
            {
                spriteHighlight.enabled = true;
            }
            UIManager.Instance.SetFarCursor();
            cursorOn = true;
            return;
        }
    }

    private void OnMouseExit()
    {
        if (spriteHighlight != null)
        {
            spriteHighlight.enabled = false;
        }
        UIManager.Instance.ResetCursor();
        isInterractableOn = false;
        cursorOn = false;
    }

    private IEnumerator NearInterraction()
    {
        nearInt0.SetActive(true);
        yield return 0;
    }
    public void QuitInterraction()
    {
        nearInt0.SetActive(false);
        security = false;
        interractionSecurity = false;
        GameManager.Instance.globalInterractionSecurity = false;
        playerInputs.currentActionMap.Enable();
        
        // Destroy(this.gameObject);
    }
    private IEnumerator FarInterraction()
    {
        observationText.text = farPhrase;

        yield return new WaitForSeconds(5f);

        security = false;
        GameManager.Instance.globalInterractionSecurity = false;

        yield return 0;

    }
    private IEnumerator FarNearInterraction()
    {
        observationText.text = nearPhrase;

        yield return new WaitForSeconds(5f);

        security = false;
        GameManager.Instance.globalInterractionSecurity = false;

        yield return 0;

    }

    public void OnEnable()
    {
        if (playerInputs != null)
        {
            playerInputs.actions.FindAction("Look").started += OnLook;
            playerInputs.actions.FindAction("Use").started += OnUse;
        }
    }

    public void OnDisable()
    {
        if (playerInputs != null)
        {
            playerInputs.actions.FindAction("Look").started -= OnLook;
            playerInputs.actions.FindAction("Use").started -= OnUse;
        }
    }

    public void DeactivateAllInteractables()
    {
        for (int i = 0; i < allBoxColliders.Length; i++)
        {
            if (allBoxColliders[i] != null && allBoxColliders[i].GetComponent<BoxCollider2D>() != null)
            {
                allBoxColliders[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        for (int i = 0; i < allBoxCollier2DDisabled.Length; i++)
        {
            if (allBoxCollier2DDisabled[i] != null)
            {
                allBoxCollier2DDisabled[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        allDisBoxColLength = 0;
    }
}

