using Auniv.Enums;
using Auniv.Models;

namespace Auniv.Data;

public static class DbSeeders
{
    public static void SeedData(AppDbContext dbContext)
    {
        #region Universidades

        if (dbContext.Universidades.Any()) return;

        var universidades = new List<Universidade>
        {
             new ()
            {
                Nome = "Universidade José Eduardo dos Santos",
                Sigla = "UJES",
                Decano = "Prof. Doutor Pedro Magalhães",
                Localizacao = new Localizacao
                {
                    Municipio = "Huambo",
                    Provincia = "Huambo"
                },
                DataFundacao = new DateOnly(2012, 8, 12),
                Tipo = ETipoUniversidade.Publica,
                Status = EStatusUniversidade.Ativa,
                NumeroEstudantes = 13500,
                SiteOficial = "www.ujes.edu.ao"
            },
            new ()
            {
                Nome = "Universidade Agostinho Neto",
                Sigla = "UAN",
                Decano = "Prof. Doutora Maria Fernandes",
                Localizacao = new Localizacao
                {
                    Municipio = "Luanda",
                    Provincia = "Luanda"
                },
                DataFundacao = new DateOnly(1991, 4, 15),
                Tipo = ETipoUniversidade.Publica,
                Status = EStatusUniversidade.Ativa,
                NumeroEstudantes = 350000,
                SiteOficial = "www.uan.ao"
            },
             new ()
            {
                Nome = "Universidade Católica de Angola",
                Sigla = "UCAN",
                Decano = "Pe. Doutor Jorge Heitor",
                Localizacao = new Localizacao
                {
                    Municipio = "Luanda",
                    Provincia = "Luanda"
                },
                DataFundacao = new DateOnly(1997, 7, 22),
                Tipo = ETipoUniversidade.Privada,
                Status = EStatusUniversidade.Ativa,
                NumeroEstudantes = 12000,
                SiteOficial = "www.ucan.ao"
            }
        };

        dbContext.Universidades.AddRange(universidades);
        dbContext.SaveChanges();

        #endregion
        #region Semente - Departamentos

        var departamentos = new List<Departamento>
        {
            // UJES
             new ()
            {
                Nome = "Ciências Naturais",
                Responsavel = "Prof. Doutor Carlos Silva",
                IdUniversidade = universidades[0].IdUniversidade
            },
             new ()
            {
                Nome = "Engenharias",
                Responsavel = "Eng. Doutora Ana Paula",
                IdUniversidade = universidades[0].IdUniversidade
            },

            // UAN
             new ()
            {
                Nome = "Medicina",
                Responsavel = "Prof. Doutor Manuel Sousa",
                IdUniversidade = universidades[1].IdUniversidade
            },
            new ()
            {
                Nome = "Ciências Sociais e Humanas",
                Responsavel = "Dra. Joana Mendes",
                IdUniversidade = universidades[1].IdUniversidade
            },
            
            // UCAN
             new ()
            {
                Nome = "Teologia",
                Responsavel = "Pe. Doutor António Costa",
                IdUniversidade = universidades[2].IdUniversidade
            },
            new ()
            {
                Nome = "Economia e Gestão",
                Responsavel = "Prof. Doutor Rui Almeida",
                IdUniversidade = universidades[2].IdUniversidade
            }
        };

        dbContext.Departamentos.AddRange(departamentos);
        dbContext.SaveChanges();

        #endregion
        #region Semente - Cursos

        var cursos = new List<Curso>
        {
            new() {
                Nome = "Biologia",
                Duracao = "4 anos",
                Nivel = ENivelCurso.Licenciatura,
                Responsavel = "Prof. Doutora Sara Pinto",
                IdDepartamento = departamentos[0].IdDepartamento
            },
            new ()
            {
                Nome = "Química",
                Duracao = "4 anos",
                Nivel = ENivelCurso.Licenciatura,
                Responsavel = "Prof. Doutor José Monteiro",
                IdDepartamento = departamentos[0].IdDepartamento
            },

            new ()
            {
                Nome = "Engenharia Civil",
                Duracao = "5 anos",
                Nivel = ENivelCurso.Licenciatura,
                Responsavel = "Eng. Doutor Miguel Ângelo",
                IdDepartamento = departamentos[1].IdDepartamento
            },
            
            // Medicina - UAN
            new ()
            {
                Nome = "Medicina Geral",
                Duracao = "6 anos",
                Nivel = ENivelCurso.Mestrado,
                Responsavel = "Prof. Doutora Carla Santos",
                IdDepartamento = departamentos[2].IdDepartamento
            },
            new ()
            {
                Nome = "Direito Público",
                Duracao = "4 anos",
                Nivel = ENivelCurso.Licenciatura,
                Responsavel = "Dr. Manuel João",
                IdDepartamento = departamentos[3].IdDepartamento
            },
            new ()
            {
                Nome = "Teologia Pastoral",
                Duracao = "4 anos",
                Nivel = ENivelCurso.Licenciatura,
                Responsavel = "Pe. Doutor Lucas Mateus",
                IdDepartamento = departamentos[4].IdDepartamento
            },
            new ()
            {
                Nome = "Gestão de Empresas",
                Duracao = "4 anos",
                Nivel = ENivelCurso.Licenciatura,
                Responsavel = "Prof. Doutora Teresa Alves",
                IdDepartamento = departamentos[5].IdDepartamento
            }
        };

        dbContext.Cursos.AddRange(cursos);
        dbContext.SaveChanges();

        #endregion
    }
}