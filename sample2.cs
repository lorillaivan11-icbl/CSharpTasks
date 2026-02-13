// Basic Pokémon structure
public class Pokemon
{
    public string name;
    public int level;
    public int health;
    public int maxHealth;
    public List<Move> moves;
    public PokemonType type;
    public Dictionary<string, int> stats; // HP, ATK, DEF, etc.
}

public class Battle
{
    public Pokemon playerPokemon;
    public Pokemon enemyPokemon;
    
    public void PlayerAttack(Move move)
    {
        float effectiveness = CalculateTypeAdvantage();
        int damage = CalculateDamage(move, effectiveness);
        enemyPokemon.TakeDamage(damage);
    }
}
