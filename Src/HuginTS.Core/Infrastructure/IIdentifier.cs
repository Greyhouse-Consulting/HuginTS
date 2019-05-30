using System;
using MongoDB.Bson;

namespace HuginTS.Core.Infrastructure
{
	public interface IIdentifier
	{
		ObjectId Id { get; set; }
	}
}