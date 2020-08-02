using System.Linq;
using System.Collections.Generic;

public interface IIdentifiable
{
    string ID { get; }
}

public static class IdentifiableExtensions
{
    public static IIdentifiable GetFromID(this IEnumerable<IIdentifiable> values, string id)
    {
        return values.FirstOrDefault(x => x.ID == id);
    }
}