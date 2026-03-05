using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WinLimitAPI;

public class User : MongoIdentityUser<Guid>
{
    
}