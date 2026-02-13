PokemonGame/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── Battle/
│   │   ├── Player/
│   │   ├── Pokemon/
│   │   ├── UI/
│   │   └── World/
│   ├── Scenes/
│   ├── Sprites/
│   └── Audio/
// Assets/Scripts/Pokemon/PokemonSpecies.cs
using System.Collections.Generic;
using UnityEngine;

public class PokemonSpecies : ScriptableObject
{
    public int id;
    public string pokemonName;
    public PokemonType type1;
    public PokemonType type2;
    
    [System.Serializable]
    public class Stats
    {
        public int hp;
        public int attack;
        public int defense;
        public int spAtk;
        public int spDef;
        public int speed;
    }
    
    public Stats baseStats;
    public List<string> movePool;
    public List<string> abilities;
    public Sprite sprite;
    public float catchRate;
}

public enum PokemonType
{
    Normal, Fire, Water, Electric, Grass, Ice, Fighting,
    Poison, Ground, Flying, Psychic, Bug, Rock, Ghost,
    Dragon, Dark, Steel, Fairy
}
// Assets/Scripts/Pokemon/Pokemon.cs
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    public PokemonSpecies species;
    public int level;
    public int experience;
    public int currentHP;
    
    [System.Serializable]
    public class Stats
    {
        public int hp;
        public int attack;
        public int defense;
        public int spAtk;
        public int spDef;
        public int speed;
    }
    
    public Stats stats;
    public List<Move> moves = new List<Move>();
    public string ability;
    public List<int> effortValues = new List<int>(6); // EV tracking
    public int friendship = 0;
    
    // Constructor
    public Pokemon(PokemonSpecies pokemonSpecies, int lvl)
    {
        species = pokemonSpecies;
        level = lvl;
        experience = 0;
        ability = pokemonSpecies.abilities[0];
        
        CalculateStats();
        currentHP = stats.hp;
        LearnStartingMoves();
    }
    
    // Calculate actual stats from base stats and level
    private void CalculateStats()
    {
        stats = new Stats();
        stats.hp = (2 * species.baseStats.hp + 0 + 0 / 4) * level / 100 + level + 5;
        stats.attack = (2 * species.baseStats.attack + 0 + 0 / 4) * level / 100 + 5;
        stats.defense = (2 * species.baseStats.defense + 0 + 0 / 4) * level / 100 + 5;
        stats.spAtk = (2 * species.baseStats.spAtk + 0 + 0 / 4) * level / 100 + 5;
        stats.spDef = (2 * species.baseStats.spDef + 0 + 0 / 4) * level / 100 + 5;
        stats.speed = (2 * species.baseStats.speed + 0 + 0 / 4) * level / 100 + 5;
    }
    
    private void LearnStartingMoves()
    {
        moves.Clear();
        // Learn up to 4 starting moves
        for (int i = 0; i < Mathf.Min(4, species.movePool.Count); i++)
        {
            moves.Add(MoveDatabase.GetMove(species.movePool[i]));
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
    }
    
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(stats.hp, currentHP + amount);
    }
    
    public bool IsAlive()
    {
        return currentHP > 0;
    }
    
    public float GetHPPercentage()
    {
        return (float)currentHP / stats.hp;
    }
}
// Assets/Scripts/Pokemon/Move.cs
using UnityEngine;

[System.Serializable]
public class Move
{
    public string moveName;
    public PokemonType type;
    public MoveCategory category; // Physical, Special, Status
    public int power;
    public int accuracy; // 0-100
    public int maxPP;
    public int currentPP;
    public string description;
    public float priority = 0;
    
    public Move(string name, PokemonType moveType, MoveCategory cat, 
                int pow, int acc, int pp)
    {
        moveName = name;
        type = moveType;
        category = cat;
        power = pow;
        accuracy = acc;
        maxPP = pp;
        currentPP = pp;
    }
    
    public void UsePP()
    {
        currentPP = Mathf.Max(0, currentPP - 1);
    }
    
    public bool CanUse()
    {
        return currentPP > 0;
    }
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}
// Assets/Scripts/Pokemon/MoveDatabase.cs
using System.Collections.Generic;
using UnityEngine;

public static class MoveDatabase
{
    private static Dictionary<string, Move> moves = new Dictionary<string, Move>();
    
    [RuntimeInitializeOnLoadMethod]
    public static void InitializeMoves()
    {
        // Fire Type Moves
        moves["Ember"] = new Move("Ember", PokemonType.Fire, 
            MoveCategory.Special, 40, 100, 25);
        moves["Fire Blast"] = new Move("Fire Blast", PokemonType.Fire, 
            MoveCategory.Special, 110, 85, 5);
        
        // Water Type Moves
        moves["Bubble"] = new Move("Bubble", PokemonType.Water, 
            MoveCategory.Special, 40, 100, 30);
        moves["Hydro Pump"] = new Move("Hydro Pump", PokemonType.Water, 
            MoveCategory.Special, 110, 80, 5);
        
        // Normal Type Moves
        moves["Tackle"] = new Move("Tackle", PokemonType.Normal, 
            MoveCategory.Physical, 40, 100, 35);
        moves["Hyper Beam"] = new Move("Hyper Beam", PokemonType.Normal, 
            MoveCategory.Special, 150, 90, 5);
        
        // Electric Type Moves
        moves["Thunderbolt"] = new Move("Thunderbolt", PokemonType.Electric, 
            MoveCategory.Special, 90, 100, 15);
        moves["Thunder Wave"] = new Move("Thunder Wave", PokemonType.Electric, 
            MoveCategory.Status, 0, 90, 20);
        
        // Grass Type Moves
        moves["Vine Whip"] = new Move("Vine Whip", PokemonType.Grass, 
            MoveCategory.Physical, 45, 100, 25);
        moves["Solar Beam"] = new Move("Solar Beam", PokemonType.Grass, 
            MoveCategory.Special, 120, 100, 10);
    }
    
