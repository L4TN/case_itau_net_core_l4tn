namespace CaseItau.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string entity, object key)
        : base($"{entity} com chave '{key}' não foi encontrado(a).") { }
}
