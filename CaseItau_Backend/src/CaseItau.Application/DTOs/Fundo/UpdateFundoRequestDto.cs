namespace CaseItau.Application.DTOs.Fundo;

public class UpdateFundoRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public int TipoFundoId { get; set; }
}