    public static Move GetMove(string moveName)
    {
        if (moves.ContainsKey(moveName))
        {
            return new Move(moves[moveName].moveName, moves[moveName].type,
                moves[moveName].category, moves[moveName].power,
                moves[moveName].accuracy, moves[moveName].maxPP);
        }
        return null;
    }
}
// Assets/Scripts/Battle/BattleManager.cs
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    
    private Pokemon playerPokemon;
    private Pokemon enemyPokemon;
    private BattleState currentState;
    
    public delegate void BattleEventHandler();
    public event BattleEventHandler OnBattleEnd;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public void StartBattle(Pokemon player, Pokemon enemy)
    {
        playerPokemon = player;
        enemyPokemon = enemy;
        currentState = BattleState.Start;
        StartCoroutine(BattleRoutine());
    }
    
    private IEnumerator BattleRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        while (playerPokemon.IsAlive() && enemyPokemon.IsAlive())
        {
            // Player's turn
            yield return StartCoroutine(PlayerTurn());
            
            if (!enemyPokemon.IsAlive())
                break;
            
            // Enemy's turn
            yield return StartCoroutine(EnemyTurn());
        }
        
        EndBattle();
    }
    
    private IEnumerator PlayerTurn()
    {
        currentState = BattleState.PlayerTurn;
        // Wait for player input (UI will handle this)
        yield return new WaitUntil(() => currentState != BattleState.PlayerTurn);
    }
    
    private IEnumerator EnemyTurn()
    {
        currentState = BattleState.EnemyTurn;
        yield return new WaitForSeconds(1f);
        
        // Simple AI: random move
        Move enemyMove = enemyPokemon.moves[Random.Range(0, enemyPokemon.moves.Count)];
        ExecuteAttack(enemyPokemon, playerPokemon, enemyMove);
        
        yield return new WaitForSeconds(1f);
    }
    
    public void PlayerAttack(Move move)
    {
        if (!move.CanUse())
        {
            Debug.Log("No PP left!");
            return;
        }
        
        ExecuteAttack(playerPokemon, enemyPokemon, move);
    }
    
    private void ExecuteAttack(Pokemon attacker, Pokemon defender, Move move)
    {
        // Check accuracy
        if (Random.Range(0, 100) > move.accuracy)
        {
            Debug.Log($"{attacker.species.pokemonName} used {move.moveName} but it missed!");
            move.UsePP();
            return;
        }
        
        int damage = CalculateDamage(attacker, defender, move);
        defender.TakeDamage(damage);
        move.UsePP();
        
        Debug.Log($"{attacker.species.pokemonName} used {move.moveName}! " +
                  $"It deals {damage} damage!");
    }
    
    private int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
    {
        float baseDamage = 0;
        
        if (move.category == MoveCategory.Physical)
            baseDamage = ((2 * attacker.level / 5f + 2) * move.power * 
                         (attacker.stats.attack / (float)defender.stats.defense) / 50f + 2);
        else if (move.category == MoveCategory.Special)
            baseDamage = ((2 * attacker.level / 5f + 2) * move.power * 
                         (attacker.stats.spAtk / (float)defender.stats.spDef) / 50f + 2);
        
        // Type effectiveness
        float effectiveness = GetTypeEffectiveness(move.type, defender.species.type1);
        if (defender.species.type2 != PokemonType.Normal)
            effectiveness *= GetTypeEffectiveness(move.type, defender.species.type2);
        
        baseDamage *= effectiveness;
        
        // Random multiplier (0.85 - 1.0)
        baseDamage *= Random.Range(0.85f, 1f);
        
        return Mathf.Max(1, (int)baseDamage);
    }
    
    private float GetTypeEffectiveness(PokemonType attackType, PokemonType defendType)
    {
        // Simplified type matchup
        // Super effective = 2.0f, Normal = 1.0f, Not very effective = 0.5f
        
        if (attackType == PokemonType.Fire)
        {
            if (defendType == PokemonType.Grass || defendType == PokemonType.Bug)
                return 2f;
            if (defendType == PokemonType.Water || defendType == PokemonType.Rock)
                return 0.5f;
        }
        
        if (attackType == PokemonType.Water)
        {
            if (defendType == PokemonType.Fire || defendType == PokemonType.Ground)
                return 2f;
            if (defendType == PokemonType.Grass || defendType == PokemonType.Electric)
                return 0.5f;
        }
        
        if (attackType == PokemonType.Electric)
        {
            if (defendType == PokemonType.Water || defendType == PokemonType.Flying)
                return 2f;
            if (defendType == PokemonType.Grass)
                return 0.5f;
        }
        
        if (attackType == PokemonType.Grass)
        {
            if (defendType == PokemonType.Water || defendType == PokemonType.Ground)
                return 2f;
            if (defendType == PokemonType.Fire || defendType == PokemonType.Bug)
                return 0.5f;
        }
        
        return 1f;
    }
    
    private void EndBattle()
    {
        if (playerPokemon.IsAlive())
        {
            Debug.Log("Player wins!");
        }
        else
        {
            Debug.Log("Player lost!");
        }
        
        OnBattleEnd?.Invoke();
    }
}

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    End
}
// Assets/Scripts/Player/PlayerManager.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [SerializeField] private string playerName;
    [SerializeField] private int money;
    
    private List<Pokemon> team = new List<Pokemon>();
    private List<PokemonSpecies> pokedex = new List<PokemonSpecies>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public void AddPokemonToTeam(Pokemon pokemon)
    {
        if (team.Count < 6)
        {
            team.Add(pokemon);
            Debug.Log($"Added {pokemon.species.pokemonName} to team!");
        }
        else
        {
            Debug.Log("Team is full!");
        }
    }
    
    public void CatchPokemon(PokemonSpecies species, int level)
    {
        Pokemon newPokemon = new Pokemon(species, level);
        AddPokemonToTeam(newPokemon);
        
        if (!pokedex.Contains(species))
            pokedex.Add(species);
    }
    
    public List<Pokemon> GetTeam()
    {
        return team;
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
    }
    
    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }
    
    public int GetMoney()
    {
        return money;
    }
    
    public int GetPokedexCompletion()
    {
        return pokedex.Count;
    }
}
// Assets/Scripts/World/PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        
        if (movement != Vector2.zero)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
    
    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trainer"))
        {
            // Battle logic
            Trainer trainer = collision.GetComponent<Trainer>();
            if (trainer != null)
                StartTrainerBattle(trainer);
        }
    }
    
    private void StartTrainerBattle(Trainer trainer)
    {
        rb.velocity = Vector2.zero;
        Pokemon playerPokemon = PlayerManager.instance.GetTeam()[0];
        Pokemon trainerPokemon = trainer.GetRandomPokemon();
        
        BattleManager.instance.StartBattle(playerPokemon, trainerPokemon);
    }
}
// Assets/Scripts/UI/BattleUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Image playerPokemonImage;
    [SerializeField] private Image enemyPokemonImage;
    
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    
    [SerializeField] private Slider playerHPSlider;
    [SerializeField] private Slider enemyHPSlider;
    
    [SerializeField] private Button[] moveButtons;
    
    private Pokemon currentPlayerPokemon;
    private Pokemon currentEnemyPokemon;
    
    private void Start()
    {
        BattleManager.instance.OnBattleEnd += OnBattleEnd;
    }
    
    public void UpdateBattleUI(Pokemon playerPokemon, Pokemon enemyPokemon)
    {
        currentPlayerPokemon = playerPokemon;
        currentEnemyPokemon = enemyPokemon;
        
        // Update names
        playerNameText.text = playerPokemon.species.pokemonName;
        enemyNameText.text = enemyPokemon.species.pokemonName;
        
        // Update sprites
        playerPokemonImage.sprite = playerPokemon.species.sprite;
        enemyPokemonImage.sprite = enemyPokemon.species.sprite;
        
        // Update move buttons
        for (int i = 0; i < moveButtons.Length; i++)
        {
            if (i < playerPokemon.moves.Count)
            {
                Move move = playerPokemon.moves[i];
                TextMeshProUGUI buttonText = moveButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = $"{move.moveName}\n(PP: {move.currentPP}/{move.maxPP})";
                
                int moveIndex = i;
                moveButtons[i].onClick.AddListener(() => OnMoveSelected(moveIndex));
            }
        }
        
        UpdateHPBars();
    }
    
    public void UpdateHPBars()
    {
        playerHPSlider.value = currentPlayerPokemon.GetHPPercentage();
        enemyHPSlider.value = currentEnemyPokemon.GetHPPercentage();
    }
    
    public void OnMoveSelected(int moveIndex)
    {
        if (moveIndex < currentPlayerPokemon.moves.Count)
        {
            Move selectedMove = currentPlayerPokemon.moves[moveIndex];
            BattleManager.instance.PlayerAttack(selectedMove);
            UpdateHPBars();
        }
    }
    
    private void OnBattleEnd()
    {
        Debug.Log("Battle ended! UI should be hidden.");
    }
}
// Assets/Scripts/Pokemon/PokedexManager.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PokedexEntry
{
    public PokemonSpecies species;
    public bool seen;
    public bool caught;
    public int timesCaught;
    public int timesDefeated;
    public List<Pokemon> caughtVariations = new List<Pokemon>();
}

