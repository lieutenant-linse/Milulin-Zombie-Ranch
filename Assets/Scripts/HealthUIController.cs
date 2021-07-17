using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIController : MonoBehaviour
{

    public GameObject heartContainer;
    private float fillValue; //for the Heart UI Container

    // Update is called once per frame
    void Update()
    {
        //univerally applicable Health Controller
        fillValue = (float)GameController.Health;
        fillValue = fillValue / GameController.MaxHealth; //curently: 3/3 - Means one hit equal to one heart
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
