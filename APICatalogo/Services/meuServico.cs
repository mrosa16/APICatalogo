namespace APICatalogo.Services;

public class MeuServico : ImeuServico
{
    public string Saudacao(string nome)
    {
        return $"Bem-Vindo, {nome} \n \n {DateTime.UtcNow}";

    }
}

