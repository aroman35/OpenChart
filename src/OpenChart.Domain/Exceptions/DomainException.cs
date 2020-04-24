using System;

namespace OpenChart.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public readonly Type DomainType;

        public DomainException(Type domainType) : base($"An exception was thrown in ${domainType.Name}")
        {
            DomainType = domainType;
        }

        public DomainException(Type domainType, string message) : base(message)
        {
            DomainType = domainType;
        }
    }
}