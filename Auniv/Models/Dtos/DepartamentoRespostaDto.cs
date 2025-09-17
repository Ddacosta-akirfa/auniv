using Auniv.Models;

namespace auniv.Models.Dtos;

public class DepartamentoRespostaDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Chefe { get; set; } = string.Empty;
    // public long IdUniversidade { get; set; }
    public string NomeUniversidade { get; set; } = string.Empty;
    public ICollection<string> Cursos { get; set; } = [];
}