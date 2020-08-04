using Audit.Core;
using Audit.EntityFramework;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace PunasMarketing
{
    public class MyCustomDataProvider : AuditDataProvider
    {
        private readonly string _directoryPath = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

        private string GetFileName(AuditEvent auditEvent)
        {
            try
            {
                EventEntry eventEntry = auditEvent.GetEntityFrameworkEvent().Entries.FirstOrDefault();
                if (eventEntry != null)
                {
                    string tableName = eventEntry.Table;
                    object id = eventEntry.PrimaryKey.FirstOrDefault().Value;
                    string action = eventEntry.Action;

                    return $"{tableName}_{action}_{id}_{DateTime.Now:yyyyMMddHHmmssffff}.json";
                }

                return $"Log{Guid.NewGuid()}.json";
            }
            catch
            {
                return $"Log{Guid.NewGuid()}.json";
            }
        }

        public override object InsertEvent(AuditEvent auditEvent)
        {
            try
            {
                string fileName = GetFileName(auditEvent);

                string path = Path.Combine(_directoryPath, fileName);

                File.WriteAllText(path, auditEvent.ToJson());
                return fileName;
            }
            catch
            {
                return $"Log{Guid.NewGuid()}.json";
            }
        }

        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            var fileName = eventId.ToString();
            File.WriteAllText(fileName, auditEvent.ToJson());
        }

        public override T GetEvent<T>(object eventId)
        {
            var fileName = eventId.ToString();
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
        }
    }
}