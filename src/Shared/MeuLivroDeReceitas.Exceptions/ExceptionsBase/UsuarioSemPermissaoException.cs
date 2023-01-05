namespace MeuLivroDeReceitas.Exceptions.ExceptionsBase;

public class UsuarioSemPermissaoException : MeuLivroDeReceitasException
{

    public UsuarioSemPermissaoException() : base(ResourceMensagensDeErro.USUARIO_SEM_PERMISSAO)
    {
    }

}
