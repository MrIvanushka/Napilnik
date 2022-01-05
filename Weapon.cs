using System;

class Weapon : IReadonlyWeapon
{
    public int Damage { get; private set; }
    public int Bullets { get; private set; }

    public Weapon(int damage, int bullets)
    {
        if (damage <= 0 || bullets < 0)
            throw new ArgumentOutOfRangeException();

        Damage = damage;
        Bullets = bullets;
    }

    public void Fire(Player player)
    {
        player.TakeDamage(Damage);
        Bullets -= 1;
    }
}

interface IReadonlyWeapon
{
    int Damage { get; }
    int Bullets { get; }
}

class Player
{
    public int _health { get; private set; }

    public Player(int health)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException();

        _health = health;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        _health -= damage;
    }
}

class Bot
{
    private Weapon _weapon;

    public IReadonlyWeapon Weapon => _weapon;

    public Bot(Weapon weapon)
    {
        _weapon = new Weapon(weapon.Damage, weapon.Bullets);
    }

    public void OnSeePlayer(Player player)
    {
        if(player._health <= 0)
            throw new InvalidOperationException();

        _weapon.Fire(player);
    }
}