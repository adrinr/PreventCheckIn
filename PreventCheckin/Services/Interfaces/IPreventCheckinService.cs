using System.Collections.Generic;

namespace PreventCheckin.Services.Interfaces
{
    public interface IPreventCheckinService
    {
        bool AreExcluded(params string[] files);

        void PreventCheckinStatus(IEnumerable<string> files);

        void UnpreventCheckinStatus(IEnumerable<string> files);
    }
}