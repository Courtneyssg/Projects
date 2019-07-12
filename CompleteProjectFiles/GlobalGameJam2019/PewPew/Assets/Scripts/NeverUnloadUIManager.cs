using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeverUnloadUIManager : MonoBehaviour
{
    public Image healthBar;
    public Health healthAmount;
    public Image FuelBar;
    public Movement1 fuel;

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthAmount.health/100;

        FuelBar.fillAmount = fuel.fuelLevel / fuel.fuelLimit;
        
        
    }
}
 