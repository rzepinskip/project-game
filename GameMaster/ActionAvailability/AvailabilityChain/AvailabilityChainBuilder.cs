using GameMaster.ActionAvailability.AvailabilityLink;


namespace GameMaster.ActionAvailability
{
    class AvailabilityChainBuilder
    {
        AvailabilityLinkBase first;
        AvailabilityLinkBase last;
        private IsPieceInCurrentLocationLink isPieceInCurrentLocationLink;

        public AvailabilityChainBuilder(AvailabilityLinkBase first) {
            this.first = first;
            this.last = first;
        }

        public AvailabilityChainBuilder(IsPieceInCurrentLocationLink isPieceInCurrentLocationLink)
        {
            this.isPieceInCurrentLocationLink = isPieceInCurrentLocationLink;
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
