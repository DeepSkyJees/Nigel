// // ***********************************************************************
// // Assembly         : Nigel.Basic.UnitTest
// // Author           : yuliang
// // Created          : 09:08
// //
// // Last Modified By : yuliang
// // Last Modified On : 09:08
// // ***********************************************************************
// // <copyright file="ObjectExtensionTester.cs" company="华规软件">
// //     Copyright (c) . All rights reserved.
// // </copyright>
// // <summary></summary>
// // ***********************************************************************

using System;
using Xunit;

namespace Nigel.Basic.UnitTest;

public class ObjectExtensionTester
{
    [Fact]
    public void GenJson()
    {
        var jsonObject = new
        {
            Name = "Nigel",
            IsTrue = true,
            Birth = "2018/07/07".ToDateTime().ToChinaDateTime(),
        };
        var jsonString = jsonObject.ToJson();
        Assert.Equal(jsonString, "{\"name\":\"Nigel\",\"isTrue\":1,\"birth\":\"2018-07-07T00:00:00Z\"}");
    }

    [Fact]
    public void ToMsgPackJson() {
        var jsonObject = new JsonObject
        {
            Name = "Nigel",
            IsTrue = true,
            Birth = "2018/07/07".ToDateTime().ToChinaDateTime(),
        };
        var jsonString = jsonObject.ToMsgPackJson();
        Assert.Equal(jsonString, "{\"Name\":\"Nigel\",\"IsTrue\":true,\"Birth\":5248351202427387904}");

    }
}