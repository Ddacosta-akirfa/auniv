using Auniv.Enums;
using Microsoft.EntityFrameworkCore;

namespace Auniv.Models;

[Owned]
public class Localizacao
{
    public string Municipio { get; set; } = string.Empty;
    public string Provincia { get; set; } = string.Empty;
}