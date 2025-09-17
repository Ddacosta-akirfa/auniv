using Auniv.Models;

namespace auniv.Models.Dtos;

public class CriarDepartamentosDto
{
    public string Nome { get; set; } = string.Empty;
    public string Chefe { get; set; } = string.Empty;
    public long IdUniversidade { get; set; }
    // public string NomeUniversidade { get; set; } = string.Empty;
}