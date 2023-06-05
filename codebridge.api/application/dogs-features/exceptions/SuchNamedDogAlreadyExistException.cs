namespace codebridge.api.application.dogs_features.exceptions;

public class SuchNamedDogAlreadyExistException : Exception
{
    public SuchNamedDogAlreadyExistException(string dogsName) : base($"Dog with such name already exists. Dog's Name : {dogsName} ")
    {
    }
}
