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


    public class MyUsers : ServiceBase<IUsers>, IUsers
    {
        List<User> _all { get; } = new List<User> {
            new User{ Id=1,Name="laowang"},
            new User{ Id=2,Name="zhangsan"},
            new User{ Id=3,Name="lisi"},
            new User{ Id=4,Name="wangwu"},
        };

        public async UnaryResult<List<User>> GetAll() {
            return await new UnaryResult<List<User>>(_all);
        }

        public async UnaryResult<User> Get(int id) {
            var result = _all.FirstOrDefault(r => r.Id == id);
            return await new UnaryResult<User>(result);
        }
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
