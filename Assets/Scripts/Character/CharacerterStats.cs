using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemSystem;
using Character.Stats;

public class CharacerterStats : MonoBehaviour
{
    [SerializeField] Stat maxHealth = new Stat(100);
    Stat currentHealth = new Stat(80);
    Stat strenght = new Stat(2);
    Stat speed = new Stat(5);
    Stat regen = new Stat(5);

    public Stat MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public Stat CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public Stat Strenght { get { return strenght; } set { strenght = value; } }
    public Stat Speed { get { return speed; } set { speed = value; } }

    public Stat Regen { get; set; }

    private PermPerTimeModifier regeneration;

    private List<Modifier> modifiers = new List<Modifier>();

    private void Start()
    {
        PermanentModifier pmod = new PermanentModifier("Health Up", "Up your max healh.", new Dictionary<string, float> { { "MaxHealth", 40f } });
        AddModifier(pmod);
        //InTimeModifier pois = new InTimeModifier("Poison", "You losing HP.", new Dictionary<string, float> { { "CurrentHealth", -20f } }, 6);
        
        TempPerTimeModifier pois = new TempPerTimeModifier("Poison", "You losing HP.", new Dictionary<string, float> { { "CurrentHealth", -5f } }, 6);
        AddModifier(pois);
        regeneration = new PermPerTimeModifier("Regeneration", "", new Dictionary<string, float> { { "CurrentHealth", (currentHealth.StatValue < maxHealth.StatValue) ? regen.StatValue : 0 } });
        AddModifier(regeneration);
    }

    private void Update()
    {
        if (currentHealth.StatValue < maxHealth.StatValue)
            regeneration.Changers["CurrentHealth"] = 2;
        else if (regeneration != null)
        {
            currentHealth.StatValue = maxHealth.StatValue;
            regeneration.Changers["CurrentHealth"] = 0;
        }
        SelfDamage();
        RemoveFirstMod();
    }

    public void AddModifier(Modifier mod)
    {
        if(mod.GetType() == typeof(TemporaryModifier))
        {
            StartCoroutine(((TemporaryModifier)mod).Use(this, RemoveMod));
        }
        else if(mod.GetType() == typeof(PermanentModifier))
        {
            ((PermanentModifier)mod).Use(this);
        }
        else if(mod.GetType() == typeof(TempPerTimeModifier))
        {
            StartCoroutine(((TempPerTimeModifier)mod).Use(this, RemoveMod));
        }
        else if (mod.GetType() == typeof(PermPerTimeModifier))
        {
            StartCoroutine(((PermPerTimeModifier)mod).Use(this, RemoveMod));
        }
        else if(mod.GetType() == typeof(InTimeModifier))
        {
            StartCoroutine(((InTimeModifier)mod).Use(this, RemoveMod));
        }

        modifiers.Add(mod);
    }

    private void SelfDamage()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            CurrentHealth.StatValue -= 10;
        }
    }

    private void RemoveFirstMod()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            modifiers.RemoveAt(0);
        }
    }

    private void RemoveMod(Modifier mod)
    {
        modifiers.Remove(mod);
    }

}