public class PokedexManager : MonoBehaviour
{
    public static PokedexManager instance;
    
    private Dictionary<int, PokedexEntry> pokedex = new Dictionary<int, PokedexEntry>();
    private List<PokemonSpecies> allSpecies = new List<PokemonSpecies>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        LoadAllSpecies();
        InitializePokedex();
    }
    
    private void LoadAllSpecies()
    {
        // Load all PokemonSpecies from Resources folder
        allSpecies = Resources.LoadAll<PokemonSpecies>("Pokemon").ToList();
    }
    
    private void InitializePokedex()
    {
        foreach (PokemonSpecies species in allSpecies)
        {
            pokedex[species.id] = new PokedexEntry
            {
                species = species,
                seen = false,
                caught = false,
                timesCaught = 0,
                timesDefeated = 0
            };
        }
    }
    
    public void RegisterSighting(PokemonSpecies species)
    {
        if (pokedex.ContainsKey(species.id))
        {
            pokedex[species.id].seen = true;
            Debug.Log($"{species.pokemonName} registered in Pokédex!");
        }
    }
    
    public void RegisterCatch(Pokemon pokemon)
    {
        if (pokedex.ContainsKey(pokemon.species.id))
        {
            PokedexEntry entry = pokedex[pokemon.species.id];
            entry.caught = true;
            entry.timesCaught++;
            entry.caughtVariations.Add(pokemon);
            Debug.Log($"{pokemon.species.pokemonName} caught! Total: {entry.timesCaught}");
        }
    }
    
    public void RegisterDefeat(PokemonSpecies species)
    {
        if (pokedex.ContainsKey(species.id))
        {
            pokedex[species.id].timesDefeated++;
        }
    }
    
    public PokedexEntry GetEntry(int id)
    {
        return pokedex.ContainsKey(id) ? pokedex[id] : null;
    }
    
    public float GetCompletion()
    {
        int caughtCount = pokedex.Values.Count(e => e.caught);
        return (float)caughtCount / pokedex.Count * 100f;
    }
    
    public List<PokedexEntry> GetAllEntries()
    {
        return pokedex.Values.ToList();
    }
    
    public List<PokedexEntry> GetSeenEntries()
    {
        return pokedex.Values.Where(e => e.seen).ToList();
    }
    
    public List<PokedexEntry> GetCaughtEntries()
    {
        return pokedex.Values.Where(e => e.caught).ToList();
    }
}
// Assets/Scripts/UI/PokedexUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PokedexUI : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Image pokemonImage;
    [SerializeField] private TextMeshProUGUI pokemonNameText;
    [SerializeField] private TextMeshProUGUI pokemonTypeText;
    [SerializeField] private TextMeshProUGUI pokemonStatsText;
    [SerializeField] private TextMeshProUGUI pokemonDescriptionText;
    [SerializeField] private TextMeshProUGUI completionText;
    [SerializeField] private Slider completionSlider;
    
    private List<PokedexEntry> currentEntries;
    private PokedexEntry selectedEntry;
    
    private void Start()
    {
        DisplayAllEntries();
    }
    
    public void DisplayAllEntries()
    {
        ClearContainer();
        currentEntries = PokedexManager.instance.GetAllEntries();
        
        foreach (PokedexEntry entry in currentEntries)
        {
            GameObject entryButton = Instantiate(entryPrefab, entryContainer);
            Button btn = entryButton.GetComponent<Button>();
            TextMeshProUGUI text = entryButton.GetComponentInChildren<TextMeshProUGUI>();
            
            if (entry.caught)
            {
                text.text = $"#{entry.species.id:000} {entry.species.pokemonName}";
            }
            else if (entry.seen)
            {
                text.text = $"#{entry.species.id:000} ?????";
            }
            else
            {
                text.text = $"#{entry.species.id:000} ---";
                btn.interactable = false;
            }
            
            btn.onClick.AddListener(() => DisplayEntry(entry));
        }
        
        UpdateCompletion();
    }
    
    public void DisplayCaughtOnly()
    {
        ClearContainer();
        currentEntries = PokedexManager.instance.GetCaughtEntries();
        
        foreach (PokedexEntry entry in currentEntries)
        {
            GameObject entryButton = Instantiate(entryPrefab, entryContainer);
            Button btn = entryButton.GetComponent<Button>();
            TextMeshProUGUI text = entryButton.GetComponentInChildren<TextMeshProUGUI>();
            
            text.text = $"#{entry.species.id:000} {entry.species.pokemonName} (x{entry.timesCaught})";
            btn.onClick.AddListener(() => DisplayEntry(entry));
        }
        
        UpdateCompletion();
    }
    
    private void DisplayEntry(PokedexEntry entry)
    {
        selectedEntry = entry;
        
        pokemonImage.sprite = entry.species.sprite;
        pokemonNameText.text = entry.species.pokemonName;
        pokemonTypeText.text = $"{entry.species.type1}" + 
            (entry.species.type2 != PokemonType.Normal ? $" / {entry.species.type2}" : "");
        
        string statsText = $"HP: {entry.species.baseStats.hp}\n" +
                          $"ATK: {entry.species.baseStats.attack}\n" +
                          $"DEF: {entry.species.baseStats.defense}\n" +
                          $"SP.ATK: {entry.species.baseStats.spAtk}\n" +
                          $"SP.DEF: {entry.species.baseStats.spDef}\n" +
                          $"SPEED: {entry.species.baseStats.speed}";
        pokemonStatsText.text = statsText;
        
        if (entry.caught)
        {
            pokemonDescriptionText.text = $"Times Caught: {entry.timesCaught}\n" +
                                         $"Times Defeated: {entry.timesDefeated}";
        }
        else if (entry.seen)
        {
            pokemonDescriptionText.text = "Seen only. Not yet caught.";
        }
    }
    
    private void UpdateCompletion()
    {
        float completion = PokedexManager.instance.GetCompletion();
        completionSlider.value = completion / 100f;
        completionText.text = $"Pokédex Completion: {completion:F1}%";
    }
    
    private void ClearContainer()
    {
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
// Assets/Scripts/Items/Item.cs
using UnityEngine;

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public int price;
    public ItemType itemType;
    public ItemEffect effect;
    
    public virtual void Use(Pokemon target)
    {
        // Override in subclasses
    }
}

