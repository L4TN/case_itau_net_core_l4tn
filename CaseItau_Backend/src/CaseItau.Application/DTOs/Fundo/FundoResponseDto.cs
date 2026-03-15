namespace CaseItau.Application.DTOs.Fundo;

public class FundoResponseDto
{
    public int Id { get; set; }
    public string CdFundo { get; set; } = string.Empty;
    public string NmFundo { get; set; } = string.Empty;
    public string NrCnpj { get; set; } = string.Empty;
    public int IdTipoFundo { get; set; }
    public string NmTipoFundo { get; set; } = string.Empty;
    public decimal? VlrPatrimonio { get; set; }
}
