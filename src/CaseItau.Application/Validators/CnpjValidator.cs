namespace CaseItau.Application.Validators;

public static class CnpjValidator
{
    public static bool IsValid(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

        if (cnpj.Length != 14)
            return false;

        if (!cnpj.All(char.IsDigit))
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        var digits = cnpj.Select(c => c - '0').ToArray();

        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum1 = 0;
        for (var i = 0; i < 12; i++)
            sum1 += digits[i] * weights1[i];

        var remainder1 = sum1 % 11;
        var check1 = remainder1 < 2 ? 0 : 11 - remainder1;

        if (digits[12] != check1)
            return false;

        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum2 = 0;
        for (var i = 0; i < 13; i++)
            sum2 += digits[i] * weights2[i];

        var remainder2 = sum2 % 11;
        var check2 = remainder2 < 2 ? 0 : 11 - remainder2;

        return digits[13] == check2;
    }
}
