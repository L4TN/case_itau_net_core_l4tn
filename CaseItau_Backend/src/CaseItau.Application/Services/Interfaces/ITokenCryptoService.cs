namespace CaseItau.Application.Services.Interfaces;

public interface ITokenCryptoService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}
