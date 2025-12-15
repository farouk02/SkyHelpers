using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SkyHelpers
{
    public class Messages(string accountSid, string authToken, string fromNumber, string phoneCode = "+213")
    {
        private readonly string _accountSid = accountSid;
        private readonly string _authToken = authToken;
        private readonly string _fromNumber = fromNumber;
        private readonly string _phoneCode = phoneCode;

        public async Task<MessageResource> SendWhatsAppAsync(string to, string message)
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
                number = _phoneCode + to;
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

        public async Task<MessageResource> SendSmsAsync(string to, string message, string messagingServiceSid)
        {
            TwilioClient.Init(_accountSid, _authToken);

            string toNumber;
            if (to.StartsWith('+'))
            {
                toNumber = to;
            }
            else if (to.StartsWith("00"))
            {
                // Handle international dialing prefix "00" by replacing it with "+"
                toNumber = string.Concat("+", to.AsSpan(2));
            }
            else
            {
                // Default to provided phone code (default +213) if no country code provided
                // Also handle cases where user might put local 0 (e.g. 055...) -> +21355...
                if (to.StartsWith('0'))
                {
                    toNumber = string.Concat(_phoneCode, to.AsSpan(1));
                }
                else
                {
                    toNumber = _phoneCode + to;
                }
            }

            CreateMessageOptions messageOptions = new(new PhoneNumber(toNumber))
            {
                MessagingServiceSid = messagingServiceSid,
                From = new PhoneNumber(_fromNumber),
                Body = message
            };

            try
            {
                return await MessageResource.CreateAsync(messageOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SMS was not sent!\n" + ex);
                throw;
            }
        }
    }
}
