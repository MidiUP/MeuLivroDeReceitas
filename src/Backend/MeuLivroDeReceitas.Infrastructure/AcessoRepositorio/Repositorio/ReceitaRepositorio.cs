using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio, IReceitaReadOnlyRepositorio, IReceitaUpdateOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _context;

    public ReceitaRepositorio(MeuLivroDeReceitasContext context)
    {
        _context = context;
    }

    public async Task<Receita> Adicionar(Receita receita)
    {
       await _context.Receitas.AddAsync(receita);
       return receita;
    }

    async Task<Receita> IReceitaReadOnlyRepositorio.GetById(long idReceita)
    {
        var receita = await _context.Receitas
            .AsNoTracking()
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.Id == idReceita);
        return receita;
    }
    async Task<Receita> IReceitaUpdateOnlyRepositorio.GetById(long idReceita)
    {
        var receita = await _context.Receitas
            .Include(r => r.Ingredientes)
            .FirstOrDefaultAsync(r => r.Id == idReceita);
        return receita;
    }

    public async Task<IList<Receita>> GetByUsuario(long userId)
    {
        return await _context.Receitas.Where(r => r.UsuarioId == userId)
            .AsNoTracking()
            .Include(r => r.Ingredientes)
            .ToListAsync();
    }

    public void Update(Receita receita)
    {
        _context.Receitas.Update(receita);
    }

    public async Task Deletar(long idReceita)
    {
        var receita = await _context.Receitas.FirstOrDefaultAsync(receita => receita.Id == idReceita);
        _context.Receitas.Remove(receita);
    }
}
