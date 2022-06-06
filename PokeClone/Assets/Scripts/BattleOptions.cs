using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // to change scenes
using UnityEngine.UI; // to change the ui elements in unity

// battle options class that deals with all of the options that can be done during a battle
public class BattleOptions : MonoBehaviour
{
    // make not of previous route
    string previousRoute;

    // Make new pokemons
    PokemonStats mossamr;
    PokemonStats volthesis;
    PokemonStats wargo;

    // make the sets for the constructor for all of the pokemon
    HashSet<string> weaknessMossamr = new HashSet<string>();
    HashSet<string> resistanceMossamr = new HashSet<string>();
    HashSet<string> immunityMossamr = new HashSet<string>();

    HashSet<string> weaknessVolthesis = new HashSet<string>();
    HashSet<string> resistanceVolthesis = new HashSet<string>();
    HashSet<string> immunityVolthesis = new HashSet<string>();

    HashSet<string> weaknessWargo = new HashSet<string>();
    HashSet<string> resistanceWargo = new HashSet<string>();

    // when the script is first run fill in all of the information to do with the pokemon
    public void Awake()
    {
        PokemonStats enemyPokemon = PokemonParty.getEnemyPokemon();

        GameObject Canvas = GameObject.FindWithTag("Canvas");

        Image pokemonImage = Canvas.transform.Find("EnemyPokemon").GetComponent<Image>(); // gets the image name
        Text pokemonName = Canvas.transform.Find("EnemyName").GetComponent<Text>(); // gets the text
        Text totalHealth = Canvas.transform.Find("EnemyTotalHealth").GetComponent<Text>(); // gets the text
        Text currentHealth = Canvas.transform.Find("EnemyCurrentHealth").GetComponent<Text>(); // gets the text

        pokemonImage.sprite = Resources.Load<Sprite>(enemyPokemon.getName()); // changes image
        pokemonName.text = enemyPokemon.getName(); // changes text
        currentHealth.text = enemyPokemon.getHealth() + "";
        totalHealth.text = enemyPokemon.maxHealth() + "";
        previousRoute = PokemonParty.getPrevRoute();
    }

    // copnctructor to inuitalize all of the fields
    public BattleOptions()
    {
        // methods to add all of the information to the sets without clogging the constrcutor
        addWeaknessMossamr();
        addWeaknessVolthesis();
        addWeaknessWargo();

        addResistanceWargo();
        addResistanceVolthesis();
        addResistanceMossamr();

        addImmunityVolthesis();
        addImmunityMossamr();

        // make the pokemon
        //public PokemonStats(string name, string primary, string secondary, int health, int speed, int attack, int spAttack, int defense, int spDefense, HashSet<string> weaknessP, HashSet<string> resistanceP, HashSet<string> immunityP)
        mossamr = new PokemonStats("Mossamr", "grass", "steel", 87, 87, 87, 87, 87, 87, weaknessMossamr, resistanceMossamr, immunityMossamr);
        volthesis = new PokemonStats("Volthesis", "fire", "fairy", 87, 87, 87, 87, 87, 87, weaknessVolthesis, resistanceVolthesis, immunityVolthesis);
        wargo = new PokemonStats("Wargo", "water", "dragon", 87, 87, 87, 87, 87, 87, weaknessWargo, resistanceWargo, null);
    }

    // changes to the previous route if the player ran away
    public void runAway()
    {
        SceneManager.LoadScene(previousRoute);
    }