public enum ItemType
{
    Pokeball,
    Potion,
    StatusHealer,
    EXPBoost,
    EvolutionStone,
    TM,
    KeyItem
}

public enum ItemEffect
{
    None,
    RestoreHP,
    RestoreAllHP,
    CurePoison,
    CureParalysis,
    RevivePokemon,
    CatchPokemon,
    BoostEXP
}
// Assets/Scripts/Items/HealingItem.cs
using UnityEngine;

public class HealingItem : Item
{
    [SerializeField] private int healAmount;
    
    public override void Use(Pokemon target)
    {
        target.Heal(healAmount);
        Debug.Log($"Restored {healAmount} HP to {target.species.pokemonName}!");
    }
}

// Assets/Scripts/Items/Pokeball.cs
public class Pokeball : Item
{
    [SerializeField] private float catchMultiplier = 1f;
    
    public bool AttemptCatch(Pokemon wildPokemon)
    {
        float catchRate = wildPokemon.species.catchRate * catchMultiplier;
        float hpPercentage = wildPokemon.GetHPPercentage();
        
        // Weaker Pokémon are easier to catch
        if (hpPercentage < 0.33f)
            catchRate *= 2.5f;
        else if (hpPercentage < 0.66f)
            catchRate *= 1.5f;
        
        return Random.Range(0f, 100f) < catchRate;
    }
}

// Assets/Scripts/Items/EvolutionStone.cs
public class EvolutionStone : Item
{
    [SerializeField] private PokemonType affectsType;
    
    public bool CanEvolve(Pokemon target)
    {
        return target.species.type1 == affectsType || 
               target.species.type2 == affectsType;
    }
}
// Assets/Scripts/Items/Inventory.cs
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
    
    public InventorySlot(Item newItem, int count = 1)
    {
        item = newItem;
        quantity = count;
    }
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    
    [SerializeField] private int maxSlots = 30;
    private List<InventorySlot> slots = new List<InventorySlot>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public bool AddItem(Item item, int quantity = 1)
    {
        // Check if item already exists
        InventorySlot existingSlot = slots.Find(s => s.item.id == item.id);
        
        if (existingSlot != null)
        {
            existingSlot.quantity += quantity;
            return true;
        }
        
        // Add new slot
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(item, quantity));
            return true;
        }
        
        Debug.Log("Inventory is full!");
        return false;
    }
    
    public bool RemoveItem(int itemId, int quantity = 1)
    {
        InventorySlot slot = slots.Find(s => s.item.id == itemId);
        
        if (slot != null && slot.quantity >= quantity)
        {
            slot.quantity -= quantity;
            
            if (slot.quantity <= 0)
                slots.Remove(slot);
            
            return true;
        }
        
        return false;
    }
    
    public int GetItemQuantity(int itemId)
    {
        InventorySlot slot = slots.Find(s => s.item.id == itemId);
        return slot != null ? slot.quantity : 0;
    }
    
    public Item GetItem(int itemId)
    {
        InventorySlot slot = slots.Find(s => s.item.id == itemId);
        return slot != null ? slot.item : null;
    }
    
    public List<InventorySlot> GetAllItems()
    {
        return new List<InventorySlot>(slots);
    }
    
    public void UseItem(Item item, Pokemon target)
    {
        item.Use(target);
        RemoveItem(item.id, 1);
    }
}
// Assets/Scripts/UI/InventoryUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI selectedItemNameText;
    [SerializeField] private TextMeshProUGUI selectedItemDescriptionText;
    [SerializeField] private TextMeshProUGUI selectedItemQuantityText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;
    
    private InventorySlot selectedSlot;
    
    private void Start()
    {
        RefreshInventory();
        useButton.onClick.AddListener(UseSelectedItem);
        dropButton.onClick.AddListener(DropSelectedItem);
    }
    
    public void RefreshInventory()
    {
        ClearContainer();
        List<InventorySlot> items = Inventory.instance.GetAllItems();
        
        foreach (InventorySlot slot in items)
        {
            GameObject slotUI = Instantiate(itemSlotPrefab, itemContainer);
            Button slotButton = slotUI.GetComponent<Button>();
            Image slotIcon = slotUI.GetComponent<Image>();
            TextMeshProUGUI quantityText = slotUI.GetComponentInChildren<TextMeshProUGUI>();
            
            slotIcon.sprite = slot.item.icon;
            quantityText.text = $"x{slot.quantity}";
            
            slotButton.onClick.AddListener(() => SelectSlot(slot));
        }
    }
    
    private void SelectSlot(InventorySlot slot)
    {
        selectedSlot = slot;
        
        selectedItemIcon.sprite = slot.item.icon;
        selectedItemNameText.text = slot.item.itemName;
        selectedItemDescriptionText.text = slot.item.description;
        selectedItemQuantityText.text = $"x{slot.quantity}";
    }
    
    private void UseSelectedItem()
    {
        if (selectedSlot != null)
        {
            // Get first Pokémon in team
            Pokemon targetPokemon = PlayerManager.instance.GetTeam()[0];
            Inventory.instance.UseItem(selectedSlot.item, targetPokemon);
            RefreshInventory();
        }
    }
    
    private void DropSelectedItem()
    {
        if (selectedSlot != null)
        {
            Inventory.instance.RemoveItem(selectedSlot.item.id, 1);
            RefreshInventory();
        }
    }
    
    private void ClearContainer()
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
// Assets/Scripts/Pokemon/EvolutionSystem.cs
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EvolutionData
{
    public PokemonSpecies evolvesInto;
    public EvolutionMethod method;
    public int requiredLevel;
    public Item requiredItem;
    public PokemonSpecies friendlyWith; // For trade evolution
    public int requiredFriendship;
}

