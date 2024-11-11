using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElectricityBillPaymentSystem.Core.Services
{
    public static class MockNotificationService
    {
        private static readonly List<string> EventLog = new();

        public static Task PublishEvent<T>(string eventType, T data)
        {
            var datajson = JsonSerializer.Serialize(data);
            string eventMessage = $"Event: {eventType}, Data: {datajson}";

            EventLog.Add(eventMessage);
            Console.WriteLine(eventMessage);
            return Task.CompletedTask;
        }

        public static Task<IEnumerable<string>> GetEventLog()
        {
            return Task.FromResult(EventLog.AsEnumerable());
        }
    }

}
