using System.Collections.Concurrent;
using System.Collections.Generic;
using ZEISSData.Models;
using ZEISSData.Repository;

namespace ZEISSData.Services
{
    public interface IMessageBusinessObject
    {
        IEnumerable<MessageModel> GetAllMessages();
        void TransferMessage(MessageModel workItem);
        void InitializeDbSchema();
    }

    public class MessageBusinessObject : IMessageBusinessObject
    {
        private readonly IRepository repo;

        public MessageBusinessObject(IRepository repo) {
            this.repo = repo;
        }
        public void TransferMessage(MessageModel workItem)
        {
            repo.SaveDataToDatabase(workItem);
        }


        public IEnumerable<MessageModel> GetAllMessages()
        {
            return repo.GetMessage(x=>true);
        }

        public void InitializeDbSchema()
        {
            repo.InitializeDbSchema();
        }
    }
}
