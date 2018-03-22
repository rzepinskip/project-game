namespace Common.ActionAvailability.AvailabilityLink
{
    internal abstract class AvailabilityLinkBase
    {
        protected AvailabilityLinkBase nextLink;

        protected abstract bool Validate();

        public void SetNextLink(AvailabilityLinkBase nextLink)
        {
            this.nextLink = nextLink;
        }

        public bool ValidateLink()
        {
            var result = false;
            if (Validate())
                if (nextLink == null)
                    result = true;
                else
                    result = nextLink.ValidateLink();
            return result;
        }
    }
}