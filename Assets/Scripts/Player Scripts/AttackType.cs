public struct AttackType
{
    public string name { get; }
    public int damage { get; }
    public int elementalChargeCost { get; }
    public string comboSynergy { get; }

    public AttackType(string attackName, int damageVal, int staminaCost, string comboAttack)
    {
        name = attackName;
        damage = damageVal;
        elementalChargeCost = staminaCost;
        comboSynergy = comboAttack;
    }

    public override string ToString()
    {
        return $"Attack: {name}, Damage: {damage}, Cost: {elementalChargeCost}, Effect: {comboSynergy}";
    }
}

