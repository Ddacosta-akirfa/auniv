using Auniv.Enums;
using Auniv.Models;

namespace auniv.Models.Dtos;

public class ActualizarUniversidadeDto
{
    public required string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public string Decano { get; set; } = string.Empty;
    public DateOnly DataFundacao { get; set; }
    public ETipoUniversidade Tipo { get; set; }
    public EStatusUniversidade Status { get; set; }
    public long NumeroEstudantes { get; set; }
    public string SiteOficial { get; set; } = string.Empty;

    public LocalizacaoDto Localizacao { get; set; } = new();
}