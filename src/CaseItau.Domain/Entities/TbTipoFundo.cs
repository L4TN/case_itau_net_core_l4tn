namespace CaseItau.Domain.Entities;

public class TbTipoFundo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<TbFundo> Fundos { get; set; } = new List<TbFundo>();
}
