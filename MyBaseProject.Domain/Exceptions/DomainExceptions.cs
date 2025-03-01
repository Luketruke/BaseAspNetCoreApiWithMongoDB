namespace MyBaseProject.Domain.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base($"An account with the email {email} already exists.") { }
    }

    public class InvalidEmailFormatException : Exception
    {
        public InvalidEmailFormatException()
            : base("The provided email format is invalid.") { }
    }

    public class InvalidIdFormatException : Exception
    {
        public InvalidIdFormatException(string id)
            : base($"The provided ID '{id}' is not a valid MongoDB ObjectId.") { }
    }

    public class RequiredPasswordException : Exception
    {
        public RequiredPasswordException()
            : base("Password is required.") { }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity, string id)
            : base($"No {entity} found with ID {id}.") { }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid credentials. Please check your email and password.") { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email)
            : base($"No account found for the provided email: {email}") { }
    }
    public class ErrorWhileProcessingException : Exception
    {
        public ErrorWhileProcessingException(string type)
            : base($"An error occurred while processing the {type}.") { }

        public ErrorWhileProcessingException(string type, Exception innerException)
            : base($"An error occurred while processing the {type}.", innerException) { }
    }

    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message) { }

        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DatabaseWriteException : Exception
    {
        public DatabaseWriteException(string message) : base(message) { }

        public DatabaseWriteException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class MessageNotFoundException : Exception
    {
        public MessageNotFoundException(string messageId)
            : base($"No message found with ID {messageId}.") { }
    }
}
