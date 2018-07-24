using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public interface IUsers : IGrainWithIntegerKey
    {
        Task<List<User>> All();

        Task<User> Get(int id);
    }


    public class MyUsers : Grain, IUsers
    {
        List<User> _all { get; } = new List<User> {
            new User{ Id=1,Name="laowang"},
            new User{ Id=2,Name="zhangsan"},
            new User{ Id=3,Name="lisi"},
            new User{ Id=4,Name="wangwu"},
        };

        public Task<List<User>> All() {
            return Task.FromResult(_all);
        }

        public Task<User> Get(int id) {
            var result = _all.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(result);
        }
    }

    public class User
    {
        public int Id { get; set; }


        public string Name { get; set; }
    }
}
