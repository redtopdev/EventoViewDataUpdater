using Engaze.Core.DataContract;
using System;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public interface IViewDataRepository
    {
        Task InsertAsync(Event @event);

        Task DeleteAsync(Guid eventId);

        Task ExtendEventAsync(Guid eventId, DateTime endTime);

        Task EndEventAsync(Guid eventId);

        Task UpdateParticipantStateAsync(Guid eventId, Guid participantId, int stateId);
       
    }
}
