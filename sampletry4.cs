using System;
using System.Collections.Generic;

// Core Pokemon Class
public class Pokemon
{
    public string Name { get; set; }
    public int Level { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }

    public Pokemon(string name, int level, int hp, int attack, int defense)
    {
        Name = name;
        Level = level;
        MaxHp = hp;
        Hp = hp;
        Attack = attack;
        Defense = defense;
    }

    // Calculate damage dealt
    public int DealDamage(Pokemon target)
    {
        int damage = Attack - target.Defense;
        return damage > 0 ? damage : 1; // Minimum 1 damage
    }

    // Restore HP
    public void Heal(int amount)
    {
        Hp = Math.Min(Hp + amount, MaxHp);
        Console.WriteLine($"{Name} healed {amount} HP! Current HP: {Hp}/{MaxHp}");
    }
}

// Player Class
public class Player
{
    public string Name { get; set; }
    public List<Pokemon> Party { get; set; }
    public int Potions { get; set; } = 3; // Starting potions

    public Player(string name)
    {
        Name = name;
        Party = new List<Pokemon>();
    }
}

// Main Game Class
class PokemonAdventure
{
    static void Main(string[] args)
    {
        // Initialize game
        Console.Title = "C# Pokemon Adventure";
        Console.WriteLine("=== POKEMON ADVENTURE ===");
        Console.Write("Enter your trainer name: ");
        Player trainer = new Player(Console.ReadLine());

        // Give starter Pokemon
        Console.WriteLine("\nChoose your starter Pokemon:");
        Console.WriteLine("1. Bulbasaur (Grass)");
        Console.WriteLine("2. Charmander (Fire)");
        Console.WriteLine("3. Squirtle (Water)");
        
        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1: trainer.Party.Add(new Pokemon("Bulbasaur", 5, 45, 49, 49)); break;
            case 2: trainer.Party.Add(new Pokemon("Charmander", 5, 39, 52, 43)); break;
            case 3: trainer.Party.Add(new Pokemon("Squirtle", 5, 44, 48, 65)); break;
            default: trainer.Party.Add(new Pokemon("Bulbasaur", 5, 45, 49, 49)); break;
        }

        Pokemon starter = trainer.Party[0];
        Console.WriteLine($"\nWelcome, {trainer.Name}! Your journey with {starter.Name} begins now!");

        // Main game loop
        bool isPlaying = true;
        while (isPlaying)
        {
            Console.WriteLine("\n--- World Map ---");
            Console.WriteLine("1. Explore Grasslands (Wild Pokemon!)");
            Console.WriteLine("2. Check Pokemon Party");
            Console.WriteLine("3. Use Potion");
            Console.WriteLine("4. Quit Game");
            Console.Write("Choose an action: ");
            int action = int.Parse(Console.ReadLine());

            switch (action)
            {
                case 1: ExploreGrasslands(trainer); break;
                case 2: ViewParty(trainer); break;
                case 3: UsePotion(trainer); break;
                case 4: isPlaying = false; break;
                default: Console.WriteLine("Invalid choice! Try again."); break;
            }
        }

        Console.WriteLine("Thanks for playing!");
    }

    // Explore grasslands and trigger wild encounters
    static void ExploreGrasslands(Player trainer)
    {
        Console.WriteLine("\nWalking through tall grass...");
        Random rand = new Random();
        int encounterChance = rand.Next(1, 4); // 33% encounter rate

        if (encounterChance == 1)
        {
            // Spawn wild Pokemon
            string[] wildPokemon = { "Pidgey", "Rattata", "Caterpie" };
            string wildName = wildPokemon[rand.Next(0, 3)];
            Pokemon wild = new Pokemon(wildName, 3 + rand.Next(3), 30, 35, 30);
            
            Console.WriteLine($"\nA wild {wild.Name} (Lvl {wild.Level}) appeared!");
            Battle(trainer, wild);
        }
        else
        {
            Console.WriteLine("No Pokemon found... Keep walking!");
        }
    }

    // Turn-based battle system
    static void Battle(Player trainer, Pokemon wild)
    {
        Pokemon playerMon = trainer.Party[0]; // Simplified: use first Pokemon
        bool battleActive = true;

        while (battleActive)
        {
            Console.WriteLine($"\n--- BATTLE ---");
            Console.WriteLine($"{playerMon.Name}: {playerMon.Hp}/{playerMon.MaxHp} HP");
            Console.WriteLine($"{wild.Name}: {wild.Hp}/{wild.MaxHp} HP");
            Console.WriteLine("\n1. Attack");
            Console.WriteLine("2. Run");
            Console.Write("Choose action: ");
            int battleChoice = int.Parse(Console.ReadLine());

            switch (battleChoice)
            {
                case 1:
                    // Player attacks first
                    int playerDamage = playerMon.DealDamage(wild);
                    wild.Hp -= playerDamage;
                    Console.WriteLine($"{playerMon.Name} attacks! Deals {playerDamage} damage!");

                    if (wild.Hp <= 0)
                    {
                        Console.WriteLine($"\nWild {wild.Name} fainted!");
                        battleActive = false;
                        break;
                    }

                    // Wild Pokemon counterattacks
                    int wildDamage = wild.DealDamage(playerMon);
                    playerMon.Hp -= wildDamage;
                    Console.WriteLine($"{wild.Name} counterattacks! Deals {wildDamage} damage!");

                    if (playerMon.Hp <= 0)
                    {
                        Console.WriteLine($"\n{playerMon.Name} fainted! Game Over!");
                        Environment.Exit(0);
                    }
                    break;

                case 2:
                    Console.WriteLine("You ran away safely!");
                    battleActive = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice! Wild Pokemon attacks!");
                    int defaultDamage = wild.DealDamage(playerMon);
                    playerMon.Hp -= defaultDamage;
                    Console.WriteLine($"{wild.Name} deals {defaultDamage} damage!");
                    break;
            }
        }
    }

    // View party status
    static void ViewParty(Player trainer)
    {
        Console.WriteLine("\n--- YOUR POKEMON ---");
        foreach (var p in trainer.Party)
        {
            Console.WriteLine($"{p.Name} | Lvl {p.Level} | HP: {p.Hp}/{p.MaxHp} | Atk: {p.Attack} | Def: {p.Defense}");
        }
    }

    // Use potion to heal Pokemon
    static void UsePotion(Player trainer)
    {
        if (trainer.Potions <= 0)
        {
            Console.WriteLine("No potions left!");
            return;
        }

        ViewParty(trainer);
        Console.Write("Choose which Pokemon to heal: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < trainer.Party.Count)
        {
            trainer.Party[index].Heal(20); // Potion heals 20 HP
            trainer.Potions--;
            Console.WriteLine($"Potions remaining: {trainer.Potions}");
        }
        else
        {
            Console.WriteLine("Invalid Pokemon!");
        }
    }
