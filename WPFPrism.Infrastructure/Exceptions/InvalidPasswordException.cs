namespace WPFPrism.Infrastructure.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string password) : base($"Пароль \"{password}\" не сменить.") { }
    }
}
