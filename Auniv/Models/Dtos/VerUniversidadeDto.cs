using auniv.Models.Dtos;
using Auniv.Enums;

namespace Auniv.Models.Dtos;

public class VerUniversidadeDto
{
    public long Id { get; set; }
    public required string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public string Decano { get; set; } = string.Empty;
    public DateOnly DataFundacao { get; set; }
    public ETipoUniversidade Tipo { get; set; }
    public EStatusUniversidade Status { get; set; }
    public long NumeroEstudantes { get; set; }
    public string Site { get; set; } = string.Empty;
    public virtual ICollection<Departamento> Departamentos { get; set; } = [];
    public int QtdCursos { get; set; }
    public LocalizacaoDto Localizacao { get; set; } = new();
}