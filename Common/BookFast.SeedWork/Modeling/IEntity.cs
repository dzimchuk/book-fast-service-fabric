using System.Collections.Generic;

namespace BookFast.SeedWork.Modeling
{
    public interface IEntity
    {
        IEnumerable<Event> CollectEvents();
    }
}
