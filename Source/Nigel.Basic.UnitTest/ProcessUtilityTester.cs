using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nigel.Basic.Utility;
using Xunit;

namespace Nigel.Basic.UnitTest
{
    public class ProcessUtilityTester
    {
        [Fact]
        public async Task GetCodeProcessId()
        {
            var id = ProcessUtility.GetProcessIdByName("Code.exe");
            Assert.NotNull(id);
        }
    }
}
