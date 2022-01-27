class Player
{
    public string Name { get; private set; }
    public int Age { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public Weapon Weapon { get; private set; }

    public void Move()
    {
        Movement.Move();
    }

    public void Attack()
    {
        Weapon.Shoot();
    }

    public bool IsReloading()
    {
        return Weapon.IsReloading();
    }
}

class Weapon
{
    public int Damage { get; private set; }

    public void Shoot()
    {
        //attack
    }

    public bool IsReloading()
    {
        throw new NotImplementedException();
    }
}

class CharacterMovement
{
    public float Speed { get; private set; }
    public Vector2 Direction { get; private set; }

    public void Move()
    {
        //Do move
    }
}

class Vector2
{
    public int X { get; private set; }
    public int Y { get; private set; }
}