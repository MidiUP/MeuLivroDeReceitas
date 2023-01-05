using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Ingrediente;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class IngredienteRepositorio : IIngredienteWriteOnlyRepository
{
    private readonly MeuLivroDeReceitasContext _context;

    public IngredienteRepositorio(MeuLivroDeReceitasContext context)
    {
        _context = context;
    }

    public async Task AdicionarIngredientes(List<Ingrediente> ingredientes)
    {
        await _context.Ingredientes.AddRangeAsync(ingredientes);
    }
}
