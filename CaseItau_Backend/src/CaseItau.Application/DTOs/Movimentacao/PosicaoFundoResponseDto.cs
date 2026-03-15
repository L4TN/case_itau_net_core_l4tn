namespace CaseItau.Application.DTOs.Movimentacao;

public class PosicaoFundoResponseDto
{
    public int Id { get; set; }
    public int IdFundo { get; set; }
    public DateTime DtPosicao { get; set; }
    public decimal VlrPatrimonio { get; set; }
}
