using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    /*
    int credits; // ammount of money owned by the player
    private int damageCost;
    private int damageBonus;

    private int fRateCost;
    private int fRateBonus;

    private int healthCost;
    private int healthBonus;


    GameObject ameliaComment;

    private void Start()
    {
        ameliaComment = GameObject.Find("Canvas").GetComponentInChildren<GameObject>();

        damageBonus = 0;
        fRateBonus = 0;
        healthBonus = 0;

        damageCost = 5;
        fRateCost = 5;
        healthCost = 5;

        GameObject.Find("Canvas").GetComponentInChildren<TextMeshPro>().SetText(damageCost.ToString());
        GameObject.Find("Canvas").GetComponentInChildren<TextMeshPro>().SetText(fRateCost.ToString());
        GameObject.Find("Canvas").GetComponentInChildren<TextMeshPro>().SetText(healthCost.ToString());

    }

    int CoinCount()
    {
        return credits = int.Parse(GameObject.Find("CoinCounter").GetComponent<TextMeshPro>().ToString());
    }

    public void purchaseDamage()
    {
        // if the player has enough money
        if (damageCost <= CoinCount()) 
        {
            // substract cost from money
            credits -= damageCost;
            // Amelia react message
            AcceptPurchase();
            // add 5$ to the cost of the buff
            damageCost += 5;
            GameObject.Find("Canvas").GetComponentInChildren<TextMeshPro>().SetText(damageCost.ToString());
            // Add actual buff
            BuffDamage(1);
        }
        else 
        {
            DenyPurchase();
        }
    }

    private void BuffDamage(int buffMarker)
    {
        damageBonus += buffMarker;
    }

    public int GetDamageBuff()
    {
        return damageBonus;
    }

    public void purchaseFireRate()
    {
        // if the player has enough money
        if (fRateCost <= CoinCount())
        {
            // substract cost from money
            credits -= fRateCost;
            // Amelia react message
            AcceptPurchase();
            // add 5$ to the cost of the buff
            fRateCost += 5;
            GameObject.Find("Canvas").GetComponentInChildren<TextMeshPro>().SetText(fRateCost.ToString());
            // Add actual buff
            BuffFireRate(1);
        }
        else
        {
            DenyPurchase();
        }
    }

    private void BuffFireRate(int buffMarker)
    {
        fRateBonus += buffMarker;
    }

    public int GetFireRateBuff()
    {
        return fRateBonus;
    }

    public void purchaseHealth()
    {
        // if the player has enough money
        if (healthCost <= CoinCount())
        {
            // substract cost from money
            credits -= healthCost;
            // Amelia react message
            AcceptPurchase();
            // add 5$ to the cost of the buff
            healthCost += 5;
            GameObject.Find("Canvas").GetComponentInChildren<TextMeshPro>().SetText(healthCost.ToString());
            // Add actual buff
            BuffHealth(1);
        }
        else
        {
            DenyPurchase();
        }
    }

    private void BuffHealth(int buffMarker)
    {
        healthBonus += buffMarker;
    }

    public int GetHealthBuff()
    {
        return healthBonus;
    }


    public void AcceptPurchase()
    {
        ameliaComment.gameObject.GetComponent<TextMeshPro>().SetText("Great choice !\n\nThank you for your business !");
    }
    public void DenyPurchase()
    {
        ameliaComment.gameObject.GetComponent<TextMeshPro>().SetText("Sorry pal...\n\nYou need more credits for that !");
    }
    */
}
