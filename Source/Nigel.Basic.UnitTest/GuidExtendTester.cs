using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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

        [Fact]
        public async Task ParGenGuid()
        {
            List<Guid> list = new List<Guid>();
            for (int i = 0; i < 100000; i++)
            {
                var dateTime = DateTime.Now.ToChinaDateTime();
                var newGuid = GuidGenerator.GenerateTimeBasedGuid(dateTime);
                list.Add(newGuid);
            }
            if (list.GroupBy(p => p).Count() == 100000)
            {
            }
            //foreach (var item in list.GroupBy(p => p))
            //{
            //    Debug.WriteLine($"{item.Key},{item.Count() > 1}");
            //}
            ConcurrentBag<Guid> conList = new ConcurrentBag<Guid>();
            await Parallel.ForAsync(1, 10, async (item, _) =>
            {
                var dateTime = DateTime.Now.ToChinaDateTime();
                var newGuid = GuidGenerator.GenerateTimeBasedGuid(dateTime);
                conList.Add(newGuid);
                await Task.CompletedTask;
            });

            //foreach (var item in conList)
            //{
            //    Debug.WriteLine(item);
            //}
        }
    }
}