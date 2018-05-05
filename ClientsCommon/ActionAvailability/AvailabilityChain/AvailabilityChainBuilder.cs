using ClientsCommon.ActionAvailability.AvailabilityLink;

namespace ClientsCommon.ActionAvailability.AvailabilityChain
{
    internal class AvailabilityChainBuilder
    {
        private readonly AvailabilityLinkBase first;
        private AvailabilityLinkBase last;

        public AvailabilityChainBuilder(AvailabilityLinkBase first)
        {
            this.first = first;
            last = first;
        }

        public AvailabilityChainBuilder AddNextLink(AvailabilityLinkBase nextLink)
        {
            last.SetNextLink(nextLink);
            last = nextLink;
            return this;
        }

        public AvailabilityLinkBase Build()
        {
            return first;
        }
    }
}