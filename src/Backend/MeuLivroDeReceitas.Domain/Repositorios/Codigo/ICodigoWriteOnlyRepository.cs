namespace MeuLivroDeReceitas.Domain.Repositorios.Codigo
{
    public interface ICodigoWriteOnlyRepository
    {
        Task Registrar(Entidades.Codigos codigo);
        Task Deletar(long usuarioId);
    }
}
