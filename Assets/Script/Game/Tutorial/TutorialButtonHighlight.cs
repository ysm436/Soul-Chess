using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonHighlight : MonoBehaviour
{

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private bool isVisible = false;

    WaitForSeconds Wait = new WaitForSeconds(0.5f);

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = isVisible = false;
        StartCoroutine(EnableHighlight());
    }

    public IEnumerator EnableHighlight()
    {
        while (true)
        {
            if (!isVisible) spriteRenderer.enabled = isVisible = true;
            else spriteRenderer.enabled = isVisible = false;

            yield return Wait;
        }
    }
}
