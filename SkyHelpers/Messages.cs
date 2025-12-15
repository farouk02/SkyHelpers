using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SkyHelpers
{
    /// <summary>
    /// Helper class for sending WhatsApp and SMS messages via Twilio.
    /// </summary>
    /// <param name="accountSid">Twilio Account SID</param>
    /// <param name="authToken">Twilio Auth Token</param>
    /// <param name="fromNumber">Sender phone number (with country code for SMS, without 'whatsapp:' prefix for WhatsApp)</param>
    /// <param name="phoneCode">Default country code for local numbers (default: +213 for Algeria)</param>
    public class Messages(string accountSid, string authToken, string fromNumber, string phoneCode = "+213")
    {
        private readonly string _fromNumber = fromNumber;
        private readonly string _phoneCode = phoneCode;

        // Initialize Twilio client once in constructor
        private readonly bool _initialized = InitializeTwilio(accountSid, authToken);

        private static bool InitializeTwilio(string accountSid, string authToken)
        {
            TwilioClient.Init(accountSid, authToken);
            return true;
        }

        /// <summary>
        /// Sends a WhatsApp message to the specified phone number.
        /// </summary>
        /// <param name="to">Recipient phone number (can be local, with +, or with 00 prefix)</param>
        /// <param name="message">Message content to send</param>
        /// <returns>MessageResource containing the sent message details</returns>
        public MessageResource SendWhatsApp(string to, string message)
        {
            var number = NormalizePhoneNumber(to);

            var messageOptions = new CreateMessageOptions(new PhoneNumber($"whatsapp:{number}"))
            {
                From = new PhoneNumber($"whatsapp:{_fromNumber}"),
                Body = message
            };

            return MessageResource.Create(messageOptions);
        }

        /// <summary>
        /// Sends an SMS message to the specified phone number.
        /// </summary>
        /// <param name="to">Recipient phone number (can be local, with +, or with 00 prefix)</param>
        /// <param name="message">Message content to send</param>
        /// <param name="messagingServiceSid">Twilio Messaging Service SID</param>
        /// <returns>MessageResource containing the sent message details</returns>
        /// <exception cref="Exception">Throws if SMS sending fails</exception>
        public MessageResource SendSms(string to, string message, string messagingServiceSid)
        {
            var toNumber = NormalizePhoneNumber(to);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(toNumber))
            {
                MessagingServiceSid = messagingServiceSid,
                From = new PhoneNumber(_fromNumber),
                Body = message
            };

            return MessageResource.Create(messageOptions);
        }

        /// <summary>
        /// Normalizes a phone number to international format with + prefix.
        /// </summary>
        /// <param name="phoneNumber">Phone number in any format</param>
        /// <returns>Normalized phone number with + prefix</returns>
        private string NormalizePhoneNumber(string phoneNumber)
        {
            // Already has international prefix
            if (phoneNumber.StartsWith('+'))
                return phoneNumber;

            // International dialing prefix "00" -> replace with "+"
            if (phoneNumber.StartsWith("00"))
                return string.Concat("+", phoneNumber.AsSpan(2));

            // Local number starting with 0 (e.g., 055...) -> remove leading 0 and add country code
            if (phoneNumber.StartsWith('0'))
                return string.Concat(_phoneCode, phoneNumber.AsSpan(1));

            // Just digits without prefix -> add country code
            return _phoneCode + phoneNumber;
        }
    }
}