public enum EvolutionMethod
{
    LevelUp,
    Item,
    Trade,
    Happiness,
    Time,
    Location
}

public class EvolutionSystem : MonoBehaviour
{
    public static EvolutionSystem instance;
    
    private Dictionary<int, List<EvolutionData>> evolutionTree = 
        new Dictionary<int, List<EvolutionData>>();
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        InitializeEvolutionTree();
    }
    
    private void InitializeEvolutionTree()
    {
        // Load evolution data from scriptable objects or database
        // Example structure:
        // Bulbasaur (1) -> Ivysaur (16) -> Venusaur (32)
        // Charmander (4) -> Charmeleon (16) -> Charizard (36)
        // Squirtle (7) -> Wartortle (16) -> Blastoise (36)
    }
    
    public List<EvolutionData> GetPossibleEvolutions(Pokemon pokemon)
    {
        if (evolutionTree.ContainsKey(pokemon.species.id))
            return evolutionTree[pokemon.species.id];
        
        return new List<EvolutionData>();
    }
    
    public bool CanEvolve(Pokemon pokemon, EvolutionData evolution)
    {
        switch (evolution.method)
        {
            case EvolutionMethod.LevelUp:
                return pokemon.level >= evolution.requiredLevel;
            
            case EvolutionMethod.Item:
                return Inventory.instance.GetItemQuantity(evolution.requiredItem.id) > 0;
            
            case EvolutionMethod.Happiness:
                return pokemon.friendship >= evolution.requiredFriendship;
            
            case EvolutionMethod.Trade:
                // Trade evolution - would require different handling
                return false;
            
            default:
                return false;
        }
    }
    
    public void EvolvePokem(Pokemon pokemon, EvolutionData evolution)
    {
        if (!CanEvolve(pokemon, evolution))
        {
            Debug.Log($"{pokemon.species.pokemonName} cannot evolve yet!");
            return;
        }
        
        // Remove required item if applicable
        if (evolution.method == EvolutionMethod.Item)
        {
            Inventory.instance.RemoveItem(evolution.requiredItem.id, 1);
        }
        
        // Change species
        PokemonSpecies oldSpecies = pokemon.species;
        pokemon.species = evolution.evolvesInto;
        
        Debug.Log($"{oldSpecies.pokemonName} evolved into {pokemon.species.pokemonName}!");
        
        // Recalculate stats
        pokemon.CalculateStats();
    }
}
// Assets/Scripts/World/DialogSystem.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class DialogNode
{
    public string characterName;
    public string dialogText;
    public Sprite characterSprite;
    public DialogChoice[] choices;
}

[System.Serializable]
public class DialogChoice
{
    public string choiceText;
    public int nextNodeIndex;
    public int triggerEventIndex = -1;
}

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SereshProUGUI] private TextMeshProUGUI dialogText;
    [SerializeField] private Image characterImage;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private GameObject choiceButtonPrefab;
    
    private DialogNode[] dialogNodes;
    private int currentNodeIndex = 0;
    private bool isInDialog = false;
    
    private void Start()
    {
        dialogPanel.SetActive(false);
    }
    
    public void StartDialog(DialogNode[] nodes)
    {
        dialogNodes = nodes;
        currentNodeIndex = 0;
        isInDialog = true;
        dialogPanel.SetActive(true);
        ShowCurrentNode();
    }
    
    private void ShowCurrentNode()
    {
        if (currentNodeIndex >= dialogNodes.Length)
        {
            EndDialog();
            return;
        }
        
        DialogNode node = dialogNodes[currentNodeIndex];
        
        characterNameText.text = node.characterName;
        characterImage.sprite = node.characterSprite;
        
        StartCoroutine(TypeText(node.dialogText));
        ShowChoices(node.choices);
    }
    
    private IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        float typingSpeed = 0.05f;
        
        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
    private void ShowChoices(DialogChoice[] choices)
    {
        ClearChoices();
        
        if (choices == null || choices.Length == 0)
        {
            Button continueButton = Instantiate(choiceButtonPrefab, 
                choiceContainer).GetComponent<Button>();
            TextMeshProUGUI buttonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Continue";
            continueButton.onClick.AddListener(ContinueDialog);
        }
        else
        {
            foreach (DialogChoice choice in choices)
            {
                Button choiceButton = Instantiate(choiceButtonPrefab, 
                    choiceContainer).GetComponent<Button>();
                TextMeshProUGUI buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = choice.choiceText;
                
                choiceButton.onClick.AddListener(() => SelectChoice(choice));
            }
        }
    }
    
    private void SelectChoice(DialogChoice choice)
    {
        if (choice.triggerEventIndex >= 0)
        {
            // Trigger event (battle, item give, etc.)
        }
        
        currentNodeIndex = choice.nextNodeIndex;
        ShowCurrentNode();
    }
    
    private void ContinueDialog()
    {
        currentNodeIndex++;
        ShowCurrentNode();
    }
    
    private void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void EndDialog()
    {
        isInDialog = false;
        dialogPanel.SetActive(false);
    }
}
// Assets/Scripts/World/Trainer.cs
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    [SerializeField] private string trainerName;
    [SerializeField] private Sprite trainerSprite;
    [SerializeField] private List<Pokemon> team = new List<Pokemon>();
    [SerializeField] private int moneyReward;
    [SerializeField] private DialogNode[] dialogNodes;
    
    private bool hasBeenDefeated = false;
    
    public void InitializeTeam()
    {
        // Load team from scriptable object or create dynamically
    }
    
    public Pokemon GetRandomPokemon()
    {
        return team[Random.Range(0, team.Count)];
    }
    
    public List<Pokemon> GetTeam()
    {
        return team;
    }
    
    public void OnDefeat()
    {
        hasBeenDefeated = true;
        PlayerManager.instance.AddMoney(moneyReward);
        
        // Register all trainer's Pokemon as defeated
        foreach (Pokemon pokemon in team)
        {
            PokedexManager.instance.RegisterDefeat(pokemon.species);
        }
    }
    
    public void StartDialog()
    {
        DialogSystem dialog = FindObjectOfType<DialogSystem>();
        if (dialog != null)
            dialog.StartDialog(dialogNodes);
    }
    
    public bool HasBeenDefeated()
    {
        return hasBeenDefeated;
    }
    
    public string GetTrainerName()
    {
        return trainerName;
    }
}
// Assets/Scripts/Battle/BattleStatus.cs
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonStatus
{
    public StatusEffect effect;
    public int turnsRemaining;
    
    public PokemonStatus(StatusEffect eff, int turns)
    {
        effect = eff;
        turnsRemaining = turns;
    }
}

public enum StatusEffect
{
    None,
    Burn,
    Poison,
    Paralysis,
    Sleep,
    Freeze,
    Confusion
}

public class StatusManager
{
    private Dictionary<Pokemon, List<PokemonStatus>> statuses = 
        new Dictionary<Pokemon, List<PokemonStatus>>();
    
