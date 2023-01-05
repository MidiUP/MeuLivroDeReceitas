using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
using MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;
using MeuLivroDeReceitas.Application.UseCases.Dashboard;
using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
using MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;
using MeuLivroDeReceitas.Application.UseCases.Receita.Recuperar;
using MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infrastructure;

public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarChaveAdicionalSenha(services, configuration);
        AdicionarTokenJwt(services, configuration);
        AdicionarHashIds(services, configuration);
        AdicionarUseCases(services);
        AdicionarUsuarioLogado(services);
    }

    private static void AdicionarUsuarioLogado(IServiceCollection services)
    {
        services.AddScoped<IUsuarioLogado, UsuarioLogado>();
    }


    private static void AdicionarChaveAdicionalSenha(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Configuracoes:Senha:ChaveAdicionalSenha");

        services.AddScoped(option => new EncriptadorDeSenha(section.Value));
    }

    private static void AdicionarTokenJwt(IServiceCollection services, IConfiguration configuration)
    {
        var sectionTempoDeVidaToken = configuration.GetRequiredSection("Configuracoes:Jwt:TempoDeVidaTokenMinutos");
        var sectionChaveToken = configuration.GetRequiredSection("Configuracoes:Jwt:ChaveToken");

        services.AddScoped(option => new TokenContoller(int.Parse(sectionTempoDeVidaToken.Value), sectionChaveToken.Value));
    }

    private static void AdicionarHashIds(IServiceCollection services, IConfiguration configuration)
    {
        var salt = configuration.GetRequiredSection("Configuracoes:HashIds:Salt");
        services.AddHashids(setup =>
        {
            setup.Salt = salt.Value;
            setup.MinHashLength = 3;
        });
    }

    private static void AdicionarUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>()
            .AddScoped<ILoginUseCase, LoginUseCase>()
            .AddScoped<IAlterarSenhaUseCase, AlterarSenhaUseCase>()
            .AddScoped<IRegistrarReceitaUseCase, RegistrarReceitaUseCase>()
            .AddScoped<IRecuperarPorUsuarioUseCase, RecuperarPorUsuarioUseCase>()
            .AddScoped<IDashboardUseCase, DashboardUseCase>()
            .AddScoped<IRecuperarPorIdUsecase, RecuperarPorIdUsecase>()
            .AddScoped<IAtualizarReceitaUseCase, AtualizarReceitaUseCase>()
            .AddScoped<IDeletarReceitaUseCase, DeletarReceitaUseCase>()
            .AddScoped<IGerarQRCodeUseCase, GerarQRCodeUseCase>()
            .AddScoped<IQRCodeLidoUseCase, QRCodeLidoUseCase>()
            .AddScoped<IAceitarConexaoUseCase, AceitarConexaoUseCase>()
            .AddScoped<IRecusarConexaoUseCase, RecusarConexaoUseCase>();

    }    
}
