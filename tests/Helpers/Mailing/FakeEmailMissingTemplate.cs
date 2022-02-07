using Gwenael.Application.Mailing;

namespace Gwenael.Tests.Helpers.Mailing
{
    public class FakeEmailMissingTemplate : EmailModelBase
    {
        public FakeEmailMissingTemplate(string subject, string to) : base(subject, to)
        {
        }
    }
}