    public void ApplyStatus(Pokemon pokemon, StatusEffect effect, int turns = -1)
    {
        if (!statuses.ContainsKey(pokemon))
            statuses[pokemon] = new List<PokemonStatus>();
        
        // Prevent duplicate status
        foreach (var status in statuses[pokemon])
        {
            if (status.effect == effect)
                return;
        }
        
        statuses[pokemon].Add(new PokemonStatus(effect, turns));
        Debug.Log($"{pokemon.species.pokemonName} is now {effect}!");
    }
    
    public void RemoveStatus(Pokemon pokemon, StatusEffect effect)
    {
        if (statuses.ContainsKey(pokemon))
        {
            statuses[pokemon].RemoveAll(s => s.effect == effect);
        }
    }
    
    public void ClearAllStatuses(Pokemon pokemon)
    {
        if (statuses.ContainsKey(pokemon))
            statuses[pokemon].Clear();
    }
    
    public List<PokemonStatus> GetStatuses(Pokemon pokemon)
    {
        return statuses.ContainsKey(pokemon) ? statuses[pokemon] : new List<PokemonStatus>();
    }
    
    public bool HasStatus(Pokemon pokemon, StatusEffect effect)
    {
        if (!statuses.ContainsKey(pokemon))
            return false;
        
        return statuses[pokemon].Find(s => s.effect == effect) != null;
    }
    
    public void UpdateStatuses(Pokemon pokemon)
    {
        if (!statuses.ContainsKey(pokemon))
            return;
        
        for (int i = statuses[pokemon].Count - 1; i >= 0; i--)
        {
            if (statuses[pokemon][i].turnsRemaining > 0)
                statuses[pokemon][i].turnsRemaining--;
            else if (statuses[pokemon][i].turnsRemaining == 0)
                statuses[pokemon].RemoveAt(i);
        }
    }
    
    public float GetStatModifier(Pokemon pokemon, StatusEffect effect)
    {
        switch (effect)
        {
            case StatusEffect.Burn:
                return 0.5f; // 50% attack
            case StatusEffect.Paralysis:
                return 0.75f; // 75% speed
            default:
                return 1f;
        }
    }
}
// Assets/Scripts/Battle/EnhancedBattleManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancedBattleManager : MonoBehaviour
{
    public static EnhancedBattleManager instance;
    
    private Pokemon playerPokemon;
    private Pokemon enemyPokemon;
    private BattleState currentState;
    private StatusManager statusManager = new StatusManager();
    
    [SerializeField] private BattleUI battleUI;
    
    public delegate void BattleEventHandler();
    public event BattleEventHandler OnBattleStart;
    public event BattleEventHandler OnBattleEnd;
    public event BattleEventHandler OnRoundEnd;
    
    private int roundCount = 0;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    public void StartBattle(Pokemon player, Pokemon enemy)
    {
        playerPokemon = player;
        enemyPokemon = enemy;
        roundCount = 0;
        
        battleUI.UpdateBattleUI(playerPokemon, enemyPokemon);
        OnBattleStart?.Invoke();
        
        StartCoroutine(BattleRoutine());
    }
    
    private IEnumerator BattleRoutine()
    {
        while (playerPokemon.IsAlive() && enemyPokemon.IsAlive())
        {
            roundCount++;
            Debug.Log($"--- Round {roundCount} ---");
            
            // Determine turn order based on speed
            if (GetSpeed(playerPokemon) >= GetSpeed(enemyPokemon))
            {
                yield return StartCoroutine(PlayerTurn());
                if (!enemyPokemon.IsAlive()) break;
                
                yield return StartCoroutine(EnemyTurn());
                if (!playerPokemon.IsAlive()) break;
            }
            else
            {
                yield return StartCoroutine(EnemyTurn());
                if (!playerPokemon.IsAlive()) break;
                
                yield return StartCoroutine(PlayerTurn());
                if (!enemyPokemon.IsAlive()) break;
            }
            
            // Update statuses
            statusManager.UpdateStatuses(playerPokemon);
            statusManager.UpdateStatuses(enemyPokemon);
            
            OnRoundEnd?.Invoke();
            battleUI.UpdateHPBars();
            
            yield return new WaitForSeconds(1f);
        }
        
        EndBattle();
    }
    
    private IEnumerator PlayerTurn()
    {
        currentState = BattleState.PlayerTurn;
        // Wait for player input via UI
        yield return new WaitUntil(() => currentState != BattleState.PlayerTurn);
    }
    
    private IEnumerator EnemyTurn()
    {
        currentState = BattleState.EnemyTurn;
        yield return new WaitForSeconds(1f);
        
        Move move = SelectEnemyMove();
        if (move != null)
            ExecuteAttack(enemyPokemon, playerPokemon, move);
        
        yield return new WaitForSeconds(1f);
    }
    
    public void PlayerAttack(Move move)
    {
        if (!move.CanUse())
        {
            Debug.Log("No PP left!");
            return;
        }
        
        ExecuteAttack(playerPokemon, enemyPokemon, move);
        currentState = BattleState.EnemyTurn;
    }
    
    private void ExecuteAttack(Pokemon attacker, Pokemon defender, Move move)
    {
        // Check if attacker can move (paralysis, sleep, etc)
        if (statusManager.HasStatus(attacker, StatusEffect.Sleep))
        {
            Debug.Log($"{attacker.species.pokemonName} is asleep and can't move!");
            return;
        }
        
        // Check accuracy
        if (Random.Range(0, 100) > move.accuracy)
        {
            Debug.Log($"{attacker.species.pokemonName} used {move.moveName} but it missed!");
            move.UsePP();
            return;
        }
        
        int damage = CalculateDamage(attacker, defender, move);
        defender.TakeDamage(damage);
        move.UsePP();
        
        Debug.Log($"{attacker.species.pokemonName} used {move.moveName}! Dealt {damage} damage!");
        
        // Apply status effects from move
        ApplyMoveEffects(move, defender);
        
        battleUI.UpdateHPBars();
    }
    
    private void ApplyMoveEffects(Move move, Pokemon target)
    {
        // Each move can have secondary effects
        switch (move.moveName)
        {
            case "Thunder Wave":
                if (Random.Range(0, 100) < 75)
                    statusManager.ApplyStatus(target, StatusEffect.Paralysis, -1);
                break;
            case "Toxic":
                if (Random.Range(0, 100) < 90)
                    statusManager.ApplyStatus(target, StatusEffect.Poison, -1);
                break;
            case "Ember":
                if (Random.Range(0, 100) < 10)
                    statusManager.ApplyStatus(target, StatusEffect.Burn, -1);
                break;
        }
    }
    
    private int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
    {
        float baseDamage = 0;
        
        float attackStat = attacker.stats.attack;
        float defenseStat = defender.stats.defense;
        
        // Apply status modifiers
        if (statusManager.HasStatus(attacker, StatusEffect.Burn) && 
            move.category == MoveCategory.Physical)
        {
            attackStat *= statusManager.GetStatModifier(attacker, StatusEffect.Burn);
        }
        
        if (move.category == MoveCategory.Physical)
        {
            baseDamage = ((2 * attacker.level / 5f + 2) * move.power * 
                         (attackStat / (float)defenseStat) / 50f + 2);
        }
        else if (move.category == MoveCategory.Special)
        {
            baseDamage = ((2 * attacker.level / 5f + 2) * move.power * 
                         (attacker.stats.spAtk / (float)defender.stats.spDef) / 50f + 2);
        }
        
        // Type effectiveness
        float effectiveness = GetTypeEffectiveness(move.type, defender.species.type1);
        if (defender.species.type2 != PokemonType.Normal)
            effectiveness *= GetTypeEffectiveness(move.type, defender.species.type2);
        
        baseDamage *= effectiveness;
        baseDamage *= Random.Range(0.85f, 1f);
        
        return Mathf.Max(1, (int)baseDamage);
    }
    
    private int GetSpeed(Pokemon pokemon)
    {
        int speed = pokemon.stats.speed;
        
        if (statusManager.HasStatus(pokemon, StatusEffect.Paralysis))
        {
            speed = (int)(speed * 0.75f);
        }
        
        return speed;
    }
    
    private Move SelectEnemyMove()
    {
        List<Move> validMoves = new List<Move>();
        
        foreach (Move move in enemyPokemon.moves)
        {
            if (move.CanUse())
                validMoves.Add(move);
        }
        
        return validMoves.Count > 0 ? validMoves[Random.Range(0, validMoves.Count)] : null;
    }
    
    private float GetTypeEffectiveness(PokemonType attackType, PokemonType defendType)
    {
        // Type matchup chart
        Dictionary<(PokemonType, PokemonType), float> typeChart = 
            new Dictionary<(PokemonType, PokemonType), float>
        {
            // Fire attacks
            { (PokemonType.Fire, PokemonType.Grass), 2f },
            { (PokemonType.Fire, PokemonType.Ice), 2f },
            { (PokemonType.Fire, PokemonType.Bug), 2f },
            { (PokemonType.Fire, PokemonType.Steel), 2f },
            { (PokemonType.Fire, PokemonType.Water), 0.5f },
            { (PokemonType.Fire, PokemonType.Ground), 0.5f },
            { (PokemonType.Fire, PokemonType.Rock), 0.5f },
            
            // Water attacks
            { (PokemonType.Water, PokemonType.Fire), 2f },
            { (PokemonType.Water, PokemonType.Ground), 2f },
            { (PokemonType.Water, PokemonType.Rock), 2f },
            { (PokemonType.Water, PokemonType.Grass), 0.5f },
            { (PokemonType.Water, PokemonType.Electric), 0.5f },
            
            // Electric attacks
            { (PokemonType.Electric, PokemonType.Water), 2f },
            { (PokemonType.Electric, PokemonType.Flying), 2f },
            { (PokemonType.Electric, PokemonType.Grass), 0.5f },
            
            // Grass attacks
            { (PokemonType.Grass, PokemonType.Water), 2f },
            { (PokemonType.Grass, PokemonType.Ground), 2f },
            { (PokemonType.Grass, PokemonType.Rock), 2f },
            { (PokemonType.Grass, PokemonType.Fire), 0.5f },
            { (PokemonType.Grass, PokemonType.Grass), 0.5f },
            { (PokemonType.Grass, PokemonType.Poison), 0.5f },
            { (PokemonType.Grass, PokemonType.Flying), 0.5f },
        };
        
        var key = (attackType, defendType);
        return typeChart.ContainsKey(key) ? typeChart[key] : 1f;
    }
    
    private void EndBattle()
    {
        if (playerPokemon.IsAlive())
        {
            Debug.Log("Player wins!");
            
            // Award experience
            int baseExp = 100; // Would come from species data
            int expGain = (baseExp * enemyPokemon.level / 7);
            playerPokemon.AddExperience(expGain);
        }
        else
        {
            Debug.Log("Player lost!");
        }
        
        OnBattleEnd?.Invoke();
    }
}
// Assets/Scripts/Pokemon/ExperienceSystem.cs
using UnityEngine;

