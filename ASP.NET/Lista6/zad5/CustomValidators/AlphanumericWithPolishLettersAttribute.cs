using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace zad5.CustomValidators;

public class AlphanumericWithPolishLettersAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null )
            return false;

        string input = value.ToString();

        // Regular expression to match Latin letters, Polish letters, digits, and white spaces
        string pattern = @"^[a-zA-ZąćęłńóśżźĄĆĘŁŃÓŚŻŹ0-9]+$";

        return Regex.IsMatch(input, pattern);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"Pole {name} zawiera niedozwolone znaki. Dozwolone są tylko litery (łacińskie i polskie), cyfry oraz białe znaki.";
    }
}
