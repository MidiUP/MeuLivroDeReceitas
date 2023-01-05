using MeuLivroDeReceitas.Domain.Repositorios;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;

public sealed class UnidadeDeTrabalho : IDisposable, IUnidadeDeTrabalho
{
    private readonly MeuLivroDeReceitasContext _context;
    private bool _disposed;

    public UnidadeDeTrabalho(MeuLivroDeReceitasContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing) 
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}
