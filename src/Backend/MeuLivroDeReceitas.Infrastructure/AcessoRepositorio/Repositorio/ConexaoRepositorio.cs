using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

internal class ConexaoRepositorio : IConexaoReadOnlyRepositorio, IConexaoWriteOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _context;

    public ConexaoRepositorio(MeuLivroDeReceitasContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteConexao(long usuarioA, long usuarioB)
    {
        var conexao = await _context.Conexoes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => (c.UsuarioId == usuarioA && c.ConectadoComUsuarioId == usuarioB) || (c.UsuarioId == usuarioB && c.ConectadoComUsuarioId == usuarioA));

        return conexao is not null;
    }

    public async Task Registrar(Conexao conexao)
    {
        await _context.Conexoes.AddAsync(conexao);
    }
}
