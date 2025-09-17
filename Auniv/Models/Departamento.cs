using System.ComponentModel.DataAnnotations;

namespace Auniv.Models;

public class Departamento
{
    [Key]
    public long IdDepartamento { get; set; }
    [Required(ErrorMessage = "O nome do departamento é um campo requerido")]
    [MaxLength(50,ErrorMessage = "Máximo de caracteres permitidos são 45")]
    [MinLength(5, ErrorMessage = "Mínimo de caracteres são 5")]
    public string Nome { get; set; } = string.Empty;
    [Required(ErrorMessage = "O nome do responsável do departamento é um campo requerido")]
    [MaxLength(100,ErrorMessage = "Máximo de caracteres permitidos são 100")]
    [MinLength(10, ErrorMessage = "Mínimo de caracteres são 10")]
    public string Responsavel { get; set; } = string.Empty;
    public long IdUniversidade { get; set; }
    public Universidade Universidade { get; set; } = default!;
    public ICollection<Curso> Cursos { get; set; } = [];
}