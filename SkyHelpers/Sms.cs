using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading.Tasks;

namespace SkyHelpers
{
    public class Sms(string accountSid, string authToken, string fromNumber)
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
                // Default to Algeria (+213) if no country code provided
                // Also handle cases where user might put local 0 (e.g. 055...) -> +21355...
                if (to.StartsWith('0'))
                {
                    number = string.Concat("+213", to.AsSpan(1));
                }
                else
                {
                    number = "+213" + to;
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
