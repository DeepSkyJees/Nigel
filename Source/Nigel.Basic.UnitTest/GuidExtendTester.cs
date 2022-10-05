using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nigel.Basic.UnitTest
{

    public class GuidExtendTester
    {
        [Fact]
        public void GenGuid()
        {
            var dateTime = DateTime.Now.ToChinaDateTime();
            var newGuid = GuidGenerator.GenerateTimeBasedGuid(dateTime);
            Assert.NotNull(newGuid);

            var dateTimeResult = GuidGenerator.GetUtcDateTime(newGuid).ToChinaDateTimeFormUtc();

            Assert.Equal(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeResult.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
