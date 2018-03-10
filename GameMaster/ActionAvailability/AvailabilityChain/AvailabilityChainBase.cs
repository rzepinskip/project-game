using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability
{
    abstract class AvailabilityChainBase
    {
        protected AvailabilityChainBase nextLink;

        abstract protected bool Validate();

        public void SetNextLink(AvailabilityChainBase nextLink)
        {
            this.nextLink = nextLink;
        }

        public bool ValidateLink()
        {
            bool result = false;
            if (this.Validate())
            {
                if (nextLink == null)
                    result = true;
                else
                    result = nextLink.ValidateLink();
            }
            return result;
        }
    }
}
