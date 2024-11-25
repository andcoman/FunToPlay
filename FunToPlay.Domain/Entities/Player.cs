using FunToPlay.Domain.Constants;
using FunToPlay.Domain.Utils;
using FunToPlay.Domain.ValueObjects;

namespace FunToPlay.Domain.Entities
{
    public class Player
    {
        public Guid PlayerId { get; private set; }
        public string DeviceId { get; private set; }
        public bool IsConnected { get; private set; }
        public List<Resource> Resources { get; private set; }
        public Player(Guid playerId, string deviceId)
        {
            PlayerId = playerId;
            DeviceId = deviceId;
            Resources = new List<Resource>();
        }

        public void Login()
        {
            if (IsConnected)
            {
                return;
            }

            PlayerId = Guid.NewGuid();
            IsConnected = true;
        }

        public OperationResult<bool> Logout()
        {
            if (!IsConnected)
                return OperationResult.Failure<bool>(ErrorMessages.PlayerNotConnected);

            IsConnected = false;
            return OperationResult.Success(true);
        }

        public OperationResult<bool> AddResource(ResourceType resourceType, int value)
        {
            if (value < 0)
                return OperationResult.Failure<bool>(ErrorMessages.NegativeResourceValue);

            var resource = Resources.FirstOrDefault(r => r.Type == resourceType);
            if (resource == null)
                Resources.Add(new Resource(resourceType, value));
            
            
            var result = resource.Increase(value);
            if (!result.IsSuccess)
                return result;
            

            return OperationResult.Success(true);
        }

        public OperationResult<bool> DeductResource(ResourceType resourceType, int value)
        {
            var resource = Resources.FirstOrDefault(r => r.Type == resourceType);
            if (resource == null)
                return OperationResult.Failure<bool>(string.Format(ErrorMessages.ResourceNotFound, resourceType));

            var result = resource.Decrease(value);
            
            return !result.IsSuccess ? result : OperationResult.Success(true);
        }
    }
}
