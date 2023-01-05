using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class CodigoRepositorio : ICodigoWriteOnlyRepository, ICodigoReadOnlyRepository
{
    private readonly MeuLivroDeReceitasContext _context; 

    public CodigoRepositorio(MeuLivroDeReceitasContext context)
    {
        _context = context;  
    }

    public async Task Deletar(long usuarioId)
    {
        var codigos = await _context.Codigos.Where(c => c.UsuarioId == usuarioId).ToListAsync();
        if(codigos.Any() )
        {
            _context.Codigos.RemoveRange(codigos);
        }
    }

    public async Task<Codigos> RecuperarEntidadeCodigo(string codigo)
    {
        return await _context.Codigos.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Codigo.Equals(codigo));
    }

    public async Task Registrar(Codigos codigo)
    {
        var codigoDb = await _context.Codigos.FirstOrDefaultAsync(c => c.UsuarioId == codigo.UsuarioId);
        if(codigoDb is not null)
        {
            codigoDb.Codigo = codigo.Codigo;
            _context.Codigos.Update(codigoDb);
            return;
        }
        await _context.Codigos.AddAsync(codigo);
    }
}
