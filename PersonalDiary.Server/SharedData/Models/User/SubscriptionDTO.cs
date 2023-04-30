using System;

namespace SharedData.Models.User
{
    public class SubscriptionDTO
    {
        public Guid SubscriberFID { get; set; }

        public Guid ObservableFID { get; set; }
    }
}
