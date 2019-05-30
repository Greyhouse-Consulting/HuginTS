using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace HuginTS.Core
{
    public class IdGenerator
    {


        public static Guid CreateGuidId(string id)
        {
            return new Guid(id.GetHashCode(), 0, 0, new byte[8]);
        }
        public static ObjectId CreateObjectId(DateTime timeStamp)
        {
            var hash = KeyGen(timeStamp.Year, timeStamp.Month, timeStamp.Day);
            return ObjectId.GenerateNewId(hash);
        }
        public static ObjectId CreateObjectId(string id)
        {
            return ObjectId.GenerateNewId(id.GetHashCode());
        }


        public static int KeyGen(int a, int b, int c)
        {
            var aHash = a.GetHashCode();
            var bHash = b.GetHashCode();
            var cHash = c.GetHashCode();
            var hash = 36469;
            unchecked
            {
                hash = hash * 17 + aHash;
                hash = hash * 17 + bHash;
                hash = hash * 17 + cHash;
            }
            return hash;
        }
    }
}
