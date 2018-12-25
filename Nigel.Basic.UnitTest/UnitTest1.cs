using Xunit;

namespace Nigel.Basic.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var dateTime = "20180707".ToDateTime();

            dateTime = "2018/07/07".ToDateTime();

            dateTime = "2018-07-07".ToDateTime();

            dateTime = "20180707000000".ToDateTime();
        }
    }
}