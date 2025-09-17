using Auniv.Enums;

namespace Auniv.Models.Dtos;

public class VerCursosDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Duracao { get; set; } = string.Empty;
    public ENivelCurso Nivel { get; set; }
    public string Responsavel { get; set; } = string.Empty;
}