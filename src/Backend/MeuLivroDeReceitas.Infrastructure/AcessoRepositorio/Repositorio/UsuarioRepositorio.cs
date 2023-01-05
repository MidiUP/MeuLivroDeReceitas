using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class UsuarioRepositorio : IUsuarioWriteOnlyRepositorio, IUsuarioReadOnlyRepositorio, IUsuarioUpdateOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _context;
    public UsuarioRepositorio(MeuLivroDeReceitasContext context)
    {
        _context = context;
    }
    public async Task Adcionar(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<bool> ExisteUsuarioComEmail(string email)
    {
        return await _context.Usuarios.AnyAsync(c => c.Email.Equals(email));
    }

    public async Task<Usuario> RecuperarPorEmail(string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email));
    }

    public async Task<Usuario> RecuperarPorEmailSenha(string email, string senha)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Senha.Equals(senha));
    }

    public async Task<Usuario> RecuperarPorId(long id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == (id));
    }

    public void Update(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
    }
}
