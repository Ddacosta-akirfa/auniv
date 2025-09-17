using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auniv.Enums;

namespace Auniv.Models;

public class Universidade
{
    [Key]
    public long IdUniversidade { get; set; }
    [Required(ErrorMessage = "O nome da universidade é um campo requerido")]
    [MaxLength(150, ErrorMessage = "Máximo de caracteres permitidos são 150")]
    [MinLength(20, ErrorMessage = "Mínimo de caracteres são 20")]
    public required string Nome { get; set; } = string.Empty;
    [MaxLength(15)]
    public string Sigla { get; set; } = string.Empty;
    [Required(ErrorMessage = "O nome do responsável curso é um campo requerido")]
    [MaxLength(100, ErrorMessage = "Máximo de caracteres permitidos são 100")]
    [MinLength(15, ErrorMessage = "Mínimo de caracteres são 15")]
    public string Decano { get; set; } = string.Empty;
    public required Localizacao Localizacao { get; set; }
    public DateOnly DataFundacao { get; set; }
    public ETipoUniversidade Tipo { get; set; }
    public EStatusUniversidade Status { get; set; }
    public long NumeroEstudantes { get; set; }
    [MaxLength(75)]
    public string SiteOficial { get; set; } = string.Empty;
    public virtual ICollection<Departamento> Departamentos { get; set; } = [];

    [NotMapped]
    public int NumeroCursos => Departamentos?.SelectMany(d => d.Cursos).Count() ?? 0;
}