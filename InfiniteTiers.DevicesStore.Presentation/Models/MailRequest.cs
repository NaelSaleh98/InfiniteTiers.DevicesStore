﻿using InfiniteTiers.DevicesStore.Data.Models;
using System;


namespace InfiniteTiers.DevicesStore.Presentation.Models
{
    public class MailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void PrepeareDeviceRequestBody(Device device, ApplicationUser requester)
        {
            string href = "/" + device.DeviceId + "?userId=" + requester.Id;
            string accept = "<a href='https://localhost:44339/TransactionsHistory/Accept" + href + "'> Accept </a>";
            string deny = "<a href='https://localhost:44339/TransactionsHistory/Deny" + href + "'> Deny </a>";
            this.Body = String.Format("{0} request {1} from You."+ accept +" | " + deny, requester.UserName, device.Name);
        }

        public void PrepeareDeviceRequestAcceptBody(Device device)
        {
            this.Body = String.Format("Your request to {0} was accespted.", device.Name);
        }

        public void PrepeareDeviceRequestDenyBody(Device device)
        {
            this.Body = String.Format("{0} denied your request to have {1}.",device.OwnedBy.UserName, device.Name);
        }
    }
}
