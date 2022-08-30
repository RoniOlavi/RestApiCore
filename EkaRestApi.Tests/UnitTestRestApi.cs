using Xunit;
using Alba;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RestApiCore.Models;

namespace RestApiCore.Tests
{
    public class UnitTestRestApi
    {
        [Fact]
        public async Task HaeKaikki()
        {
            var hostBuilder = RestApiCore.Program.CreateHostBuilder(new string[0]);
            using (var system = new SystemUnderTest(hostBuilder))
            {
                await system.Scenario(s =>
                {
                    s.Get.Url("/api/users");
                    s.StatusCodeShouldBeOk();
                });
                var results = await system.GetAsJson<IEnumerable<User>>("/api/users");

                Assert.NotEmpty(results);
                Assert.Equal(5, results.Count());
            }
        }

        [Fact]
        public async Task HaeYksi()
        {
            var hostBuilder = RestApiCore.Program.CreateHostBuilder(new string[0]);
            using (var system = new SystemUnderTest(hostBuilder))
            {
                await system.Scenario(s =>
                {
                    s.Get.Url("/api/users/85");
                    s.StatusCodeShouldBe(204);

                    s.Get.Url("/api/kurssit/80");
                    s.StatusCodeShouldBe(200);

                    s.Get.Url("/api/user");
                    s.StatusCodeShouldBe(404);
                });
            }
        }

        [Fact]
        public async Task Lis‰‰Uusi()
        {
            var hostBuilder = RestApiCore.Program.CreateHostBuilder(new string[0]);
            using (var system = new SystemUnderTest(hostBuilder))
            {
                var tulokset = await system.GetAsJson<IEnumerable<User>>("/api/users");
                var lasku = tulokset.Count();

                var k‰ytt‰j‰ = new User { FirstName = "Testi", LastName = "K‰ytt‰j‰", UserName = "Testi", Password = "K‰ytt‰j‰", AccesslevelId = 2 };

                await system.Scenario(s =>
                {
                    s.Post.Json<User>(k‰ytt‰j‰).ToUrl("/api/users");
                    s.StatusCodeShouldBe(200);
                });

                var uusitulos = await system.GetAsJson<IEnumerable<User>>("/api/users");
                Assert.Equal(lasku +1,uusitulos.Count());
            }
        }

        [Fact]
        public async Task PoistaViimeisin()
        {
            var hostBuilder = RestApiCore.Program.CreateHostBuilder(new string[0]);
            using (var system = new SystemUnderTest(hostBuilder))
            {
                //Tarkistetaan k‰ytt‰jien lukum‰‰r‰ ennen poistoa
                var results1 = await system.GetAsJson<IEnumerable<User>>("/api/users");
                var count1 = results1.Count();

                // Etsit‰‰n suurin id linq:lla ja max-metodilla
                int suurinId = (from r in results1 select r.UserId).ToArray().Max();

                // Poistetaan kurssin, jolla on suurin id
                await system.Scenario(s =>
                {
                    s.Delete.Url("/api/users/" + suurinId);
                    s.StatusCodeShouldBe(200);
                });

                ////Tarkistetaan kurssien lukum‰‰r‰ poston j‰lkeen
                var results2 = await system.GetAsJson<IEnumerable<User>>("/api/users");
                Assert.Equal(count1 - 1, results2.Count()); // Varmistaa, ett‰ k‰ytt‰jien m‰‰r‰ on lis‰‰ntynyt yhdell‰, lis‰yksen j‰lkeen
            }
        }
    }
}
