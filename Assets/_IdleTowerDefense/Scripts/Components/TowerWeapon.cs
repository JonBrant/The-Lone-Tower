public struct TowerWeapon
{
    // Attack Damage
    public float BaseAttackDamage;
    public float AttackDamage;
    public float AttackDamageMultiplier;
    public float AttackDamageAdditions;

    // ToDo: Use multiplier/additions here as well
    // AttackCooldown
    public float BaseAttackCooldown;
    public float AttackCooldown;
    public float AttackCooldownMultiplier;
    public float AttackCooldownRemaining;

    public void RecalculateAttackDamage()
    {
        AttackDamage = BaseAttackDamage * AttackDamageMultiplier + AttackDamageAdditions;
    }

    public void RecalculateAttackCooldown()
    {
        AttackCooldown = BaseAttackCooldown * AttackCooldownMultiplier;
    }
}