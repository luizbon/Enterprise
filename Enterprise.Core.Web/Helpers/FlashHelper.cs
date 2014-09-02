using System.Collections.Generic;

namespace Enterprise.Core.Web.Helpers
{
    public class FlashHelper
    {
        private readonly IDictionary<string, object> _dictionary;

        public FlashHelper(IDictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void Attention(string message)
        {
            AddOrUpdate(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            AddOrUpdate(Alerts.SUCCESS, message);
        }

        public void AddOrUpdate(string message)
        {
            AddOrUpdate(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            AddOrUpdate(Alerts.ERROR, message);
        }

        public void Details(string message)
        {
            AddOrUpdate(Alerts.DETAILS, message);
        }

        private void AddOrUpdate(string type, string message)
        {
            if (_dictionary.ContainsKey(type))
                _dictionary[type] = message;
            else
                _dictionary.Add(type, message);
        }
    }

    public static class Alerts
    {
        public const string SUCCESS = "success";
        public const string ATTENTION = "attention";
        public const string ERROR = "error";
        public const string INFORMATION = "info";
        public const string DETAILS = "details";

        public static string[] ALL
        {
            get
            {
                return new[]
                {
                    SUCCESS,
                    ATTENTION,
                    INFORMATION,
                    ERROR
                };
            }
        }
    }
}