    // goes into battle and depending on the number sent then that is the move done by the pokemon
    public void battle(int n)
    {
        // initializes all of the variables that will need to be used
        //string stat;

        PokemonStats enemyPokemon = PokemonParty.getEnemyPokemon();

        PokemonStats[] party = PokemonParty.getParty();
        PokemonStats currentPokemon = party[0];

        // as long as the pokemon is still alive
        if (currentPokemon.getHealth() > 1)
        {

            double enemyDamage;
            double currentDamage;
            double enemyFrac;
            double currentFrac;

            // makes the effectiveness fractions for volthesis and gets the damage done for both different type attacks
            currentFrac = currentPokemon.effectiveness(enemyPokemon.getPrimaryType());
            double enemyDamagePrimary = enemyPokemon.damageDone(currentPokemon, enemyPokemon.getPrimaryType(), currentFrac);
            currentFrac = currentPokemon.effectiveness(enemyPokemon.getSecondaryType());
            double enemyDamageSecondary = enemyPokemon.damageDone(volthesis, enemyPokemon.getSecondaryType(), currentFrac);

            // the better dame is done to the players pokemon
            if (enemyDamagePrimary > enemyDamageSecondary)
            {
                enemyDamage = enemyDamagePrimary;
            }
            else
            {
                enemyDamage = enemyDamageSecondary;
            }

            if (n == 1)
            {
                // if int n is a 1 then we are using the primary stat
                enemyFrac = enemyPokemon.effectiveness(currentPokemon.getPrimaryType());
                currentDamage = currentPokemon.damageDone(enemyPokemon, currentPokemon.getPrimaryType(), enemyFrac);
            }
            else
            {
                // if int n is a 2 then we are using the secondary stat
                enemyFrac = enemyPokemon.effectiveness(currentPokemon.getSecondaryType());
                currentDamage = currentPokemon.damageDone(enemyPokemon, currentPokemon.getSecondaryType(), enemyFrac);
            }

            // whichever pokemon is faster attacks first
            if (currentPokemon.getSpeed() > enemyPokemon.getSpeed())
            {
                enemyPokemon.takeDamageEnemy(currentDamage, currentPokemon);
                currentPokemon.takeDamage(enemyDamage, enemyPokemon);
            }
            else
            {
                currentPokemon.takeDamage(enemyDamage, enemyPokemon);
                enemyPokemon.takeDamageEnemy(currentDamage, currentPokemon);
            }
        }
    }

    // if the player wants to battle then change the butons
    public void moveSelection()
    {
        GameObject[] selectionButtons = GameObject.FindGameObjectsWithTag("Button");

        foreach (GameObject button in selectionButtons)
        {
            button.SetActive(false);
        }

        // activate battling buttons
        GameObject battleButton = GameObject.Find("Canvas/BattleOptions/SecondaryButton");
        battleButton.SetActive(true);
        
        battleButton = GameObject.Find("Canvas/BattleOptions/PrimaryButton");
        battleButton.SetActive(true);
        
        battleButton = GameObject.Find("Canvas/BattleOptions/SwitchButton");
        battleButton.SetActive(true);
    }

    // method to add mossamrs weakness'
    public void addWeaknessMossamr()
    {
        weaknessMossamr.Add("fire 2"); //4x weakness
        weaknessMossamr.Add("fighting");
    }

    // method to add mossamrs resistances
    public void addResistanceMossamr()
    {
        resistanceMossamr.Add("normal");
        resistanceMossamr.Add("grass 2"); // 4x weakness
        resistanceMossamr.Add("water");
        resistanceMossamr.Add("electric");
        resistanceMossamr.Add("psychic");
        resistanceMossamr.Add("rock");
        resistanceMossamr.Add("dragon");
        resistanceMossamr.Add("steel");
        resistanceMossamr.Add("fairy");
    }

    // method to add mossamrs immunities
    public void addImmunityMossamr()
    {
        immunityMossamr.Add("poison");
    }

    // method to add volthesis wekaness'
    public void addWeaknessVolthesis()
    {
        weaknessVolthesis.Add("water");
        weaknessVolthesis.Add("ground");
        weaknessVolthesis.Add("poison");
        weaknessVolthesis.Add("rock");
    }

    // method to add volthesis resistance's
    public void addResistanceVolthesis()
    {
        resistanceVolthesis.Add("fire");
        resistanceVolthesis.Add("grass");
        resistanceVolthesis.Add("ice");
        resistanceVolthesis.Add("fighting");
        resistanceVolthesis.Add("fairy");
        resistanceVolthesis.Add("bug 2"); // 4x resistance
        resistanceVolthesis.Add("dark");
    }

    // method to add voltheis immunities
    public void addImmunityVolthesis()
    {
        immunityVolthesis.Add("dragon");
    }

    // method to add wargos weakness'
    public void addWeaknessWargo()
    {
        weaknessWargo.Add("fairy"); 
        weaknessWargo.Add("dragon");
    }

    // method to add wargos resistance
    public void addResistanceWargo()
    {
        resistanceWargo.Add("fire 2");
        resistanceWargo.Add("water 2");
        resistanceWargo.Add("steel");
    }

