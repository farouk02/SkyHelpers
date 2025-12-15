using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SkyHelpers
{
    public class WhatsApp(string accountSid, string authToken, string fromNumber = "+213555338866")
    {
        private readonly string _accountSid = accountSid;
        private readonly string _authToken = authToken;
        private readonly string _fromNumber = fromNumber;

        public async Task<MessageResource> SendMessageAsync(string to, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            string number;
            if (to.StartsWith('+'))
            {
                number = to;
            }
            else if (to.StartsWith("00"))
            {
                // Handle international dialing prefix "00" by replacing it with "+"
                number = string.Concat("+", to.AsSpan(2));
            }
            else
            {
                // Default to Algeria (+213) if no country code provided, for backward compatibility/user preference
                number = "+213" + to;
            }

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("whatsapp:" + number)
            )
            {
                From = new PhoneNumber("whatsapp:" + _fromNumber),
                Body = message
            };

            return await MessageResource.CreateAsync(messageOptions);
        }
    }
}