[System.Serializable]
public class ExperienceCurve
{
    public int[] requiredExp = new int[100];
    
    public void GenerateCurve(GrowthType type)
    {
        for (int i = 1; i < 100; i++)
        {
            switch (type)
            {
                case GrowthType.Fast:
                    requiredExp[i] = (int)(0.8f * i * i * i);
                    break;
                case GrowthType.MediumFast:
                    requiredExp[i] = (int)(i * i * i);
                    break;
                case GrowthType.MediumSlow:
                    requiredExp[i] = (int)(1.2f * i * i * i - 15 * i * i + 100 * i - 140);
                    break;
                case GrowthType.Slow:
                    requiredExp[i] = (int)(1.25f * i * i * i);
                    break;
            }
        }
    }
}

public enum GrowthType
{
    Fast,
    MediumFast,
    MediumSlow,
    Slow
}

public partial class Pokemon
{
    private ExperienceCurve expCurve;
    
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"{species.pokemonName} gained {amount} EXP!");
        
        CheckLevelUp();
    }
    
    private void CheckLevelUp()
    {
        while (level < 100 && experience >= expCurve.requiredExp[level])
        {
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        level++;
        currentHP = stats.hp; // Full heal on level up
        
        // Increase stats
        CalculateStats();
        
        Debug.Log($"{species.pokemonName} grew to level {level}!");
        
        // Check if moves can be learned
        LearnNewMoves();
    }
    
    private void LearnNewMoves()
    {
        // Check move pool for moves that can be learned at this level
    }
    
    public void CalculateStats()
    {
        stats.hp = (2 * species.baseStats.hp + 0 + 0 / 4) * level / 100 + level + 5;
        stats.attack = (2 * species.baseStats.attack + 0 + 0 / 4) * level / 100 + 5;
        stats.defense = (2 * species.baseStats.defense + 0 + 0 / 4) * level / 100 + 5;
        stats.spAtk = (2 * species.baseStats.spAtk + 0 + 0 / 4) * level / 100 + 5;
        stats.spDef = (2 * species.baseStats.spDef + 0 + 0 / 4) * level / 100 + 5;
        stats.speed = (2 * species.baseStats.speed + 0 + 0 / 4) * level / 100 + 5;
        
        if (currentHP > stats.hp)
            currentHP = stats.hp;
    }
}
// Assets/Scripts/Core/SaveSystem.cs
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public string playerName;
    public int money;
    public List<PokemonData> teamData;
    public List<PokedexEntryData> pokedexData;
    public List<InventorySlotData> inventoryData;
}

