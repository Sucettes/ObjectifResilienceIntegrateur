using Gwenael.Application.Mailing;

namespace Gwenael.Tests.Helpers.Mailing
{
    public class FakeEmailModelBaseTemplate : EmailModelBase
    {
        public FakeEmailModelBaseTemplate(string subject, string to) : base(subject, to)
        {
        }
    }
}