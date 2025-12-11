using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading.Tasks;

namespace SkyHelpers
{
    public class Sms
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;

        public Sms(string accountSid, string authToken, string fromNumber)
        {
            _accountSid = accountSid;
            _authToken = authToken;
            _fromNumber = fromNumber;
        }

        public async Task<MessageResource> SendAsync(string to, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            string number;
            if (to.StartsWith("+"))
            {
                number = to;
            }
            else if (to.StartsWith("00"))
            {
                // Handle international dialing prefix "00" by replacing it with "+"
                number = "+" + to.Substring(2);
            }
            else
            {
                // Default to Algeria (+213) if no country code provided
                // Also handle cases where user might put local 0 (e.g. 055...) -> +21355...
                if (to.StartsWith("0"))
                {
                    number = "+213" + to.Substring(1);
                }
                else
                {
                    number = "+213" + to;
                }
            }

            var messageOptions = new CreateMessageOptions(new PhoneNumber(number))
            {
                From = new PhoneNumber(_fromNumber),
                Body = message
            };

            return await MessageResource.CreateAsync(messageOptions);
        }
    }
}
