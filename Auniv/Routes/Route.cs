using auniv.Models.Dtos;
using auniv.Models.Validacoes;
using Auniv.Data;
using Auniv.Enums;
using Auniv.Models;
using Auniv.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auniv.Routes;

public static class Route
{
    public static void Routes(this WebApplication app)
    {
        var routes = app.MapGroup("auniv");

        routes.MapGet("nos", () => "AUNIV para todos");

        #region Endpoints para Cursos

        routes.MapGet("cursos", async (AppDbContext dbContext) =>
        {
            var cursos = await dbContext.Cursos
                .Select(c => new VerCursosDto()
                {
                    Id = c.IdCurso,
                    Nome = c.Nome,
                    Duracao = c.Duracao,
                    Nivel = c.Nivel,
                    Responsavel = c.Responsavel
                })
                .ToListAsync();
            return Results.Ok(cursos);
        });

        routes.MapGet("cursos/{id:long}", async (AppDbContext dbContext, long id) =>
        {
            var curso = await dbContext.Cursos
                .Select(c => new VerCursosDto()
                {
                    Id = c.IdCurso,
                    Nome = c.Nome,
                    Duracao = c.Duracao,
                    Nivel = c.Nivel,
                    Responsavel = c.Responsavel
                })
                .Where(c => c.Id == id)
                .ToListAsync();

            return Results.Ok(curso);
        });
        /*
            - GET /api/cursos/{id} Retorna detalhes de um curso específico.
            - GET /api/cursos/universidade/{universidadeId} Lista cursos de uma universidade específica.
            - GET /api/cursos/categoria/{categoriaId} Lista cursos de uma categoria específica.
            - POST /api/cursos Adiciona um novo curso.
            - PUT /api/cursos/{id} Atualiza informações de um curso.
            - DELETE /api/cursos/{id} Remove um curso.
        */
        routes.MapDelete("cursos/{id:long}", async (AppDbContext dbContext, long id) =>
        {
            var cursoApagado = await dbContext.Cursos.FindAsync(id);

            if (cursoApagado is null)
                return Results.NotFound("ERD_1 - Curso não encontrado");

            dbContext.Remove(cursoApagado);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        #endregion

        #region Endpoints para Departamentos

        routes.MapGet("departamentos", async (AppDbContext dbContext) =>
        {
            var departamentos = await dbContext.Departamentos
                .Include(d => d.Universidade)
                .Include(d => d.Cursos)
                .AsNoTracking()
                .Select(d => new VerDepartamentosDto
                {
                    Id = d.IdUniversidade,
                    Nome = d.Nome,
                    Chefe = d.Responsavel,
                    NomeUniversidade = d.Universidade.Nome,
                    Cursos = d.Cursos.Select(c => c.Nome).ToList()
                }).ToListAsync();

            return Results.Ok(departamentos);
        });

        routes.MapGet("departamentos/{id:long}", async (long id, AppDbContext dbContext) =>
        {
            var departamentoEncontrado = await dbContext.Departamentos
                .Include(d => d.Universidade)
                .Include(d => d.Cursos)
                .AsNoTracking()
                .Where(d => d.IdDepartamento == id)
                .Select(d => new VerDepartamentosDto
                {
                    Id = d.IdUniversidade,
                    Nome = d.Nome,
                    Chefe = d.Responsavel,
                    NomeUniversidade = d.Universidade.Nome,
                    Cursos = d.Cursos.Select(c => c.Nome).ToList()
                }).ToListAsync();

            return departamentoEncontrado is null ? Results.NotFound($"DEP_NOTF_001 - Departamento com id {id} não encontrado") : Results.Ok(departamentoEncontrado);
        });

        routes.MapGet("departamentos/universidade/{IdUniversidade:long}", async (long IdUniversidade, AppDbContext dbContext) =>
                {
                    var departamentoEncontrado = await dbContext.Departamentos
                        .AsNoTracking()
                        .Where(d => d.Universidade.IdUniversidade == IdUniversidade)
                        .Select(d => new VerDepartamentosDto
                        {
                            Id = d.IdUniversidade,
                            Nome = d.Nome,
                            Chefe = d.Responsavel,
                            NomeUniversidade = d.Universidade.Nome
                        })
                        .ToListAsync();

                    if (departamentoEncontrado is null)
                        return Results.NotFound($"DEP_NOTF_003 - Não foi encontrado nenhum Departamento na universidade com id {IdUniversidade}");

                    return Results.Ok(departamentoEncontrado);
                });

        routes.MapGet("departamentos/universidade/{sigla}", async (string sigla, AppDbContext dbContext) =>
            {
                var departamentoEncontrado = await dbContext.Departamentos
                    .AsNoTracking()
                    .Where(d => d.Universidade.Sigla == sigla)
                    .Select(d => new VerDepartamentosDto
                    {
                        Id = d.IdUniversidade,
                        Nome = d.Nome,
                        Chefe = d.Responsavel,
                        NomeUniversidade = d.Universidade.Nome
                    })
                    .ToListAsync();

                if (departamentoEncontrado is null)
                    return Results.NotFound($"DEP_NOTF_004 - Não foi encontrado nenhum Departamento na universidade {sigla}");

                return Results.Ok(departamentoEncontrado);
            });

        routes.MapPost("departamentos", async (CriarDepartamentosDto criarDepartamento, AppDbContext dbContext) =>
        {
            var universidadeExiste = await dbContext.Universidades
                .AnyAsync(u => u.IdUniversidade == criarDepartamento.IdUniversidade);

            if (!universidadeExiste)
                return Results.BadRequest($"[DEP_VAL_001] Universidade com Id = {criarDepartamento.IdUniversidade} não encontrada");

            var departamentoDuplicado = await dbContext.Departamentos
                .AnyAsync(d => d.IdDepartamento == criarDepartamento.IdUniversidade && d.Nome == criarDepartamento.Nome);

            if (departamentoDuplicado)
                return Results.Conflict($"[DEP_VAL_002] Já existe um departamento '{criarDepartamento.Nome}' nesta universidade");

            var novoDepartamento = new Departamento
            {
                Nome = criarDepartamento.Nome,
                IdUniversidade = criarDepartamento.IdUniversidade,
                Responsavel = criarDepartamento.Chefe
            };

            await dbContext.Departamentos.AddAsync(novoDepartamento);
            await dbContext.SaveChangesAsync();

            await dbContext.Entry(novoDepartamento)
                .Reference(d => d.Universidade)
                .LoadAsync();

            await dbContext.Entry(novoDepartamento)
                .Collection(d => d.Cursos)
                .LoadAsync();

            var respostaDepartamento = new DepartamentoRespostaDto
            {
                Id = novoDepartamento.IdDepartamento,
                Nome = novoDepartamento.Nome,
                Chefe = novoDepartamento.Responsavel,
                NomeUniversidade = novoDepartamento.Universidade.Nome,
                Cursos = novoDepartamento.Cursos.Select(c => c.Nome).ToList()
            };

            return Results.Created($"/departamentos/{novoDepartamento.IdDepartamento}", respostaDepartamento);
        });

        routes.MapPut("departamentos/{id:long}", async (long id, CriarDepartamentosDto actualizarDepartamento, AppDbContext dbContext) =>
            {
                var departamentoNaBd = await dbContext.Departamentos
                    .Include(d => d.Universidade)
                    .FirstOrDefaultAsync(d => d.IdDepartamento == id);

                if (departamentoNaBd is null)
                    return Results.NotFound($"[DEP_VAL_003] Departamento com Id = {id} não encontrado");

                var universidadeExiste = await dbContext.Universidades
                    .AnyAsync(u => u.IdUniversidade == actualizarDepartamento.IdUniversidade);

                if (!universidadeExiste)
                    return Results.BadRequest($"[DEP_VAL_001] Universidade com Id = {actualizarDepartamento.IdUniversidade} não encontrada");

                var departamentoDuplicado = await dbContext.Departamentos
                    .AnyAsync(d => d.IdDepartamento != id
                                && d.Nome == actualizarDepartamento.Nome
                                && d.IdUniversidade == actualizarDepartamento.IdUniversidade);

                if (departamentoDuplicado)
                    return Results.Conflict($"[DEP_VAL_002] Já existe um departamento '{actualizarDepartamento.Nome}' nesta universidade");

                departamentoNaBd.Nome = actualizarDepartamento.Nome;
                departamentoNaBd.Responsavel = actualizarDepartamento.Chefe;
                departamentoNaBd.IdUniversidade = actualizarDepartamento.IdUniversidade;

                await dbContext.SaveChangesAsync();

                var resposta = new DepartamentoRespostaDto
                {
                    Id = departamentoNaBd.IdDepartamento,
                    Nome = departamentoNaBd.Nome,
                    Chefe = departamentoNaBd.Responsavel,
                    NomeUniversidade = departamentoNaBd.Universidade?.Nome ?? "",
                    Cursos = departamentoNaBd.Cursos.Select(c => c.Nome).ToList()
                };

                return Results.NoContent();
            });

        routes.MapDelete("departamentos/{id:long}", async (long id, AppDbContext dbContext) =>
        {
            var departamentoEncontrado = await dbContext.Departamentos
                .Include(d => d.Cursos)
                .Where(d => d.IdDepartamento == id)
                .ExecuteDeleteAsync();

            if (departamentoEncontrado == 0)
                return Results.NotFound($"[DEP_NOTF_002] - Departamento com id {id} não encontrado");

            return Results.NoContent();
        });

        #endregion

        #region Endpoints para Universidades

        routes.MapGet("universidades", async (AppDbContext dbContext) =>
        {
            var todasUniversidades = await dbContext.Universidades
                .AsNoTracking()
                .Select(u => new VerUniversidadeDto
                {
                    Id = u.IdUniversidade,
                    DataFundacao = u.DataFundacao,
                    Nome = u.Nome,
                    Localizacao = new LocalizacaoDto
                    {
                        Municipio = u.Localizacao.Municipio,
                        Provincia = u.Localizacao.Provincia
                    },
                    QtdCursos = u.Departamentos
                        .SelectMany(d => d.Cursos)
                        .Count(),
                    Sigla = u.Sigla,
                    Site = u.SiteOficial,
                    Decano = u.Decano,
                    Tipo = u.Tipo,
                    Status = u.Status,
                    NumeroEstudantes = u.NumeroEstudantes
                })
                .ToListAsync();

            return Results.Ok(todasUniversidades);
        });

        routes.MapGet("universidades/{id:long}", async (long id, AppDbContext dbContext) =>
        {
            var universidadeEncontrada = await dbContext.Universidades
                .AsNoTracking()
                .Select(u => new VerUniversidadeDto
                {
                    Id = u.IdUniversidade,
                    DataFundacao = u.DataFundacao,
                    Nome = u.Nome,
                    Localizacao = new LocalizacaoDto
                    {
                        Municipio = u.Localizacao.Municipio,
                        Provincia = u.Localizacao.Provincia
                    },
                    QtdCursos = u.Departamentos
                        .SelectMany(d => d.Cursos)
                        .Count(),
                    Sigla = u.Sigla,
                    Site = u.SiteOficial,
                    Decano = u.Decano,
                    Tipo = u.Tipo,
                    Status = u.Status,
                    NumeroEstudantes = u.NumeroEstudantes
                })
                .Where(u => u.Id == id)
                .ToListAsync();

            if (universidadeEncontrada is null)
                return Results.NotFound("UNI_NOTF_001 - Universidade não encontrada");

            return Results.Ok(universidadeEncontrada);
        });

        routes.MapGet("universidades/{sigla}", async (string sigla, AppDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(sigla))
                return Results.BadRequest("UNI_VAL_001 - A sigla não pode ser vazia");


            var universidadeEncontrada = await dbContext.Universidades
                .AsNoTracking()
                .Select(u => new VerUniversidadeDto
                {
                    Id = u.IdUniversidade,
                    DataFundacao = u.DataFundacao,
                    Nome = u.Nome,
                    Localizacao = new LocalizacaoDto
                    {
                        Municipio = u.Localizacao.Municipio,
                        Provincia = u.Localizacao.Provincia
                    },
                    QtdCursos = u.Departamentos
                        .SelectMany(d => d.Cursos)
                        .Count(),
                    Sigla = u.Sigla,
                    Site = u.SiteOficial,
                    Decano = u.Decano,
                    Tipo = u.Tipo,
                    Status = u.Status,
                    NumeroEstudantes = u.NumeroEstudantes
                }).Where(u => u.Sigla.Equals(sigla)).ToListAsync();

            if (universidadeEncontrada is null)
                return Results.NotFound($"UNI_NOTF_002 - Universidade com a sigla {sigla} não encontrada");

            return Results.Ok(universidadeEncontrada);
        });

        routes.MapGet("universidades/localizacao", async (string provincia, AppDbContext dbContext) =>
                {
                    if (string.IsNullOrWhiteSpace(provincia))
                        return Results.BadRequest("UNI_VAL_002 - A província deve ser informada");

                    if (!ProvinciasAngola.EhValida(provincia))
                        return Results.BadRequest(
                            $"UNI_VAL_003 - {provincia}' não é válida. " +
                            $"Escolha uma das seguintes: {string.Join(", ", ProvinciasAngola.Todas)}");

                    var universidadesEncontradas = await dbContext.Universidades
                        .AsNoTracking()
                        .Where(u => u.Localizacao.Provincia == provincia)
                        .Select(u => new VerUniversidadeDto
                        {
                            Id = u.IdUniversidade,
                            DataFundacao = u.DataFundacao,
                            Nome = u.Nome,
                            Localizacao = new LocalizacaoDto
                            {
                                Municipio = u.Localizacao.Municipio,
                                Provincia = u.Localizacao.Provincia
                            },
                            QtdCursos = u.Departamentos
                                .SelectMany(d => d.Cursos)
                                .Count(),
                            Sigla = u.Sigla,
                            Site = u.SiteOficial,
                            Decano = u.Decano,
                            Tipo = u.Tipo,
                            Status = u.Status,
                            NumeroEstudantes = u.NumeroEstudantes
                        }).ToListAsync();

                    if (universidadesEncontradas.Count == 0)
                        return Results.NotFound($"UNI_NOTF_003 - Nenhuma universidade encontrada na província '{provincia}'");

                    return Results.Ok(universidadesEncontradas);
                });

        routes.MapGet("universidades/{id:long}/departamentos", async (long id, AppDbContext dbContext) =>
{
    var universidade = await dbContext.Universidades
        .Include(u => u.Departamentos)
        .ThenInclude(d => d.Cursos)
        .FirstOrDefaultAsync(u => u.IdUniversidade == id);

    if (universidade is null)
        return Results.NotFound($"[UNI_VAL_003] Universidade com Id = {id} não encontrada");

    var departamentos = universidade.Departamentos
        .Select(d => new DepartamentoRespostaDto
        {
            Id = d.IdDepartamento,
            Nome = d.Nome,
            Chefe = d.Responsavel,
            NomeUniversidade = universidade.Nome,
            Cursos = d.Cursos.Select(c => c.Nome).ToList()
        })
        .ToList();

    return Results.Ok(departamentos);
});


        routes.MapPost("universidades", async (AppDbContext dbContext, CriarUniversidadeDto criarUniversidade) =>
        {
            if (string.IsNullOrWhiteSpace(criarUniversidade.Nome) || string.IsNullOrWhiteSpace(criarUniversidade.Sigla))
                return Results.BadRequest("UNI_VAL_004 - Nome e Sigla são obrigatórios");

            var existeUniversidade = await dbContext.Universidades.AnyAsync(u => u.Sigla == criarUniversidade.Sigla);
            if (existeUniversidade)
                return Results.Conflict($"UNI_VAL_005 - Já existe uma universidade com a sigla '{criarUniversidade.Sigla}'");

            if (!ProvinciasAngola.EhValida(criarUniversidade.Localizacao.Provincia))
                return Results.BadRequest(
                    $"UNI_VAL_005 - A província '{criarUniversidade.Localizacao.Provincia}' não é válida. " +
                    $"Escolha uma das seguintes: {string.Join(", ", ProvinciasAngola.Todas)}");

            var universidadeCriada = new Universidade
            {
                Nome = criarUniversidade.Nome,
                Sigla = criarUniversidade.Sigla,
                Decano = criarUniversidade.Decano,
                DataFundacao = criarUniversidade.DataFundacao,
                Tipo = criarUniversidade.Tipo,
                NumeroEstudantes = criarUniversidade.NumeroEstudantes,
                SiteOficial = criarUniversidade.SiteOficial,
                Localizacao = new Localizacao
                {
                    Provincia = criarUniversidade.Localizacao.Provincia,
                    Municipio = criarUniversidade.Localizacao.Municipio
                }
            };

            await dbContext.Universidades.AddAsync(universidadeCriada);
            await dbContext.SaveChangesAsync();

            var respostaUniversidade = new UniversidadeRespostaDto
            {
                Id = universidadeCriada.IdUniversidade,
                DataFundacao = universidadeCriada.DataFundacao,
                Nome = universidadeCriada.Nome,
                Localizacao = new LocalizacaoDto
                {
                    Municipio = universidadeCriada.Localizacao.Municipio,
                    Provincia = universidadeCriada.Localizacao.Provincia
                },
                QtdCursos = universidadeCriada.Departamentos
                                .SelectMany(d => d.Cursos)
                                .Count(),
                Sigla = universidadeCriada.Sigla,
                Site = universidadeCriada.SiteOficial,
                Decano = universidadeCriada.Decano,
                Tipo = universidadeCriada.Tipo,
                Status = universidadeCriada.Status,
                NumeroEstudantes = universidadeCriada.NumeroEstudantes
            };

            return Results.Created($"/auniv/universidades/{universidadeCriada.IdUniversidade}", respostaUniversidade);
        });

        routes.MapPut("universidades/{id:long}", async (long id, AppDbContext dbContext, ActualizarUniversidadeDto universidadeActualizada) =>
        {
            var universidadeNaBd = await dbContext.Universidades
                .Include(u => u.Localizacao)
                .FirstOrDefaultAsync(u => u.IdUniversidade == id);

            if (universidadeNaBd is null)
                return Results.NotFound($"UNI_NOTF_004 - Universidade com ID {id} não encontrada");

            if (!ProvinciasAngola.EhValida(universidadeActualizada.Localizacao.Provincia))
                return Results.BadRequest(
                    $"UNI_VAL_006 - A província '{universidadeActualizada.Localizacao.Provincia}' não é válida. " +
                    $"Escolha: {string.Join(", ", ProvinciasAngola.Todas)}");

            universidadeNaBd.Nome = universidadeActualizada.Nome;
            universidadeNaBd.Sigla = universidadeActualizada.Sigla;
            universidadeNaBd.DataFundacao = universidadeActualizada.DataFundacao;
            universidadeNaBd.SiteOficial = universidadeActualizada.SiteOficial;
            universidadeNaBd.Decano = universidadeActualizada.Decano;
            universidadeNaBd.Tipo = universidadeActualizada.Tipo;
            universidadeNaBd.Status = universidadeActualizada.Status;
            universidadeNaBd.NumeroEstudantes = universidadeActualizada.NumeroEstudantes;
            universidadeNaBd.Localizacao.Municipio = universidadeActualizada.Localizacao.Municipio;
            universidadeNaBd.Localizacao.Provincia = universidadeActualizada.Localizacao.Provincia;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        routes.MapPut("universidades/{sigla}", async (string sigla, AppDbContext dbContext, ActualizarUniversidadeDto universidadeActualizada) =>
        {
            if (string.IsNullOrWhiteSpace(sigla))
                return Results.BadRequest("UNI_VAL_007 - A sigla deve ser informada");

            var universidadeNaBd = await dbContext.Universidades
                  .Include(u => u.Localizacao)
                  .FirstOrDefaultAsync(u => u.Sigla == sigla);

            if (universidadeNaBd is null)
                return Results.NotFound($"UNI_NOTF_005 - Universidade com sigla '{sigla}' não encontrada");

            if (!ProvinciasAngola.EhValida(universidadeActualizada.Localizacao.Provincia))
                return Results.BadRequest(
                    $"A província '{universidadeActualizada.Localizacao.Provincia}' não é válida. " +
                    $"Escolha: {string.Join(", ", ProvinciasAngola.Todas)}");

            universidadeNaBd.Nome = universidadeActualizada.Nome;
            universidadeNaBd.DataFundacao = universidadeActualizada.DataFundacao;
            universidadeNaBd.SiteOficial = universidadeActualizada.SiteOficial;
            universidadeNaBd.Decano = universidadeActualizada.Decano;
            universidadeNaBd.Tipo = universidadeActualizada.Tipo;
            universidadeNaBd.Status = universidadeActualizada.Status;
            universidadeNaBd.NumeroEstudantes = universidadeActualizada.NumeroEstudantes;
            universidadeNaBd.Localizacao.Municipio = universidadeActualizada.Localizacao.Municipio;
            universidadeNaBd.Localizacao.Provincia = universidadeActualizada.Localizacao.Provincia;

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        routes.MapDelete("universidades/{id:long}", async (long id, AppDbContext dbContext) =>
        {
            var registrosApagados = await dbContext.Universidades
                .Where(u => u.IdUniversidade == id)
                .ExecuteDeleteAsync();

            if (registrosApagados == 0)
                return Results.NotFound("UNI_NOTF_006 - Universidade não encontrada");

            return Results.NoContent();
        });

        routes.MapDelete("universidades/{sigla}", async (string sigla, AppDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(sigla))
            {
                return Results.BadRequest("UNI_VAL_008 - A sigla não pode ser vazia");
            }

            var registrosApagados = await dbContext.Universidades
                .Where(u => u.Sigla.Equals(sigla))
                .ExecuteDeleteAsync();

            if (registrosApagados == 0)
                return Results.NotFound("UNI_NOTF_007 - Universidade com a sigla {sigla} não encontrada");

            return Results.NoContent();
        });

        #endregion

        #region Outros Endpoints

        /*
        - Buscar cursos por duração e nível:
            - GET /api/cursos?duracao<=4&nivel=graduacao  - Lista cursos de graduação com duração de até 4 anos.
        - Estatísticas:
            - GET /api/universidades/estatisticas
        Retorna estatísticas gerais, como número de universidades, estudantes, cursos, etc.
        */

        #endregion
    }
}