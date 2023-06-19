namespace Feipder.Entities.RequestModels
{
    public class PhoneCallRequest
    {
        public string apiKey { get; set; }
        public SmsFormat[] sms { get; set; }
    }
}
