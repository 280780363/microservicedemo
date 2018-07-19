using Grpc.Core;
using MagicOnion;
using MagicOnion.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoClient
{
    public interface IUsers : IService<IUsers>
    {
        UnaryResult<List<User>> GetAll();

        UnaryResult<User> Get(int id);
    }

    public class User : Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    [MessagePack.MessagePackObject(true)]

    public abstract class Entity
    {

    }
}
