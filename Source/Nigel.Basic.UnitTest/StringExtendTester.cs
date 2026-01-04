using Xunit;

namespace Nigel.Basic.UnitTest
{
    public class StringExtendTester
    {
        [Fact]
        public void StringToDateTime()
        {
            var dateTime = "20180707".ToDateTime();

            dateTime = "2018/07/07".ToDateTime();

            dateTime = "2018-07-07".ToDateTime();

            dateTime = "20180707000000".ToDateTime();

            dateTime = "1722441611112111".ToUtcDateTimeFromTimestamp();
        }

        [Fact]
        public void ToWithMsgPack()
        {
            var jsonObject = new JsonObject
            {
                Name = "Nigel",
                IsTrue = true,
                Birth = "2018/07/07".ToDateTime().ToChinaDateTime(),
            };
            var jsonString = jsonObject.ToMsgPackJson();
            var obj = jsonString.To<JsonObject>();

        }
    }
}