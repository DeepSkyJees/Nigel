using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nigel.FlakeGen;
using Xunit;
using Xunit.Abstractions;

namespace Nigel.Basic.UnitTest
{
    public class FlakeGenTester(ITestOutputHelper testOutputHelper)
    {
        [Fact]
        public async Task GenId()
        {
            var idGenerator =
                new IdStringGeneratorWrapper(
                    new Id64Generator(), IdStringGeneratorWrapper.Base32);
            ConcurrentBag<string> conList = new ConcurrentBag<string>();
            await Parallel.ForAsync(0, 100000, async (item, _) =>
            {
                var id = idGenerator.GenerateId();
                conList.Add(id);
                await Task.CompletedTask;
            });

            if (conList.GroupBy(p => p).Count() == 100000)
            {
                foreach (var item in conList)
                {
                    testOutputHelper.WriteLine($"Id:{item}");
                }
            }
        }

        [Fact]
        public async Task GenId2()
        {
            var idGenerator = new IdGuidGenerator(20, DateTime.Now);
            ConcurrentBag<string> conList = new ConcurrentBag<string>();
            await Parallel.ForAsync(0, 10000, async (item, _) =>
            {
                var id = idGenerator.GenerateId();
                conList.Add(id.ToGuidString());
                await Task.CompletedTask;
            });

            if (conList.GroupBy(p => p).Count() == 10000)
            {
                foreach (var item in conList)
                {
                    testOutputHelper.WriteLine($"Id:{item}");
                }
            }
        }

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
            //foreach (var item in list.GroupBy(p => p))
            //{
            //    Debug.WriteLine($"{item.Key},{item.Count() > 1}");
            //}
            ConcurrentBag<Guid> conList = new ConcurrentBag<Guid>();
            await Parallel.ForAsync(0, 10000, async (item, _) =>
            {
                var dateTime = DateTime.Now.ToChinaDateTime();
                var newGuid = GuidGenerator.GenerateTimeBasedGuid(dateTime);
                conList.Add(newGuid);
                await Task.CompletedTask;
            });

            if (conList.GroupBy(p => p).Count() == 10000)
            {
                foreach (var item in conList)
                {
                    var dateTimeResult = GuidGenerator.GetUtcDateTime(item).ToChinaDateTimeFormUtc();
                    testOutputHelper.WriteLine($"GUID:{item},TIME:{dateTimeResult}");
                }
            }
        }
    }
}