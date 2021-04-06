using HSM.WebApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HSM.WebApp.Services
{
    public interface ISmallIdService
    {
        string GetSmallId<TEntity>(TEntity entity) where TEntity : IIdentifiable;
        string GetFullId<TEntity>(string smallId) where TEntity : IIdentifiable;
    }

    public class SmallIdService : ISmallIdService
    {
        private readonly Dictionary<string, string> _smallIdSets = new Dictionary<string, string>();

        public string GetFullId<TEntity>(string smallId) where TEntity : IIdentifiable
        {
            if (_smallIdSets.TryGetValue(smallId, out var oldValueId))
                return oldValueId;
            return null;
        }

        public string GetSmallId<TEntity>(TEntity entity) where TEntity : IIdentifiable
        {
            var cMaxRun = 0;
            var key = GetKey(entity.Id, typeof(TEntity));
            while(_smallIdSets.TryGetValue(key, out var oldValueId))
            {
                if (oldValueId == entity.Id) return key;

                var nounce = Guid.NewGuid().ToString();
                key = GetKey(entity.Id, typeof(TEntity), nounce);
                cMaxRun++;
                if (cMaxRun >= 5) throw new InvalidOperationException($"Unable to create small ids");
            }
            _smallIdSets.Add(key, entity.Id);
            return key;
        }

        protected string GetKey(string id, Type typeOfInstance, string nounce = null)
        {
            return ComputeSha256Hash($"{nounce}{typeOfInstance.FullName}{id}").Substring(0, 8).ToUpper();
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
