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
            var result = false;
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