    // method to catch pokemon with the name of the pokemon given as the parameter
    public void catchPokemon(Text name) 
    {
        // if the party is too full then can't add a pokemon
        PokemonStats[] pokemonParty = PokemonParty.getParty();
        if (pokemonParty.Length > 6)
        {
            Debug.Log("Party is too full");
        }
        // adds the pokemon to the party
        string nameOfPokemon = name.text;
        if (nameOfPokemon.Equals("Wargo"))
        {
            PokemonParty.add(wargo);
        } else
        {
            PokemonParty.add(mossamr);
        }
        runAway(); // goes back to the route because youa are done catching
    }

    // activates the panel to see the switch menu and then fill in all of the pokemon information
    public void showPokmeon(GameObject SwitchMenu)
    {
        SwitchMenu.SetActive(true);
        fillInValues(PokemonParty.getParty(), SwitchMenu);
    }

    // Very similiar method as the one in view party. and disables the button for pokemon that are not in your party
    void fillInValues(PokemonStats[] pokemonArray, GameObject SwitchMenu)
    {
        string imageName;
        string textName;
        string buttonName;
        //string buttonName;
        for (int i = 0; i < 6; i++)
        {
            imageName = "Image" + i;
            textName = "Text" + i;
            buttonName = "Button" + i;
            Image pokemonImage = SwitchMenu.transform.Find(imageName).GetComponent<Image>(); // gets the image name
            Text pokemonName = SwitchMenu.transform.Find(textName).GetComponent<Text>(); // gets the text
            GameObject button = GameObject.Find("SwitchMenu/Panel/" + buttonName);

            // checks if the pokemon is not null and now dead, then allows the button to be pressed.
            if (pokemonArray[i] != null)
            {
                PokemonStats pokemon = pokemonArray[i];
                pokemonImage.sprite = Resources.Load<Sprite>(pokemonArray[i].getName()); // changes image
                pokemonName.text = pokemon.getName() + " " + pokemon.getHealth() + "/" + pokemon.maxHealth(); // changes text
                
                // deactivate button if the pokemon is dead
                if (pokemonArray[i].getHealth() < 1)
                {
                    Debug.Log("button deactivated");
                    button.SetActive(false);
                }
            } else 
            {
                button.SetActive(false);
            }
        }
    }

    // switches the pokemon in the array so that the pokemon you selected is first.
    public void switchPokemon(int n)
    {
        //Debug.Log("I am being run");
        PokemonStats[] party = PokemonParty.getParty();
        PokemonStats volthesis = party[0]; // because the first slot will always be volthesis
        party[0] = party[n]; // party of n is the pokemon wanted
        party[n] = volthesis;

        // change the visual pokemon
        GameObject Canvas = GameObject.FindWithTag("Canvas");

        Image pokemonImage = Canvas.transform.Find("CurrentPokemon").GetComponent<Image>(); // gets the image name
        Text pokemonName = Canvas.transform.Find("CurrentName").GetComponent<Text>(); // gets the text
        Text totalHealth = Canvas.transform.Find("TotalHealth").GetComponent<Text>(); // gets the text
        Text currentHealth = Canvas.transform.Find("CurrentHealth").GetComponent<Text>(); // gets the text
        
        Image primaryImage = GameObject.Find("Canvas/BattleOptions/PrimaryButton").GetComponent<Image>(); // gets button image
        Image secondaryImage = GameObject.Find("Canvas/BattleOptions/SecondaryButton").GetComponent<Image>(); // gets button image

        // updates all of the values
        pokemonImage.sprite = Resources.Load<Sprite>(party[0].getName() + "BackSprite"); // changes image
        pokemonName.text = party[0].getName(); // changes text
        currentHealth.text = party[0].getHealth() + "";
        totalHealth.text = party[0].maxHealth() + "";

        primaryImage.sprite = Resources.Load<Sprite>(party[0].getPrimaryType() + "Button");
        secondaryImage.sprite = Resources.Load<Sprite>(party[0].getSecondaryType() + "Button");

        GameObject Panel = GameObject.FindWithTag("Panel");
        Panel.SetActive(false);
    }

    public void close()
    {
        GameObject panel = GameObject.FindWithTag("Panel");
        panel.SetActive(false);
    }
}
