using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace MeuLivroDeReceitas.Api.Filtros;

public class FiltroDasExceptions : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MeuLivroDeReceitasException)
        {
            TratarMeuLivroDeReceitasException(context);
        } 
        else
        {
            LancarErroDesconhecido(context);
        }
    }

    private static void TratarMeuLivroDeReceitasException(ExceptionContext context)
    {
        if (context.Exception is ErrosDeValidacaoException)
        {
            TratarErroDeValidacaoException(context);
        } else if (context.Exception is LoginInvalidoException)
        {
            TratarErroDeLoginException(context);
        } else if(context.Exception is UsuarioSemPermissaoException)
        {
            TratarUsuarioSemPermissaoException(context);
        }
    }

    private static void TratarErroDeValidacaoException(ExceptionContext context)
    {
        var erroDeValidacaoException = context.Exception as ErrosDeValidacaoException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new RespostaErroJson(erroDeValidacaoException.MensagensDeErro));
    }

    private static void TratarErroDeLoginException(ExceptionContext context)
    {
        var erroDeLoginException = context.Exception as LoginInvalidoException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new RespostaErroJson(erroDeLoginException.Message));
    }
    private static void TratarUsuarioSemPermissaoException(ExceptionContext context)
    {
        var erroDeLoginException = context.Exception as UsuarioSemPermissaoException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new RespostaErroJson(erroDeLoginException.Message));
    }

    private static void LancarErroDesconhecido(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new RespostaErroJson(ResourceMensagensDeErro.ERRO_DESCONHECIDO));
    }
}
