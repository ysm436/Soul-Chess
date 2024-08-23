using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    public Image loadingImage;
    public int loadingSpeed;
    private void Awake()
    {

    }
    private void Update()
    {
        if (loadingImage.fillClockwise)
        {
            loadingImage.fillAmount += loadingSpeed * Time.deltaTime;
            if (loadingImage.fillAmount >= 1)
            {
                loadingImage.fillClockwise = false;
            }
        }
        else
        {
            loadingImage.fillAmount -= loadingSpeed * Time.deltaTime;
            if (loadingImage.fillAmount <= 0)
            {
                loadingImage.fillClockwise = true;
            }
        }

    }
}
