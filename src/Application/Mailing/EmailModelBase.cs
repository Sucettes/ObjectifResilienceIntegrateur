using Spk.Common.Helpers.Guard;

namespace Gwenael.Application.Mailing
{
    public abstract class EmailModelBase
    {
        public string To { get; }
        public string Subject { get; }

        protected EmailModelBase(string subject, string to)
        {
            To = to.GuardIsNotNullOrWhiteSpace(nameof(to));
            Subject = subject.GuardIsNotNullOrWhiteSpace(nameof(subject));
        }
    }
}