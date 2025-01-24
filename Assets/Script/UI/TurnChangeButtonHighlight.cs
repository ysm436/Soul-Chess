using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnChangeButtonHighlight : MonoBehaviour
{
    private PlayerController player;
    public Material highlightMaterial;
    private bool isVisible = false;

    WaitForSeconds Wait = new WaitForSeconds(0.5f);

    void Start()
    {
        highlightMaterial = GetComponent<Renderer>().material;
        isVisible = false;
        player = GameBoard.instance.myController;

        highlightMaterial.SetFloat("_InnerOutlineAlpha", 0f);

        if (player.playerColor == GameBoard.PlayerColor.Black)
        {   
            highlightMaterial.SetColor("_InnerOutlineColor", new Color(1, 1, 1, 1));
            highlightMaterial.SetFloat("_InnerOutlineGlow", 2.5f);
        }
        else
        {
            highlightMaterial.SetColor("_InnerOutlineColor", new Color(1, 1, 0, 1));
            highlightMaterial.SetFloat("_InnerOutlineGlow", 1f);
        }

        StartCoroutine(EnableHighlight());
    }

    public IEnumerator EnableHighlight()
    {
        while (true)
        {
            if (!isVisible && player.TurnEndPossible && GameBoard.instance.isActivePlayer) 
            {
                highlightMaterial.SetFloat("_InnerOutlineAlpha", 1f);
                isVisible = true;
            }
            else
            {
                highlightMaterial.SetFloat("_InnerOutlineAlpha", 0f);
                isVisible = false;
            }

            yield return Wait;
        }
    }
}
