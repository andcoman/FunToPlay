using FunToPlay.Domain.Constants;
using FunToPlay.Domain.Entities;
using FunToPlay.Domain.Utils;

namespace FunToPlay.Domain.ValueObjects
{
    public class Resource
    {
        public ResourceType Type { get; private set; }
        public int Value { get; private set; }

        public Resource(ResourceType type, int value)
        {
            Type = type;
            Value = value;
        }
        
        public Resource(string type, int value)
        {
            Type = Enum.Parse<ResourceType>(type);
            Value = value;
        }

        public OperationResult<bool> Increase(int amount)
        {
            if (amount < 0)
                return OperationResult.Failure<bool>(ErrorMessages.NegativeResourceValue);

            Value += amount;
            return OperationResult.Success(true);
        }

        public OperationResult<bool> Decrease(int amount)
        {
            if (amount < 0)
                return OperationResult.Failure<bool>(ErrorMessages.NegativeResourceValue);

            if (amount > Value)
                return OperationResult.Failure<bool>(ErrorMessages.InsufficientResources);

            Value -= amount;
            return OperationResult.Success(true);
        }
    }
}