using System;
using System.ComponentModel.DataAnnotations;

namespace zad5.CustomValidators;

public class PeselValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        string pesel = value.ToString();

        if (pesel.Length != 11 || !long.TryParse(pesel, out _))
            return false;

        int sum = 0;
        int[] weights = new int[] { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

        for (int i = 0; i < 10; i++)
        {
            sum += (pesel[i] - '0') * weights[i];
        }

        int checksum = (10 - (sum % 10)) % 10;

        return pesel[10] - '0' == checksum;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"Numer PESEL '{name}' jest niepoprawny.";
    }
}
