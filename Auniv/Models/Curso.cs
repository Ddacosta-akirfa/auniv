using System.ComponentModel.DataAnnotations;
using Auniv.Enums;

namespace Auniv.Models;

public class Curso
{
    [Key]
    public long IdCurso { get; set; }
    [Required(ErrorMessage = "O nome do curso é um campo requerido")]
    [MaxLength(45,ErrorMessage = "Máximo de caracteres permitidos são 45")]
    [MinLength(5, ErrorMessage = "Mínimo de caracteres são 5")]
    public string Nome { get; set; } = string.Empty;
    public string Duracao { get; set; } = string.Empty;
    public ENivelCurso Nivel { get; set; }
    [Required(ErrorMessage = "O nome do responsável do curso é um campo requerido")]
    [MaxLength(100,ErrorMessage = "Máximo de caracteres permitidos são 100")]
    [MinLength(10, ErrorMessage = "Mínimo de caracteres são 10")]
    public string Responsavel { get; set; } = string.Empty;
    public long IdDepartamento { get; set; }
    public Departamento Departamento { get; set; } = default!;
}