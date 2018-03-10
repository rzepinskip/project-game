using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability
{
    class AvailabilityChainBuilder
    {
        AvailabilityChainBase first;
        AvailabilityChainBase last;

        public AvailabilityChainBuilder(AvailabilityChainBase first) {
            this.first = first;
            this.last = first;
        }

        public AvailabilityChainBuilder AddNextLink(AvailabilityChainBase nextLink) {
            this.last.SetNextLink(nextLink);
            this.last = nextLink;
            return this;
        }

        public AvailabilityChainBase Build() {
            return first;
        }
    }
}
