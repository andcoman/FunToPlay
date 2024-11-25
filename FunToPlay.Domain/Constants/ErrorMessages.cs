namespace FunToPlay.Domain.Constants;

public class ErrorMessages
{
    public const string PlayerNotFound = "Player not found.";
    public const string PlayerAlreadyConnected = "Player is already connected.";
    public const string PlayerNotConnected = "Player is not connected.";
    public const string ResourceNotFound = "Resource of type {0} does not exist.";
    public const string NegativeResourceValue = "Resource value cannot be negative.";
    public const string InsufficientResources = "Cannot decrease resource below zero.";
}