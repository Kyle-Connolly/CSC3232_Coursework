using System.Collections.Generic;

public static class AttackSet
{
    public static readonly AttackType Inferno = new AttackType("Inferno", 50, 3, "inferno+hydro");
    public static readonly AttackType Hydro = new AttackType("Hydro", 25, 1, "inferno+hydro");
    public static readonly AttackType Tempest = new AttackType("Tempest", 75, 5, "none");
    public static readonly AttackType InfernoFlash = new AttackType("Inferno Flash", 90, 8, "inferno+vectorlaunch");

    public static readonly Dictionary<string, AttackType> Attacks = new Dictionary<string, AttackType>
    {
        { "Inferno", Inferno },
        { "Hydro", Hydro },
        { "Tempest", Tempest },
        { "Inferno Flash", InfernoFlash },
    };
}
