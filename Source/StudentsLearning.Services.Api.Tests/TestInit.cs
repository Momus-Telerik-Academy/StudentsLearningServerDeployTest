﻿#region

using System.Reflection;
using System.Web.Http;

using MyTested.WebApi;

using NUnit.Framework;

using StudentsLearning.Server.Api;

#endregion

// DO NOT PUT IN A NAMESPACE!!!
// It will only work for the current namespace and not all namespaces in the project.
[SetUpFixture]
public class TestInit
{
    [SetUp]
    public static void AssemblyInit()
    {
        AutoMapperConfig.RegisterMappings(Assembly.Load("StudentsLearning.Server.Api"));

        var config = new HttpConfiguration();
        WebApiConfig.Register(config);
        MyWebApi.IsUsing(config);
    }
}