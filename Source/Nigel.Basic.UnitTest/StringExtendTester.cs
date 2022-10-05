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
        }
    }
}