using GameMaster.ActionAvailability.AvailabilityLink;


namespace GameMaster.ActionAvailability.AvailabilityChain
{
    class AvailabilityChainBuilder
    {
        private AvailabilityLinkBase first;
        private AvailabilityLinkBase last;

        public AvailabilityChainBuilder(AvailabilityLinkBase first) {
            this.first = first;
            this.last = first;
        }

        public AvailabilityChainBuilder AddNextLink(AvailabilityLinkBase nextLink) {
            this.last.SetNextLink(nextLink);
            this.last = nextLink;
            return this;
        }

        public AvailabilityLinkBase Build() {
            return first;
        }
    }
}
