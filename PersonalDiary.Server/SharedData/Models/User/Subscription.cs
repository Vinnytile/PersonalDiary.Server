﻿using System;

namespace SharedData.Models.User
{
    public class Subscription
    {
        // Id
        public Guid Id { get; set; }

        // General
        public Guid SubscriberFID { get; set; }
        public UserIdentity Subscriber { get; set; }

        public Guid ObservableFID { get; set; }
        public UserIdentity Observable { get; set; }
    }
}
