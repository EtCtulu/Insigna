using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    #region Singlton:Profile

    public static GameManager Instance;

    public string heartSfx = "event:/SFX/UI/Heartbeat";
    public FMOD.Studio.EventInstance heartbeatEvent;


    void Awake()
    {
        heartbeatEvent = FMODUnity.RuntimeManager.CreateInstance(heartSfx);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        playerInput = new PlayerInputs();

        madnessZone = GameObject.FindGameObjectWithTag("MadnessZone");
        sanityZone = GameObject.FindGameObjectWithTag("SanityZone");
    }
    #endregion

    [Header("Player Stats")]
    public float playerMadness;
    public int playerSanity;
    public int playerPillsCount;
    public int healAmmount = 30;
    public int playerMadnessDecrement = 2;
    public PlayerInputs playerInput;
    public bool playerInVent;

    public RuntimeAnimatorController currentPlayerAnimation;

    [Space(5)]
    [Header("Helmet")]
    public bool isHelmetEquipped = false;
    public bool canEquipHelmet = false;
    public bool hasHelmetEquipped = false;

    [Space(5)]
    [Header("Status Check")]
    public bool isScared;
    [HideInInspector] public bool hasInteracted = false;

    [HideInInspector] public Coroutine hidePortaitCoroutine;
    /*[HideInInspector]*/
    public GameObject near_Text;
    [HideInInspector] public Coroutine near_TextCoroutine;
    [HideInInspector] public GameObject far_Text;
    [HideInInspector] public Coroutine far_TextCoroutine;


    [Space(5)]
    [Header("MadnessZone")]
    public GameObject madnessZone;
    public GameObject sanityZone;

    public GameObject madnessZoneInterractions;
    [HideInInspector]
    public BoxCollider2D[] madnessInterractionsBC2D;
    [HideInInspector]
    public SpriteRenderer[] madnessInterractionsSprRend;
    public GameObject sanityZoneInterractions;
    [HideInInspector]
    public BoxCollider2D[] sanityInterractionsBC2D;
    [HideInInspector]
    public SpriteRenderer[] sanityInterractionsSprRend;
    public bool dimensionSwapNormal;
    public bool dimensionSwapMadness;

    public MadnessAppear indicible;
    public bool once;

    public bool globalInterractionSecurity = false;
    public bool isNear = false;
    public GameObject player;

    public bool N03T02energy;

    public Volume v_Neutre;
    public Volume v_Transition;
    public Volume v_Indicible;

    public float timer = 0f;
    public float timerdeux = 0f;

    public bool WarningBool;
    public float warningtime;


    #region Madness Functions

    private void Start()
    {
        StartCoroutine("SanityDecrement");
        playerPillsCount = 0;
        dimensionSwapNormal = true;
        hasInteracted = false;
    }

    public void Update()
    {

        if(canEquipHelmet == true && WarningBool == false)
        {
            warningtime += Time.deltaTime;
        }
        if(warningtime >= 120f)
        {
            UIManager.Instance.DisplayWarning();
            warningtime = 0f;
        }

        if (isHelmetEquipped == true)
        {
            timer += Time.deltaTime;
            // Debug.Log(timer);
            /*if(timer >= 0f)
            {*/
                timerdeux += Time.deltaTime;
                if (timerdeux >= 2f)
                {
                    timerdeux = 0;
                    playerSanity = Mathf.Clamp(playerSanity, 0, 100);
                    playerSanity = playerSanity + 3;
                    playerSanity = Mathf.Clamp(playerSanity, 0, 100);
                if (playerSanity >= 100)
                {
                    DeactivateInGameActions();
                    heartbeatEvent.setParameterByName("Stress", 0);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Corruption", 0);
                    MenusManager.instance.GameOver();
                }
            }
        }
        if (isHelmetEquipped == false)
        {
            timer = 0;
        }

        if (player != null)
        {
            currentPlayerAnimation = player.GetComponent<Animator>().runtimeAnimatorController;
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (dimensionSwapMadness == false)
        {
            // Tu mets le pp folie et enl�ve le normal
            //ok
            if (once == true)
            {
                if (v_Indicible != null && v_Neutre != null && v_Transition != null)
                {
                    v_Indicible.weight = 1;
                    v_Neutre.weight = 0;
                    v_Transition.weight = 0;
                }

            }

        }
        else
        {
            // Tu mets le pp normal et enl�ve le folie tu relies la variable de pp transition a la barre de folie
            //non 
            if (v_Neutre != null)
            {
                v_Neutre.weight = 1;
            }
            if (v_Indicible != null)
            {
                v_Indicible.weight = 0;
            }
            if (isScared == true)
            {
                if (v_Transition != null)
                {
                    v_Transition.weight = playerMadness / 100f;
                }
            }
            else
            {
                if (v_Transition != null)
                {
                    v_Transition.weight = 0;
                }
            }

        }



    }




    public IEnumerator InsideMadnessZone(int sanityDmg, int madnessGain)
    {

        if (isScared == true)
        {
            heartbeatEvent.setParameterByName("Stress", playerSanity);
            
            UIManager.Instance.madnessFill.transform.parent.GetComponent<Animator>().SetBool("Filling", true);

            /*if (isHelmetEquipped == true)
            {
                playerSanity = Mathf.Clamp(playerSanity, 0, 100);
                playerSanity++;
                playerSanity = Mathf.Clamp(playerSanity, 0, 100);
            }*/

            if (isHelmetEquipped == false)
            {
                playerSanity = Mathf.Clamp(playerSanity, 0, 100);
                playerSanity = playerSanity + sanityDmg;
                if (playerSanity >= 100)
                {
                    DeactivateInGameActions();
                    heartbeatEvent.setParameterByName("Stress", 0);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Corruption", 0);
                    MenusManager.instance.GameOver();
                }
                playerSanity = Mathf.Clamp(playerSanity, 0, 100);
                playerMadness = Mathf.Clamp(playerMadness, 0, 100);
                playerMadness = playerMadness + madnessGain;
                if (playerMadness >= 80 && dimensionSwapMadness == true)
                {
                    WarningBool = true;
                    warningtime = 0f;
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Corruption", 1);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Trigger Corrupt", 1);
                    heartbeatEvent.start();

                    indicible.SetAppear();

                    madnessZone.SetActive(true);
                    for (int i = 0; i < madnessInterractionsBC2D.Length; i++)
                    {
                        if (madnessInterractionsBC2D[i] == null)
                        {
                            yield return 0;
                        }
                        else
                        {
                            madnessInterractionsBC2D[i].enabled = true;
                        }
                    }
                    for (int i = 0; i < madnessInterractionsSprRend.Length; i++)
                    {
                        if (madnessInterractionsSprRend[i] == null)
                        {
                            yield return 0;
                        }
                        else
                        {
                            madnessInterractionsSprRend[i].enabled = true;
                        }
                    }

                    sanityZone.SetActive(false);
                    for (int i = 0; i < sanityInterractionsBC2D.Length; i++)
                    {
                        if (sanityInterractionsBC2D[i] == null)
                        {
                            yield return 0;
                        }
                        else
                        {
                            sanityInterractionsBC2D[i].enabled = false;
                        }
                    }
                    for (int i = 0; i < sanityInterractionsSprRend.Length; i++)
                    {
                        if (sanityInterractionsSprRend[i] == null)
                        {
                            yield return 0;
                        }
                        else
                        {
                            sanityInterractionsSprRend[i].enabled = false;
                        }
                    }

                    globalInterractionSecurity = false;
                    dimensionSwapMadness = false;
                    dimensionSwapNormal = true;


                }
                playerMadness = Mathf.Clamp(playerMadness, 0, 100);

            }

            yield return new WaitForSeconds(0.5f);
            if (isScared == true)
            {
                StartCoroutine(InsideMadnessZone(sanityDmg, madnessGain));
            }
        }

        yield return 0;
    }

    public IEnumerator SanityDecrement()
    {
        if (isScared == false)
        {
            heartbeatEvent.setParameterByName("Stress", playerSanity);

            UIManager.Instance.madnessFill.transform.parent.GetComponent<Animator>().SetBool("Filling", false);

            if (isHelmetEquipped == false)
            {

                playerMadness = Mathf.Clamp(playerMadness, 0, 100);
                playerMadness = playerMadness - playerMadnessDecrement;
                if (playerMadness <= 50 && dimensionSwapNormal == true)
                {
                    if (once == true)
                    {
                        WarningBool = false;
                        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Corruption", 0);
                        heartbeatEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                        indicible.SetDisAppear();
                    }
                    if (once == false)
                    {
                        once = true;
                    }

                    if (madnessZone != null && sanityZone != null)
                    {
                        madnessZone.SetActive(false);
                        for (int i = 0; i < madnessInterractionsBC2D.Length; i++)
                        {
                            if (madnessInterractionsBC2D[i] == null)
                            {
                                yield return 0;
                            }
                            else
                            {
                                madnessInterractionsBC2D[i].enabled = false;
                            }
                        }
                        for (int i = 0; i < madnessInterractionsSprRend.Length; i++)
                        {
                            if (madnessInterractionsSprRend[i] == null)
                            {
                                yield return 0;
                            }
                            else
                            {
                                madnessInterractionsSprRend[i].enabled = false;
                            }
                        }

                        sanityZone.SetActive(true);
                        for (int i = 0; i < sanityInterractionsBC2D.Length; i++)
                        {
                            if (sanityInterractionsBC2D[i] == null)
                            {
                                yield return 0;
                            }
                            else
                            {
                                sanityInterractionsBC2D[i].enabled = true;
                            }
                        }
                        for (int i = 0; i < sanityInterractionsSprRend.Length; i++)
                        {
                            if (sanityInterractionsSprRend[i] == null)
                            {
                                yield return 0;
                            }
                            else
                            {
                                sanityInterractionsSprRend[i].enabled = true;
                            }
                        }
                    }
                    globalInterractionSecurity = false;
                    dimensionSwapNormal = false;
                    dimensionSwapMadness = true;

                }
                playerMadness = Mathf.Clamp(playerMadness, 0, 100);
            }


            yield return new WaitForSeconds(0.5f);
            if (isScared == false)
            {
                StartCoroutine("SanityDecrement");
            }
        }

        yield return 0;
    }



    #endregion

    #region HP Fuctions


    public void GetHPBack()
    {
        playerSanity = Mathf.Clamp(playerSanity, 0, 100);
        playerSanity = playerSanity - healAmmount;
        playerSanity = Mathf.Clamp(playerSanity, 0, 100);
        playerPillsCount--;
    }

    public void TakePills()
    {
        if (GameManager.Instance.isHelmetEquipped == false && GameManager.Instance.isScared == false && GameManager.Instance.playerPillsCount != 0)
        {
            GameManager.Instance.GetHPBack();
            FindObjectOfType<AudioManager>().Play("Pills");
            return;
        }
    }

    #endregion

    public void ActivateInGameActions()
    {
        player.GetComponent<PlayerInput>().enabled = true;
        //playerInput.Gameplay.Enable();
    }
    public void DeactivateInGameActions()
    {
        player.GetComponent<PlayerInput>().enabled = false;
        //playerInput.Gameplay.Disable();
    }

    public void ForceMadness()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Corruption", 1);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Trigger Corrupt", 1);
        heartbeatEvent.start();

        indicible.SetAppear();

        madnessZone.SetActive(true);
        for (int i = 0; i < madnessInterractionsBC2D.Length; i++)
        {
            if (madnessInterractionsBC2D[i] == null)
            {
                return;
            }
            else
            {
                madnessInterractionsBC2D[i].enabled = true;
            }
        }
        for (int i = 0; i < madnessInterractionsSprRend.Length; i++)
        {
            if (madnessInterractionsSprRend[i] == null)
            {
                return;
            }
            else
            {
                madnessInterractionsSprRend[i].enabled = true;
            }
        }

        sanityZone.SetActive(false);
        for (int i = 0; i < sanityInterractionsBC2D.Length; i++)
        {
            if (sanityInterractionsBC2D[i] == null)
            {
                return;
            }
            else
            {
                sanityInterractionsBC2D[i].enabled = false;
            }
        }
        for (int i = 0; i < sanityInterractionsSprRend.Length; i++)
        {
            if (sanityInterractionsSprRend[i] == null)
            {
                return;
            }
            else
            {
                sanityInterractionsSprRend[i].enabled = false;
            }
        }

        globalInterractionSecurity = false;
        dimensionSwapMadness = false;
        dimensionSwapNormal = true;
    }

    public IEnumerator HidePortraitAndOthers()
    {
        yield return new WaitForSeconds(5f);
        hasInteracted = false;
        UIManager.Instance.HidePortraits();
    }

    public IEnumerator nearText()
    {
        near_Text.SetActive(true);
        yield return new WaitForSeconds(5f);

        if (near_Text != null)
        {
            near_Text.SetActive(false);
        }
    }

    public IEnumerator farText()
    {
        far_Text.SetActive(true);
        yield return new WaitForSeconds(5f);

        if (far_Text != null)
        {
            far_Text.SetActive(false);
        }
    }

    public void StartHidePortaitFonction()
    {
        hidePortaitCoroutine = StartCoroutine(HidePortraitAndOthers());
    }

    public void StopHidePortaitFonction()
    {
        if (hidePortaitCoroutine != null)
        {
            StopCoroutine(hidePortaitCoroutine);
        }
    }

    public void StartNearTextFonction()
    {
        near_TextCoroutine = StartCoroutine(nearText());
    }

    public void StopNearTextFonction()
    {
        if (near_TextCoroutine != null)
        {
            StopCoroutine(near_TextCoroutine);
        }
    }

    public void StartFarTextFonction()
    {
        far_TextCoroutine = StartCoroutine(farText());
    }

    public void StopFarTextFonction()
    {
        if (far_TextCoroutine != null)
        {
            StopCoroutine(far_TextCoroutine);
        }
    }

}
