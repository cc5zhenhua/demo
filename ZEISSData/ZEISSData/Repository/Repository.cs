using System;
using System.Collections.Generic;
using System.Linq;
using ZEISSData.Models;

namespace ZEISSData.Repository
{
    public interface IRepository
    {
        IEnumerable<MessageModel> GetMessage(Func<MessageEntity, bool> condition,int limit =20);
        void SaveDataToDatabase(MessageModel model);
        void InitializeDbSchema();
    }

    public class Repository : IRepository
    {
        private readonly MachineLogMemoryDbContext dbContext;

        public Repository(MachineLogMemoryDbContext dbContext)
        {
            this.dbContext = dbContext;          
        }

        public void SaveDataToDatabase(MessageModel model)
        {
            dbContext.Messages.Add(new MessageEntity
            {
                id = model.payload.id,
                //join_ref = model.join_ref,
                machine_id = model.payload.machine_id,
                status = model.payload.status,
                timestamp = model.payload.timestamp,
                topic = model.topic,
                _event = model._event,
                _ref = model._ref
            });

            dbContext.SaveChanges();
        }


        public IEnumerable<MessageModel> GetMessage(Func<MessageEntity,bool> condition, int limit)
        {
            return dbContext.Messages.Where(condition).Select(x => new MessageModel
            {
                //join_ref = x.join_ref,
                topic = x.topic,
                _event = x._event,
                _ref = x._ref,
                payload = new Payload {  id = x.id, machine_id = x.machine_id, status = x.status, timestamp = x.timestamp}
            }).Take(limit) ;
        }

        public void InitializeDbSchema()
        {
            dbContext.Database.EnsureCreated();
        }
    }
}
