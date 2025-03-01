using zad5.CustomValidators;

public class UserModel
{
    [PeselValidation(ErrorMessage = "Numer PESEL jest niepoprawny.")]
    public string Pesel { get; set; }

    [AlphanumericWithPolishLetters(ErrorMessage = "Tylko litery, cyfry i białe znaki są dozwolone.")]
    public string FullName { get; set; }
}
