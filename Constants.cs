class Weapon
{
    private int _bullets;
    private const int _minimalAmmo = 0;
    private const int _bulletsRate = 1;
    

    public bool CanShoot() => _bullets > _minimalAmmo;

    public void Shoot() => _bullets -= _bulletsRate;
}