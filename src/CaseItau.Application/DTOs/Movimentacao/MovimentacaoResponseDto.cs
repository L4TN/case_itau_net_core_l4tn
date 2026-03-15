namespace CaseItau.Application.DTOs.Movimentacao;

public class MovimentacaoResponseDto
{
    public int Id { get; set; }
    public int IdFundo { get; set; }
    public DateTime DtMovimentacao { get; set; }
    public decimal VlrMovimentacao { get; set; }
}
