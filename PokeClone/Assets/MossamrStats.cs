using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MossamrStats : MonoBehaviour
{
    public string primaryType = "Grass";
    public string secondaryType = "Steel";

    public int MaxHealth = 87;
    public double currentHealth;

    public int maxSpeed = 87;
    public int currentSpeed;

    public int maxAttack = 87;
    public int currentAttack;

    public int maxSpecialAttack = 87;
    public int currentSpecialAttack;

    public int maxDefense = 87;
    public int currentDefense;

    public int maxSpecialDefense = 87;
    public int currentSpecialDefense;

    public Move grass;
    public Move steel;

    private HashSet<string> weakness = new HashSet<string>();
    private HashSet<string> resistance = new HashSet<string>();
    private HashSet<string> immunity = new HashSet<string>();

    public MossamrStats()
    {
        currentHealth = MaxHealth;
        currentSpeed = maxSpeed;
        currentAttack = maxAttack;
        currentSpecialAttack = maxSpecialAttack;
        currentDefense = maxDefense;
        currentSpecialDefense = maxSpecialDefense;
           
        grass = new Move(80, "Attack", "grass");
        steel = new Move(80, "Attack", "steel");
    }

    public double damageDone(VolthesisStats volthesis, string typeBeingUsed)
    {
        //Damage = ((((2 * Level / 5 + 2) * AttackStat * AttackPower / DefenseStat) / 50) + 2) * STAB * Weakness/Resistance * RandomNumber / 100
        //Level will just be 50 before a level up system is added.

        double effective = effectiveness(typeBeingUsed);

        int attackPower;
        if (grass.getType().Equals(typeBeingUsed))
        {
            attackPower = grass.getBasePower();
        }
        else
        {
            attackPower = steel.getBasePower();
        }

        int attackStat = getSpecialAttack(); // will have to use the move.getAttackStatBeingUsed
        int defenseStat = volthesis.getSpecialDefense();
        double randomNum = Random.Range(85, 101);
        randomNum /= 100;

        Debug.Log("Stats: " + attackStat + " " + attackPower + " " + defenseStat + " " + effective + " " + randomNum);

        double damage = ((((2 * 50 / 5 + 2) * attackStat * attackPower / defenseStat) / 50) + 2) * 1.5 * effective * randomNum / 100;
        return damage;
    }

    public double effectiveness(string type)
    {
        int numerator = 1;
        int denominator = 1;
        int stringReturnVal = 0;

        if (immunity.Contains(type))
        {
            return 0;
        }

        foreach (string weaknessType in weakness)
        {
            stringReturnVal = System.String.Compare(type, weaknessType, System.StringComparison.CurrentCultureIgnoreCase);
            if (stringReturnVal == 0)
            {
                numerator = 2;

                if (System.String.Compare("2", weaknessType, System.StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    numerator = 4;
                }
            }
        }

        foreach (string resistanceType in resistance)
        {
            stringReturnVal = System.String.Compare(type, resistanceType, System.StringComparison.CurrentCultureIgnoreCase);
            if (stringReturnVal == 0)
            {
                denominator = 2;
                if (System.String.Compare("2", resistanceType, System.StringComparison.CurrentCultureIgnoreCase) > -1)
                {
                    denominator = 4;
                }
            }
        }

        double returnVal = numerator;
        returnVal /= denominator;

        return returnVal;
    }

    public void takeDamage(double damage, VolthesisStats volthesis)
    {
        currentHealth -= damage;
        Debug.Log("Mossamr took " + damage + " hitpoints of damage.");
        Debug.Log("Mossamr current health " + currentHealth + ".");

        if (currentHealth <= 0 && volthesis.getHealth() != 0)
        {
            die();
        }
    }

    public void die()
    {
        SceneManager.LoadScene("Route 1");
    }

    public double getHealth()
    {
        return currentHealth;
    }

    public int getSpeed()
    {
        return currentSpeed;
    }

    public int getAttack()
    {
        return currentAttack;
    }

    public int getSpecialAttack()
    {
        return currentSpecialAttack;
    }

    public int getDefense()
    {
        return currentDefense;
    }

    public int getSpecialDefense()
    {
        return currentSpecialDefense;
    }

    public string getPrimaryType()
    {
        return primaryType;
    }

    public string getSecondaryType()
    {
        return secondaryType;
    }

    public void addWeakness()
    {
        weakness.Add("fire"); //4x weakness
        weakness.Add("fighting");
    }

    public void addResistance()
    {
        resistance.Add("normal");
        resistance.Add("grass"); // 4x weakness
        resistance.Add("water");
        resistance.Add("electric");
        resistance.Add("psychic");
        resistance.Add("rock");
        resistance.Add("dragon");
        resistance.Add("steel");
        resistance.Add("fairy");
    }

    public void addImmunity()
    {
        immunity.Add("poison");
    }
}
