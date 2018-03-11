using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.ActionAvailability.AvailabilityLink
{
    abstract class AvailabilityLinkBase
    {
        protected AvailabilityLinkBase nextLink;

        abstract protected bool Validate();

        public void SetNextLink(AvailabilityLinkBase nextLink)
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
