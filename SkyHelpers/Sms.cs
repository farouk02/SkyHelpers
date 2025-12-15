using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SkyHelpers
{
    public class Sms(string accountSid, string authToken, string fromNumber, string phoneCode = "+213")
    {
        public async Task<MessageResource> SendAsync(string to, string message)
        {
            TwilioClient.Init(accountSid, authToken);

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
                // Default to provided phone code (default +213) if no country code provided
                // Also handle cases where user might put local 0 (e.g. 055...) -> +21355...
                if (to.StartsWith('0'))
                {
                    number = string.Concat(phoneCode, to.AsSpan(1));
                }
                else
                {
                    number = phoneCode + to;
                }
            }

            var messageOptions = new CreateMessageOptions(new PhoneNumber(number))
            {
                From = new PhoneNumber(fromNumber),
                Body = message
            };

            return await MessageResource.CreateAsync(messageOptions);
        }
    }
}