[System.Serializable]
public class PokemonData
{
    public int speciesId;
    public int level;
    public int experience;
    public int currentHP;
    public List<string> moveNames;
    public int friendship;
}

[System.Serializable]
public class PokedexEntryData
{
    public int speciesId;
    public bool seen;
    public bool caught;
}

[System.Serializable]
public class InventorySlotData
{
    public int itemId;
    public int quantity;
}

public class SaveSystem : MonoBehaviour
{
    private string savePath;
    
    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
    }
    
    public void SaveGame()
    {
        GameData data = new GameData();
        
        // Collect player data
        data.playerName = PlayerManager.instance.GetPlayerName();
        data.money = PlayerManager.instance.GetMoney();
        
        // Collect team data
        data.teamData = new List<PokemonData>();
        foreach (Pokemon pokemon in PlayerManager.instance.GetTeam())
        {
            data.teamData.Add(ConvertPokemonToData(pokemon));
        }
        
        // Collect Pokédex data
        data.pokedexData = new List<PokedexEntryData>();
        foreach (var entry in PokedexManager.instance.GetAllEntries())
        {
            data.pokedexData.Add(new PokedexEntryData
            {
                speciesId = entry.species.id,
                seen = entry.seen,
                caught = entry.caught
            });
        }
        
        // Collect inventory data
        data.inventoryData = new List<InventorySlotData>();
        foreach (var slot in Inventory.instance.GetAllItems())
        {
            data.inventoryData.Add(new InventorySlotData
            {
                itemId = slot.item.id,
                quantity = slot.quantity
            });
        }
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        
        Debug.Log($"Game saved to {savePath}");
    }
    
    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Save file not found!");
            return;
        }
        
        string json = File.ReadAllText(savePath);
        GameData data = JsonUtility.FromJson<GameData>(json);
        
        // Load player data
        PlayerManager.instance.SetPlayerName(data.playerName);
        PlayerManager.instance.SetMoney(data.money);
        
        // Load team
        PlayerManager.instance.ClearTeam();
        foreach (PokemonData pokemonData in data.teamData)
        {
            Pokemon pokemon = ConvertDataToPokemon(pokemonData);
            PlayerManager.instance.AddPokemonToTeam(pokemon);
        }
        
        // Load Pokédex
        foreach (PokedexEntryData entryData in data.pokedexData)
        {
            // Load entry data
        }
        
        // Load inventory
        Inventory.instance.ClearInventory();
        foreach (InventorySlotData slotData in data.inventoryData)
        {
            Item item = ItemDatabase.GetItem(slotData.itemId);
            Inventory.instance.AddItem(item, slotData.quantity);
        }
        
        Debug.Log("Game loaded!");
    }
    
    private PokemonData ConvertPokemonToData(Pokemon pokemon)
    {
        PokemonData data = new PokemonData();
        data.speciesId = pokemon.species.id;
        data.level = pokemon.level;
        data.experience = pokemon.experience;
        data.currentHP = pokemon.currentHP;
        data.friendship = pokemon.friendship;
        
        data.moveNames = new List<string>();
        foreach (Move move in pokemon.moves)
        {
            data.moveNames.Add(move.moveName);
        }
        
        return data;
    }
    
    private Pokemon ConvertDataToPokemon(PokemonData data)
    {
        PokemonSpecies species = Resources.Load<PokemonSpecies>($"Pokemon/{data.speciesId}");
        Pokemon pokemon = new Pokemon(species, data.level);
        pokemon.experience = data.experience;
        pokemon.currentHP = data.currentHP;
        pokemon.friendship = data.friendship;
        
        return pokemon;
    }
}
// Assets/Scripts/World/GymLeader.cs
using System.Collections.Generic;
using UnityEngine;

public class GymLeader : Trainer
{
    [SerializeField] private Sprite badgeSprite;
    [SerializeField] private int badgeId;
    [SerializeField] private PokemonType gymType;
    [SerializeField] private int expRewardMultiplier = 2;
    
    private bool gymComplete = false;
    
    public void OnDefeat()
    {
        base.OnDefeat();
        
        gymComplete = true;
        PlayerManager.instance.AddMoney(moneyReward * 2);
        PlayerManager.instance.AddBadge(badgeId, badgeSprite);
        
        Debug.Log($"You earned the {gymType} Badge!");
    }
    
    public PokemonType GetGymType()
    {
        return gymType;
    }
    
    public bool IsGymComplete()
    {
        return gymComplete;
    }
    
    public Sprite GetBadgeSprite()
    {
        return badgeSprite;
    }
}
// Assets/Scripts/Core/GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState currentGameState;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        currentGameState = GameState.Exploring;
    }
    
    public void StartNewGame(string playerName)
    {
        PlayerManager.instance.SetPlayerName(playerName);
        PlayerManager.instance.SetMoney(500);
        
        // Give starter Pokémon
        // Load world scene
        SceneManager.LoadScene("World");
    }
    
    public void SetGameState(GameState state)
    {
        currentGameState = state;
    }
    
    public GameState GetGameState()
    {
        return currentGameState;
    }
}

public enum GameState
{
    MainMenu,
    Exploring,
    InBattle,
    InMenu,
    InDialog,
    InGym
}
// Assets/Scripts/UI/StarterSelectionUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarterSelectionUI : MonoBehaviour
{
    [SerializeField] private PokemonSpecies[] starters;
    [SerializeField] private Transform starterContainer;
    [SerializeField] private GameObject starterButtonPrefab;
    [SerializeField] private Image selectedImage;
    [SerializeField] private TextMeshProUGUI selectedNameText;
    [SerializeField] private Button confirmButton;
    
    private PokemonSpecies selectedStarter;
    
    private void Start()
    {
        DisplayStarters();
    }
    
    private void DisplayStarters()
    {
        foreach (PokemonSpecies starter in starters)
        {
            GameObject button = Instantiate(starterButtonPrefab, starterContainer);
            Button btn = button.GetComponent<Button>();
            Image img = button.GetComponent<Image>();
            
            img.sprite = starter.sprite;
            btn.onClick.AddListener(() => SelectStarter(starter));
        }
    }
    
    private void SelectStarter(PokemonSpecies starter)
    {
        selectedStarter = starter;
        selectedImage.sprite = starter.sprite;
        selectedNameText.text = starter.pokemonName;
    }
    
    public void ConfirmSelection()
    {
        if (selectedStarter != null)
        {
            Pokemon starterPokemon = new Pokemon(selectedStarter, 5);
            PlayerManager.instance.AddPokemonToTeam(starterPokemon);
            PokedexManager.instance.RegisterCatch(starterPokemon);
            
            GameManager.instance.StartNewGame("Player");
        }
    }
}
