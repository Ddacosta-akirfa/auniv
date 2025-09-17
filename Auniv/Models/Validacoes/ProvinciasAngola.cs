namespace auniv.Models.Validacoes;

public static class ProvinciasAngola
{
    public static readonly HashSet<string> Todas = new(StringComparer.OrdinalIgnoreCase)
    {
        "Bengo", "Benguela", "Bié", "Cabinda", "Cuando Cubango", "Cuanza Norte", "Cuanza Sul",
        "Cunene", "Huambo", "Huíla", "Luanda", "Lunda Norte", "Lunda Sul", "Malanje",
        "Moxico", "Namibe", "Uíge", "Zaire"
    };

    public static bool EhValida(string provincia) => Todas.Contains(provincia);
}
