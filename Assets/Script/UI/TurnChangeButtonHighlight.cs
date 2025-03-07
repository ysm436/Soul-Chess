using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnChangeButtonHighlight : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] float buttonAlpha = 0.4f;
    public Material highlightMaterial;
    public TextMeshPro buttonText;

    private bool isVisible = false;
    WaitForSeconds Wait = new WaitForSeconds(0.7f);

    public bool tutorialFlag;

    void Start()
    {
        highlightMaterial = GetComponent<Renderer>().material;
        isVisible = false;
        player = GameBoard.instance.myController;
        highlightMaterial.SetFloat("_OutlineAlpha", 0f);

        if (SceneManager.GetActiveScene().name == "TutorialScene")
            StartCoroutine(EnableHighlightForTutorial());
        else
            StartCoroutine(EnableHighlight());
    }

    public IEnumerator EnableHighlight()
    {
        while (true)
        {
            if (player.TurnEndPossible && GameBoard.instance.isActivePlayer) 
            {
                highlightMaterial.SetFloat("_OutlineAlpha", 1f);
                buttonText.text = "턴 종료";

                if (GameBoard.instance.gameData.myPlayerData.CheckAllCardUnAvailable())
                {
                    if (isVisible)
                    {
                        highlightMaterial.SetFloat("_Alpha", buttonAlpha);
                        highlightMaterial.SetFloat("_OutlineAlpha", buttonAlpha);
                        buttonText.alpha = buttonAlpha;
                        isVisible = false;
                    }
                    else
                    {
                        highlightMaterial.SetFloat("_Alpha", 1f);
                        highlightMaterial.SetFloat("_OutlineAlpha", 1f);
                        buttonText.alpha = 1f;
                        isVisible = true;
                    }
                }
            }
            yield return Wait;
        }
    }

    public void DisableHighlight()
    {
        highlightMaterial.SetFloat("_Alpha", 1f);
        buttonText.alpha = 1f;
        highlightMaterial.SetFloat("_OutlineAlpha", 0f);
    }

    public IEnumerator EnableHighlightForTutorial()
    {
        while (true)
        {
            if (tutorialFlag) 
            {
                highlightMaterial.SetFloat("_OutlineAlpha", 1f);
                buttonText.text = "턴 종료";

                if (isVisible)
                {
                    highlightMaterial.SetFloat("_Alpha", buttonAlpha);
                    highlightMaterial.SetFloat("_OutlineAlpha", buttonAlpha);
                    buttonText.alpha = buttonAlpha;
                    isVisible = false;
                }
                else
                {
                    highlightMaterial.SetFloat("_Alpha", 1f);
                    highlightMaterial.SetFloat("_OutlineAlpha", 1f);
                    buttonText.alpha = 1f;
                    isVisible = true;
                }
            }
            yield return Wait;
        }
    }

    public void DisableHighlightForTutorial()
    {
        highlightMaterial.SetFloat("_Alpha", 1f);
        buttonText.alpha = 1f;
        highlightMaterial.SetFloat("_OutlineAlpha", 0f);
        tutorialFlag = false;
    }
}
